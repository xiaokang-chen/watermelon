using System;
using System.Security.Cryptography;
using System.Text;

public static class HmacSha1Utils
{
    public static string Hash(string key, string message)
    {
        if (message == null || key == null)
        {
            throw new ArgumentNullException(message == null ? "message" : "key");
        }

        using (var hmacSha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
        {
            byte[] hashBytes = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
