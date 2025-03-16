using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;


public class TTSService 
{
    private static TTSService instance;

    public static TTSService Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new TTSService();
            }

            return instance;
        }
    }

    private const string TTS_TOKEN  = "TOKEN";


    private NlsToken token;

    public  const string NLS_APP_KEY = "LTAI5tBbcC9FejHhNYy3pDF4";
    private const string NLS_APP_SECRET = "U4losdbE2Jq6tUibNXmduwdQdH7FkH";
    public const string TTS_APP_KEY = "bPaLYkY6kg0fP0ki";

    private TTSService() {
        var sToken = PlayerPrefs.GetString(TTS_TOKEN);
        if (0 != sToken.Length) {
            var o = JsonUtility.FromJson<NlsResponse>(sToken);
            token = o.Token;
        }
    }


    ~TTSService()
    {
       token = null;
    }


    public void PostTTS(TTSRequest request, Action<byte[]> onSuccess, Action<TTSResponseError> onError)
    {
        const string url = "https://nls-gateway-cn-beijing.aliyuncs.com/stream/v1/tts";

        if (0 == request.appkey.Length)
        {
            Debug.Log("user default tts appkey");
            request.appkey = TTS_APP_KEY;
        }

        long utcNow = DateTimeUtils.GetTimestampToSeconds();
        if (null == token || token.ExpireTime <= utcNow)
        {
            void success(NlsResponse reponse)
            {
                token = reponse.Token;
                PostTTS(request, onSuccess, onError);
            };
            Action<NlsResponseError> error = (NlsResponseError reponse) => {
                onError?.Invoke(new TTSResponseError(int.Parse(reponse.Code), reponse.Message));
            };
            GetToken(NLS_APP_KEY, NLS_APP_SECRET, success, error);
            return;
        }

        request.token = token.Id;


        // Convert the JSON string to a byte array
        var json = JsonConvert.SerializeObject(request);
        Debug.Log("json send:" + json);
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

        var uwr = new UnityWebRequest(url, "POST")
        {
            // Attach the JSON byte array to the request
            uploadHandler = new UploadHandlerRaw(jsonToSend),
            // Create a new download handler to receive the response
            downloadHandler = new DownloadHandlerBuffer()
        };

        // Set the content type to JSON
        uwr.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        var asyncOperation =  uwr.SendWebRequest();

        asyncOperation.completed += (AsyncOperation op) => {
            // Check for errors
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + uwr.error);
                onError?.Invoke(new TTSResponseError((int)uwr.responseCode, uwr.error));
            }
            else
            {
                var responseContentType = uwr.GetResponseHeader("Content-Type");
                if (0 == responseContentType.CompareTo("audio/mpeg"))
                {
                    onSuccess?.Invoke(uwr.downloadHandler.data);
                }
                else
                {
                    string responseText = uwr.downloadHandler.text;
                    Debug.Log("Response: " + responseText);
                    var responseObject = JsonUtility.FromJson<TTSResponseError>(responseText);
                    onError?.Invoke(responseObject);
                }
            }
            uwr.uploadHandler.Dispose();
            uwr.downloadHandler.Dispose();
        };
    }


    public void GetToken(string accessKeyId, string accessKeySecret, Action<NlsResponse> onSuccess, Action<NlsResponseError> onError)
    {
        const string url = "http://nls-meta.cn-shanghai.aliyuncs.com";

        Dictionary<string, string> dicts = new()
        {
            ["AccessKeyId"] = accessKeyId,
            ["Action"] = "CreateToken",
            ["Version"] = "2019-02-28",
            ["Format"] = "JSON",
            ["RegionId"] = "cn-shanghai",
            ["Timestamp"] = DateTimeUtils.UtcTimestamp(),
            ["SignatureMethod"] = "HMAC-SHA1",
            ["SignatureVersion"] = "1.0",
            ["SignatureNonce"] = UUIDGenerator.Generate()
        };

        var norm = NormalizeParam(dicts);
        var encodedNorm = "GET&" + WebUtility.UrlEncode("/") + "&" + WebUtility.UrlEncode(norm);
        var sign = HmacSha1Utils.Hash($"{accessKeySecret}&", encodedNorm);
        var finalParam = "Signature=" + WebUtility.UrlEncode(sign) + "&" + norm;
        // Create a new UnityWebRequest for a POST request
        var finalUrl = $"{url}/?{finalParam}";
        var uwr = new UnityWebRequest(finalUrl, "GET")
        {
            // Create a new download handler to receive the response
            downloadHandler = new DownloadHandlerBuffer()
        };

        var asyncOperation = uwr.SendWebRequest();

        asyncOperation.completed += (AsyncOperation op) =>
        {

            // Check for errors
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + uwr.error);
                onError?.Invoke(new NlsResponseError(uwr.responseCode.ToString(), uwr.error));
            }
            else
            {
                // Get the response body
                string responseText = uwr.downloadHandler.text;
                Debug.Log("Response: " + responseText);

                if (200 == uwr.responseCode)
                {
                    var responseObject = JsonUtility.FromJson<NlsResponse>(responseText);
                    PlayerPrefs.SetString(TTS_TOKEN, responseText);
                    onSuccess?.Invoke(responseObject);
                }
                else
                {
                    var responseObject = JsonUtility.FromJson<NlsResponseError>(responseText);
                    onError?.Invoke(responseObject);
                }
            }
            uwr.downloadHandler.Dispose();
        };
    }

    private string NormalizeParam(Dictionary<string, string> queryParams) {
        if (queryParams == null || queryParams.Count == 0)
        {
            return "";
        }

        // Sort query parameters by key names
        List<KeyValuePair<string, string>> sortedParams = new(queryParams);
        sortedParams.Sort((x, y) => x.Key.CompareTo(y.Key));

        // Build canonicalized query string
        StringBuilder canonicalizedQueryString = new StringBuilder();
        foreach (var param in sortedParams)
        {
            canonicalizedQueryString.Append("&")
                .Append(WebUtility.UrlEncode(param.Key))
                .Append("=")
                .Append(WebUtility.UrlEncode(param.Value));
        }

        // Remove the leading '&' character
        return canonicalizedQueryString.ToString()[1..];
    }
}
