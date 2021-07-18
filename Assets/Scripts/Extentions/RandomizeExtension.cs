using System.Collections.Generic;
using UnityEngine;

public static class RandomizeExtension
{
    public static float NextFloat(this System.Random rnd, float a, float b, float step = 10000)
    {
        var value = rnd.Next(Mathf.RoundToInt(a * step), Mathf.RoundToInt(b * step));
        return value / step;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<T> CreateShuffledList<T>(this IEnumerable<T> list)
    {
        var newList = new List<T>(list);
        newList.Shuffle();
        return newList;
    }
}