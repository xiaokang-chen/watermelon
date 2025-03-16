public enum TTSAudioFormmat {
    PCM,
    WAV,
    MP3
}

[System.Serializable]
public class TTSRequest
{
    // 项目appkey required
    public string appkey;
    // 待合成的文本，需要为UTF-8编码 required
    public string text;
    // 若不设置token参数，需要在HTTP Headers中设置X-NLS-Token字段来指定Token。
    public string token;
    // 音频编码格式，支持PCM/WAV/MP3格式。默认值：pcm。

    //[JsonConverter(typeof(StringEnumConverter))]
    public string formmat;

    // 音频采样率，支持16000 Hz和8000 Hz，默认值：16000 Hz。
    public int sample_rate;

    // 发音人，默认值：xiaoyun。更多发音人请参见接口说明。
    public string voice;
    // 音量，取值范围：0~100，默认值：50。
    public int volume;
    // 语速，取值范围：-500~500，默认值：0。
    public int speech_rate;

    // 语调，取值范围：-500~500，默认值：0。
    public int pitch_rate;

    public TTSRequest(string appkey, string text)
	{
        this.appkey = appkey;
        this.text = text;
        sample_rate = 16000;
        formmat = "WAV";
        //voice = "xiaoyun";
        //volume = 50;
        //speech_rate = 0;
        //pitch_rate = 0;
    }
}

[System.Serializable]
public class TTSResponseError
{
    // 32位请求任务ID，请记录该值，用于排查错误。
    public string task_id;
    // 服务结果
    public string result;
    // 服务状态码
    public int status;
    // 服务状态描述
    public string message;

    public TTSResponseError(int status, string message)
    {
        this.status = status;
        this.message = message;
    }
}

