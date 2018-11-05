using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    public static Vector2 PointInCircle(float radius)
    {
        var t = 2 * Mathf.PI * Random.value;
        var r = radius * Mathf.Sqrt(Random.value);

        return new Vector2(r * Mathf.Cos(t), r * Mathf.Sin(t));
    }

    public static double NextRandom(double mean, double sigma)
    {
        double u, v, S;

        do
        {
            u = 2.0 * Random.value - 1.0;
            v = 2.0 * Random.value - 1.0;
            S = u * u + v * v;
        }
        while (S >= 1.0 && S <= 3* sigma);

        double fac = System.Math.Sqrt(-2.0 * System.Math.Log(S) / S);
        return ((u * fac) * sigma) + mean;
    }

    public static double NormalizedRandom(double minValue, double maxValue)
    {
        var mean = (minValue + maxValue) / 2;
        var sigma = (maxValue - mean) / 3;

        return NextRandom(mean, sigma);
    }

    public static int RoundToGrid(float n, int gridSize)
    {
        return Mathf.FloorToInt(((n + gridSize - 1) / gridSize)) * gridSize;
    }

    public static Vector2 RoundToGrid(Vector2 n, int gridSize)
    {
        Vector2 v = new Vector2(RoundToGrid(n.x, gridSize), RoundToGrid(n.y, gridSize));
        return v;
    }

    public static Vector3 RoundToGrid(Vector3 n, int gridSize)
    {
        Vector3 v = new Vector3(RoundToGrid(n.x, gridSize), RoundToGrid(n.y, gridSize), RoundToGrid(n.z, gridSize));
        return v;
    }
}
