namespace Additions;

using Godot;
using System;

public static class NumericExtensions
{
    /// <summary>
    /// Returns true if the value is an specific range (both min and max are inclusive use IsInRangeExlusive if you want them to be exclusive)
    /// </summary>
    public static bool IsInRange(this float value, float min, float max) => value >= min && value <= max;
    /// <summary>
    /// Returns true if the value is an specific range (both min and max are inclusive use IsInRangeExlusive if you want them to be exclusive)
    /// </summary>
    public static bool IsInRange(this int value, int min, int max) => value >= min && value <= max;
    /// <summary>
    /// Returns true if the value is an specific range (both min and max are exclusive use IsInRange if you want them to be inclusive)
    /// </summary>
    public static bool IsInRangeExlusive(this float value, float min, float max) => value > min && value < max;
    /// <summary>
    /// Returns true if the value is an specific range (both min and max are exclusive use IsInRange if you want them to be inclusive)
    /// </summary>
    public static bool IsInRangeExlusive(this int value, int min, int max) => value > min && value < max;

    public static float Lerp(this float from, float to, float weight) => Mathf.Lerp(from, to, weight);
    public static float Clamp(this float from, float min, float max) => Mathf.Clamp(from, min, max);
    public static int Clamp(this int from, int min, int max) => Mathf.Clamp(from, min, max);
    public static float Clamp01(this float from) => Mathf.Clamp(from, 0f, 1f);
    public static int Clamp01(this int from) => Mathf.Clamp(from, 0, 1);
    public static float Abs(this float from) => Mathf.Abs(from);
    public static float Abs(this int from) => Mathf.Abs(from);
    public static int Round(this float from) => Mathf.RoundToInt(from);
    public static int Floor(this float from) => Mathf.FloorToInt(from);
    public static int Ceil(this float from) => Mathf.CeilToInt(from);

    public static string InvariantToString(this float value) => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
}
