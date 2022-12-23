namespace Additions;

using Godot;
using System;

public static class RNG
{
    private static RandomNumberGenerator _numberGenerator;

    public static RandomNumberGenerator NumberGenerator
    {
        get
        {
            if (_numberGenerator == null)
            {
                _numberGenerator = new();
                _numberGenerator.Randomize();
            }

            return _numberGenerator;
        }
    }

    public static ulong Seed
    {
        get => NumberGenerator.Seed;
        set => NumberGenerator.Seed = value;
    }

    public static ulong State
    {
        get => NumberGenerator.State;
        set => NumberGenerator.State = value;
    }

    public static void Randomize() => NumberGenerator.Randomize();

    public static int IntRange(int min, int max) => NumberGenerator.RandiRange(min, max);

    public static int IntRange(Vector2i range) => NumberGenerator.RandiRange(range.x, range.y);

    public static float FloatRange(float min, float max) => NumberGenerator.RandfRange(min, max);

    public static float FloatRange(Vector2 range) => NumberGenerator.RandfRange(range.x, range.y);

    public static uint RandI() => NumberGenerator.Randi();

    public static float Float01() => NumberGenerator.Randf();

    public static float NormallyDistributedFloat(float mean = 0, float deviation = 1) => NumberGenerator.Randfn(mean, deviation);
}
