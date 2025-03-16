[System.Serializable]
public class NlsToken
{
    // 请求分配的Token值。
    public string Id;
    // Token的有效期时间戳 单位：秒
    public long ExpireTime;
}

[System.Serializable]
public class NlsResponse
{
    public NlsToken Token;
    public string NlsRequestId;
    public string RequestId;
}

[System.Serializable]
public class NlsResponseError
{
    public string RequestId;
    // 失败响应的错误信息
    public string Message;
    // 失败响应的错误码
    public string Code;

    public NlsResponseError(string Code,  string Message) {
        this.Code = Code;
        this.Message = Message;
    }
}


