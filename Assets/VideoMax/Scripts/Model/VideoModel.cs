using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VideoCharacteristic
{
    Default,
    VideoScene1,
    VideoScene2,
    VideoScene3,
    VideoScene4,
    VideoScene5_1,
    VideoScene5_2,
    VideoScene6,
    VideoScene7,
    VideoScene8,
    VideoScene9,
    VideoScene10,
    VideoScene11,
    VideoScene12,
    VideoScene13,
    VideoScene14,
    VideoScene15,
    VideoScene16,
    VideoScene17,
    VideoScene18,
    VideoScene19,
    VideoScene20,
    VideoScene21,
    VideoScene22,
    VideoScene23,
    VideoScene24,
    VideoScene25,
    VideoScene26
}

public enum VideoJumpAction
{
    /// <summary>
    /// 默认
    /// </summary>
    Default,

    /// <summary>
    /// 上一个视频
    /// </summary>
    LastVideo,

    /// <summary>
    /// 下一个视频
    /// </summary>
    NextVideo,

    /// <summary>
    /// 重复视频
    /// </summary>
    RepetitionVideo,

    /// <summary>
    /// 指定视频
    /// </summary>
    DesignatedVideo,

    /// <summary>
    /// 结束播放
    /// </summary>
    OverPlay,

    /// <summary>
    /// 等待
    /// </summary>
    Wait
}

public static class VideoModel
{
    private static Dictionary<VideoCharacteristic, VideoData> VideoDataDic = new();

    public static VideoData GetVideoData(this VideoCharacteristic videoCharacteristic)
    {
        if (VideoDataDic.Count > 0)
            return VideoDataDic.ContainsKey(videoCharacteristic) ? VideoDataDic[videoCharacteristic] : null;
        var datas = Resources.Load<VideoConfigurationTable>("ScriptableObject/VFTable")._videoDatas;
        foreach (var t in datas)
        {
            VideoDataDic[t.characteristic] = t;
        }
        return VideoDataDic.ContainsKey(videoCharacteristic) ? VideoDataDic[videoCharacteristic] : null;
    }
}

[Serializable]
public class VideoData
{
    /// <summary>
    /// 视频名
    /// </summary>
    public string name;

    /// <summary>
    /// 唯一标识
    /// </summary>
    [Header("唯一标识")] public VideoCharacteristic characteristic = VideoCharacteristic.Default;

    /// <summary>
    /// 上一个视频
    /// </summary>
    [Header("上一个视频")] public VideoCharacteristic lastVideo = VideoCharacteristic.Default;

    /// <summary>
    /// 视频链接
    /// </summary>
    [TextArea] [Header("视频链接")] public string VideoUrl = "";

    /// <summary>
    /// 下一个视频
    /// </summary>
    [Header("下一个视频")] public VideoCharacteristic nextVideo = VideoCharacteristic.Default;

    /// <summary>
    /// 指定视频
    /// </summary>
    [Header("指定视频")] public VideoCharacteristic designatedVideo = VideoCharacteristic.Default;

    /// <summary>
    /// 跳转动作
    /// </summary>
    [Header("跳转动作")] public VideoJumpAction action = VideoJumpAction.NextVideo;

    /// <summary>
    /// 跳转场景
    /// </summary>
    [TextArea] [Header("跳转场景")] public string gameSceneName = "";
}