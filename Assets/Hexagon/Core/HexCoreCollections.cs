using System.Collections.Generic;

public static class HexCollection
{
    public static string Join<T>(this string separator, IEnumerable<T> collection)
    {
        return string.Join(separator, collection);
    }
    public static string Join<T>(this IEnumerable<T> collection, string separator)
    {
        return string.Join(separator, collection);
    }
}