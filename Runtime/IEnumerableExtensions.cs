using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableExtensions
{
    public static T RandomElement<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.ElementAt(Random.Range(0, enumerable.Count()));
    }
    
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.OrderBy(x => Random.value);
    }
}