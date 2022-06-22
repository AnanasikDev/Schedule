using UnityEngine;
public static class Hexath
{
    public static float SnapNumberToStep(this float number, float step)
    {
        float sediment = number % step;

        if (Mathf.Abs(sediment) < step / 2f) return number - sediment;
        else return number - sediment + step * Mathf.Sign(number);
    }
    public static Vector2 GetCirclePositionDegrees(float radius, float angleDeg)
    {
        angleDeg *= Mathf.Deg2Rad;

        float x = Mathf.Sin(angleDeg) * radius;
        float z = Mathf.Cos(angleDeg) * radius;

        return new Vector2(x, z);
    }
    public static Vector2 GetCirclePositionRadians(float radius, float angleRad)
    {
        float x = Mathf.Sin(angleRad) * radius;
        float z = Mathf.Cos(angleRad) * radius;

        return new Vector2(x, z);
    }
    public static Vector2 GetRandomRingPoint(float radius)
    {
        return GetCirclePositionDegrees(radius, UnityEngine.Random.Range(0f, 360f));
    }
    public static Vector2 GetRandomCirclePoint(float radius)
    {
        return GetCirclePositionDegrees(Random.Range(0, radius), UnityEngine.Random.Range(0f, 360f));
    }
}
