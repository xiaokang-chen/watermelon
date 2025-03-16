using System;
using UnityEngine;
using WeChatWASM;

public class DataStorageMgr
{
    private static DataStorageMgr instance;

    public static DataStorageMgr Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new DataStorageMgr();
            }
            return instance;
        }
    }

    private string lastLoginDate;
    public string LastLoginDate
    {
        get { return lastLoginDate; }
        set
        {
            if (value != lastLoginDate)
            {
                lastLoginDate = value;
                PlayerPrefs.SetString("LastLoginDate", lastLoginDate);
            }
        }
    }
    // 好感度
    private int feelings;
    public int Feelings
    {
        get { return feelings; }
        set
        {
            if (value != feelings)
            {
                feelings = value;
                UploadRankScore(RankKeyType.FeelingsRank, feelings);
                PlayerPrefs.SetInt("Feelings", feelings);
            }
        }
    }

    // watermelon score
    private int watermelonScore;
    public int WatermelonScore
    {
        get { return watermelonScore; }
        set
        {
            if (value != watermelonScore)
            {
                if (value > watermelonScore)
                {
                    UploadRankScore(RankKeyType.ScoreRank, value);
                }
                watermelonScore = value;
                PlayerPrefs.SetInt("watermelonScore", watermelonScore);
            }
        }
    }

    private int gameInstaneBTimes;

    public int GameInstaneBTimes
    {
        get { return gameInstaneBTimes; }
        set
        {
            if (value != gameInstaneBTimes)
            {
                gameInstaneBTimes = value;
                PlayerPrefs.SetInt("GameInstaneBTimes", gameInstaneBTimes);
            }
        }
    }

    private DataStorageMgr()
    {
        lastLoginDate = PlayerPrefs.GetString("LastLoginDate");
        if (DateTimeUtils.IsNewDay(lastLoginDate))
        {
            LastLoginDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            GameInstaneBTimes = 0;
        }

        feelings = PlayerPrefs.GetInt("Feelings");
        watermelonScore = PlayerPrefs.GetInt("watermelonScore");
    }


    private void UploadRankScore(RankKeyType rankKeyType, int score)
    {

        OpenDataMessage msgData = new()
        {
            type = "setUserRecord",
            rankKey = RankKeyType.ScoreRank == rankKeyType ? "ScoreRank" : "FeelingsRank",
            score = score
        };

        string msg = JsonUtility.ToJson(msgData);

        Debug.Log(msg);
#if UNITY_WEBGL
        if (Application.platform == RuntimePlatform.WebGLPlayer && WeChatDetector.IsWeChat())
            WX.GetOpenDataContext().PostMessage(msg);
#endif
    }

}
