using UnityEngine;
public struct Bounds
{
    public static float GetRandomFloat(Bounds bounds)
    {
        return Random.Range(bounds.min, bounds.max);
    }

    public float min;
    public float max;

    public Bounds(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}