using System.Collections.Generic;

public enum TextEmoticonsType
{ 
    Idle,
    Happy,
	Calm,
	Angry,
	Sad,
    Confused,
    Surprise,
    ActingCute, // 卖萌
}

public class TextEmoticons
{
    private static readonly Dictionary<TextEmoticonsType, string> textEmoticonsDict = new() {
        { TextEmoticonsType.Happy, "^_^"},
        { TextEmoticonsType.Calm, "-"},
        { TextEmoticonsType.Angry, "ಠ_ಠ"},
        { TextEmoticonsType.Sad, ";_;"},
        { TextEmoticonsType.Confused, "%"},
        { TextEmoticonsType.Surprise, "!"},
    };

    public static TextEmoticonsType GetEmoticons(string msg) {
        foreach (var kvp in textEmoticonsDict) {
            if (msg.Contains(kvp.Value)) {
                return kvp.Key;
            }
        }
        return TextEmoticonsType.Calm;
	}

    public static string TrimTextEmoticons(string msg)
    {
        foreach (var kvp in textEmoticonsDict)
        {
            if (TextEmoticonsType.Happy == kvp.Key ||
                TextEmoticonsType.Angry == kvp.Key ||
                TextEmoticonsType.Sad == kvp.Key)
            {
                if (msg.Contains(kvp.Value))
                {
                    return msg.Replace(kvp.Value, "");
                }
            }
            else if (TextEmoticonsType.Confused == kvp.Key ||
                TextEmoticonsType.Calm == kvp.Key)
            {
                if (msg.StartsWith(kvp.Value))
                {
                    return msg[kvp.Value.Length..];
                }
                if (msg.EndsWith(kvp.Value))
                {
                    return msg[..^kvp.Value.Length];
                }
            }
            else if (TextEmoticonsType.Surprise == kvp.Key) {
                if (msg.StartsWith(kvp.Value))
                {
                    return msg[kvp.Value.Length..];
                }
            }
            
        }
        return msg;
    }
}

