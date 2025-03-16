using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVFTable", menuName = "VideoConfigurationTable/VFTable", order = 1)]
public class VideoConfigurationTable : ScriptableObject
{
    [NonReorderable]
    public List<VideoData> _videoDatas = new List<VideoData>();
}
