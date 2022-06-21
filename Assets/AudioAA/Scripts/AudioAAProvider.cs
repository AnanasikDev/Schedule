using UnityEngine;

public static class AudioAAProvider
{
    public static void RPitch(ref AudioSource source, float min, float max)
    {
        source.pitch = Random.Range(min, max);
    }
    public static void RSpread(ref AudioSource source, float min, float max)
    {
        source.spread = Random.Range(min, max);
    }
}
