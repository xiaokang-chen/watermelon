[System.Serializable]
public class OpenDataMessage
{
    // type 用于表明时间类型
    public string type;

    public string shareTicket;

    public int score;
    // ScoreRank FeelingsRank
    // [JsonConverter(typeof(StringEnumConverter))]
    public string rankKey;
}

public enum RankKeyType
{
    FeelingsRank,
    ScoreRank,
}
