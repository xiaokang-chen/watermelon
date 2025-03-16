using System.Runtime.InteropServices;

public class WeChatDetector 
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern bool IsWeChatPlatform();
#endif

    public static bool IsWeChat()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsWeChatPlatform();
#else
        return false;
#endif
    }
}
