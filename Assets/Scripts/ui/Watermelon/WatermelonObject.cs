using UnityEngine;

public enum WatermelonObjectType
{
    WatermelonBegin,
    WatermelonEnd = 100,
    Blast,
    Combo,
    Max = 7
}

public class WatermelonObject : MonoBehaviour
{
	public WatermelonObjectType objectType;
    [HideInInspector]
    public float lastActiveTimestamp;
}
