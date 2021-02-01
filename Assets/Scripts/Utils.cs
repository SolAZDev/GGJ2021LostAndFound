using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static string ConvertSecondsToHHMMSS(float secs)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(secs);
        string result = string.Format("{1:D2}:{2:D2}",
            t.Hours,
            t.Minutes,
            t.Seconds,
            t.Milliseconds);
        return result;
    }

    public static Vector2 MouseAimHelper(Vector2 input)
    {
        Vector2 norm = input.normalized;
        // norm.x = norm.x * 180f;
        return norm;
    }
}
