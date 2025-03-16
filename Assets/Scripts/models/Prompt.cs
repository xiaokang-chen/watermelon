using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class PromptInput
{
    public string prompt;
    public PromptInput(string prompt)
    {
        this.prompt = prompt;
    }
}
[System.Serializable]
public class PromptRequest
{
    public PromptInput input;
    [JsonIgnore]
    public string appId;
    public PromptRequest(string appId, PromptInput input) {
        this.appId = appId;
        this.input = input;
    }
}

[System.Serializable]
public class PromptOut
{
    public string text;
    public string session_id;
    public string finish_reason;
}

[System.Serializable]
public class PromptUsage
{
    public List<PromptModel> models;
}

[System.Serializable]
public class PromptModel
{
    public int output_tokens;
    public string model_id;
    public int input_tokens;
}

/*
 * {
 *   "output":{"finish_reason":"stop","session_id":"f392953836e144bab1b21cc8aebb268a","text":"- 我叫小雅，是这款游戏中的NPC。很高兴遇见你，李智恩。有什么可以帮助你的吗？"},
 *   "usage":{"models":[{"output_tokens":25,"model_id":"qwen-max","input_tokens":2554}]},
 *   "request_id":"8d882cca-31fe-9865-9571-8c40edba0910"
 * }
 */
[System.Serializable]
public class PromptResponse
{
    public PromptOut output;
    public PromptUsage usage;
    public string request_id;
}
