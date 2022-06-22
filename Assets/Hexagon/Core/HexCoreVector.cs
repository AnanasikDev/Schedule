using UnityEngine;
using System;

public static class HexVector3
{
    public static Vector3 Multiply(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static Vector3 Divide(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
    public static Vector3 ManualPower(this Vector3 a, Vector3 b)
    {
        return new Vector3((float)System.Math.Pow(a.x, b.x), (float)System.Math.Pow(a.y, b.y), (float)System.Math.Pow(a.z, b.z));
    }
    public static Vector3 DoManual(this Vector3 a, Vector3 b, Func<float, float, float> function)
    {
        return new Vector3(function(a.x, b.x), function(a.y, b.y), function(a.z, b.z));
    }
}

public static class HexVector3Int
{
    public static Vector3Int Multiply(this Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static Vector3Int Divide(this Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.x / b.x, a.y / b.y, a.z / b.z);
    }
    public static Vector3Int ManualPower(this Vector3Int a, Vector3Int b)
    {
        return new Vector3Int((int)System.Math.Pow(a.x, b.x), (int)System.Math.Pow(a.y, b.y), (int)System.Math.Pow(a.z, b.z));
    }
    public static Vector3Int DoManual(this Vector3Int a, Vector3Int b, Func<int, int, int> function)
    {
        return new Vector3Int(function(a.x, b.x), function(a.y, b.y), function(a.z, b.z));
    }
}

public static class HexVector2
{
    public static Vector2 Multiply(this Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }
    public static Vector2 Divide(this Vector2 a, Vector2 b)
    {
        return new Vector2(a.x / b.x, a.y / b.y);
    }
    public static Vector2 ManualPower(this Vector2 a, Vector2 b)
    {
        return new Vector2((float)System.Math.Pow(a.x, b.x), (float)System.Math.Pow(a.y, b.y));
    }
    public static Vector2 DoManual(this Vector2 a, Vector2 b, Func<float, float, float> function)
    {
        return new Vector2(function(a.x, b.x), function(a.y, b.y));
    }
}

public static class HexVector2Int
{
    public static Vector2Int Multiply(this Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x * b.x, a.y * b.y);
    }
    public static Vector2Int Divide(this Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x / b.x, a.y / b.y);
    }
    public static Vector2Int ManualPower(this Vector2Int a, Vector2Int b)
    {
        return new Vector2Int((int)System.Math.Pow(a.x, b.x), (int)System.Math.Pow(a.y, b.y));
    }
    public static Vector2Int DoManual(this Vector2Int a, Vector2Int b, Func<int, int, int> function)
    {
        return new Vector2Int(function(a.x, b.x), function(a.y, b.y));
    }
}