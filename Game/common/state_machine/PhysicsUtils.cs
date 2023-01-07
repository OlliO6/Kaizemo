namespace Game;

using Godot;

public static class PhysicsUtils
{
    public static float Damped(this float value, float damping, double delta)
    {
        return value * Mathf.Pow(1f - damping, (float)delta * 10f);
    }
}
