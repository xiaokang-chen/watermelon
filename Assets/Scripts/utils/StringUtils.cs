using System;

public static class StringUtils
{
    public static string PadLeft(int value, int totalWidth = 2, char paddingChar = '0')
    {
        if (value >= Math.Pow(10, totalWidth))
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is too large for padding width {totalWidth}.");
        }

        return value.ToString().PadLeft(totalWidth, paddingChar);
    }
}