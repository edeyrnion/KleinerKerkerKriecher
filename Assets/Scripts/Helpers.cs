using UnityEngine;

public class Helpers
{
    public static Vector2Int PointInCircle(int radius)
    {
        var t = 2 * Mathf.PI * Random.value;
        var r = radius * Mathf.Sqrt(Random.value);

        var x = Mathf.FloorToInt(r * Mathf.Cos(t));
        var y = Mathf.FloorToInt(r * Mathf.Sin(t));

        return new Vector2Int(x, y);
    }

    public static Vector2Int PointInEllipse(int width, int height)
    {
        var t = 2 * Mathf.PI * Random.value;
        var r = Mathf.Sqrt(Random.value);

        var x = Mathf.FloorToInt(width * r * Mathf.Cos(t));
        var y = Mathf.FloorToInt(height * r * Mathf.Sin(t));

        return new Vector2Int(x, y);
    }

    private static float NextRandom(float mean, float sigma)
    {
        float u, v, s;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            s = u * u + v * v;
        }
        while (s >= 1.0f && s <= 3f * sigma);

        float fac = Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);
        return ((u * fac) * sigma) + mean;
    }

    public static int NormalizedRandom(int minValue, int maxValue)
    {
        var mean = (minValue + maxValue) / 2;
        var sigma = (maxValue - mean) / 3;

        return Mathf.FloorToInt(NextRandom(mean, sigma));
    }

    public static int RoundToGrid(float n, int gridSize)
    {
        return Mathf.FloorToInt(((n + gridSize - 1) / gridSize)) * gridSize;
    }

    public static Vector2Int RoundToGrid(Vector2 n, int gridSize)
    {
        return new Vector2Int(RoundToGrid(n.x, gridSize), RoundToGrid(n.y, gridSize));
    }
}
