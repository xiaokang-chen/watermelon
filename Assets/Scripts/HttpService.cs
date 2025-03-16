using System;
using UnityEngine;
using UnityEngine.Networking;

public class HttpService 
{
    private static HttpService instance;

    public static HttpService Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new HttpService();
            }
            return instance;
        }
    }

    private const string PROMPT_APP_ID = "4b432593e6804206bbeb1a943d08a478";

    public void PostPrompt(PromptRequest request, Action<PromptResponse> onSuccess, Action<string> onError) {
        var appId = 0 == request.appId.Length ? PROMPT_APP_ID : request.appId;
        string url = $"https://dashscope.aliyuncs.com/api/v1/apps/{appId}/completion";
 

        // Convert the JSON string to a byte array
        var json = JsonUtility.ToJson(request);
        Debug.Log("json send:" + json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        // Create a new UnityWebRequest for a POST request
        var uwr = new UnityWebRequest(url, "POST")
        {
            // Attach the JSON byte array to the request
            uploadHandler = new UploadHandlerRaw(jsonToSend),
            // Create a new download handler to receive the response
            downloadHandler = new DownloadHandlerBuffer()
        };

        // Set the content type to JSON
        uwr.SetRequestHeader("Content-Type", "application/json");

        // api_key sk-0cc6649220c144c4bb14db1e4e1abb50

        const string apiKey = "sk-0cc6649220c144c4bb14db1e4e1abb50";
        uwr.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        var asyncOperation = uwr.SendWebRequest();
        // Send the request and wait for the response
        asyncOperation.completed += (AsyncOperation op) =>
        {

            // Check for errors
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + uwr.error);
                onError?.Invoke(uwr.error);
            }
            else
            {
                // Get the response body
                string responseText = uwr.downloadHandler.text;
                Debug.Log("Response: " + responseText);

                // Parse the JSON response (if needed)
                // For example, if the response is a JSON object
                var responseObject = JsonUtility.FromJson<PromptResponse>(responseText);
                onSuccess?.Invoke(responseObject);
            }
            uwr.uploadHandler.Dispose();
            uwr.downloadHandler.Dispose();
        };

    }
}
