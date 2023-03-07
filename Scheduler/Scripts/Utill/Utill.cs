using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utill
{
    public static string GetMiddleString(string str, string begin, string end)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        string result = null;

        if (str.IndexOf(begin) > -1)
        {
            str = str.Substring(str.IndexOf(begin) + begin.Length);
            if (str.IndexOf(end) > -1) result = str.Substring(0, str.IndexOf(end));
            else result = str;
        }

        return result;
    }

    public static Color ToColor32(float r, float g, float b, float a)
    {
        return new Color(r / 255, g / 255, b / 255, a);
    }

    public static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 9, 0, 0, 0);

        return origin.AddSeconds(timestamp);
    }

    public static string ConvertFromUnixTimestampToString(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 9, 0, 0, 0);
        origin = origin.AddSeconds(timestamp);

        return origin.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static string ConvertToDateTimeToString(DateTime date)
    {
        return date.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static int ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 9, 0, 0, 0);
        TimeSpan diff = date - origin;

        return (int)Math.Floor(diff.TotalSeconds);
    }
}
