using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Re-scales a value between a new float range.
    /// </summary>
    public static float Remap(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue, bool clamp = true)
    {
        if (clamp)
            OldValue = Mathf.Clamp(OldValue, OldMin, OldMax);
        return (((OldValue - OldMin) * (NewMax - NewMin)) / (OldMax - OldMin)) + NewMin;
    }
    
    public static string Color(this string str, Color c)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(c);
        return Color(str, hexColor);

    }

    public static string Color(this string str, string hexColor)
    {
        return $"<color=#{hexColor}>{str}</color>";
    }
}
