using System.Collections.Generic;

namespace XCore.Extensions
{
    public static class ListExtensions
    {
        public static T Random<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}