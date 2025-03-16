using UnityEngine;

public static class UUIDGenerator
{
    private static readonly string HexDigits = "0123456789abcdef";

    public static string Generate(bool useHyphens = true)
    {
        var chars = new char[36];
        for (int i = 0; i < 36; i++)
        {
            chars[i] = HexDigits[Random.Range(0, 16)];
        }

        chars[14] = '4';
        chars[19] = (char)(HexDigits[(chars[19] & 0x3) | 0x8]);
        chars[8] = chars[13] = chars[18] = chars[23] = '-';

        var uuid = new string(chars);
        return useHyphens ? uuid : uuid.Replace("-", "");
    }
}
