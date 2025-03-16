using System;

public static class DateTimeUtils
{
    public static long GetTimestampToSeconds() {
        var now = DateTime.UtcNow - new DateTime(1970, 1,1,0,0,0,0);
        return Convert.ToInt64(now.TotalSeconds);
    }

    public static long GetTimestampToMilliseconds()
    {
        var now = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(now.TotalMilliseconds);
    }

    public static string UtcTimestamp()
    {
        DateTime utcNow = DateTime.UtcNow;
        int year = utcNow.Year;
        string month = StringUtils.PadLeft(utcNow.Month, 2);
        string day = StringUtils.PadLeft(utcNow.Day, 2);
        string hour = StringUtils.PadLeft(utcNow.Hour, 2);
        string minute = StringUtils.PadLeft(utcNow.Minute, 2);
        string second = StringUtils.PadLeft(utcNow.Second, 2);

        return $"{year}-{month}-{day}T{hour}:{minute}:{second}Z";
    }


    public static bool IsNewDay(string sLastLoginDate)
    {
        DateTime currentDate = DateTime.Now.Date;
        if (string.IsNullOrEmpty(sLastLoginDate))
        {
            return true;
        }

        DateTime lastLoginDate;
        if (DateTime.TryParse(sLastLoginDate, out lastLoginDate))
        {
            return currentDate > lastLoginDate;
        }

        return true;
    }
}