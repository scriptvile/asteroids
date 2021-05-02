using UnityEngine;

public struct Bounds
{
    // Privates
    public float min;
    public float max;


    // Constructor
    public Bounds(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    // Methods
    public static float GetRandomPoint(Bounds bounds)
    {
        return Random.Range(bounds.min, bounds.max);
    }
}