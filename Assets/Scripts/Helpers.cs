using UnityEngine;

public class Helpers
{
    public static Vector2Int PointInEllipse(int width, int height)
    {
        var t = 2 * Mathf.PI * Random.value;
        var r = Mathf.Sqrt(Random.value);

        var x = Mathf.FloorToInt(width * r * Mathf.Cos(t));
        var y = Mathf.FloorToInt(height * r * Mathf.Sin(t));

        return new Vector2Int(x, y);
    }

    private static float NextRandom()
    {
        float u, v, s;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            s = u * u + v * v;
        }
        while (s >= 1.0f);

        float fac = Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);
        return u * fac;
    }

    public static int NormalizedRandom(int minValue, int maxValue)
    {
        var mean = (minValue + maxValue) / 2;
        var sigma = (maxValue - mean) / 3;

        float result;

        do
        {
            result = NextRandom();
            result = result * sigma + mean;
        } while (result < 3 * sigma);

        return Mathf.FloorToInt(result);
    }
}
