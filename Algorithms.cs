using System;
using System.Collections.Generic;

namespace OpamLab07
{
    public static class Algorithms
    {
        public static List<T> Filter<T>(List<T> src, Func<T, bool> keep)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (keep == null) throw new ArgumentNullException(nameof(keep));
            var result = new List<T>();
            foreach (var item in src)
                if (keep(item)) result.Add(item);
            return result;
        }

        public static List<TOut> Map<TIn, TOut>(List<TIn> src, Func<TIn, TOut> convert)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (convert == null) throw new ArgumentNullException(nameof(convert));
            var result = new List<TOut>(src.Count);
            foreach (var item in src)
                result.Add(convert(item));
            return result;
        }

        public static TAcc Aggregate<T, TAcc>(List<T> src, TAcc start, Func<TAcc, T, TAcc> step)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (step == null) throw new ArgumentNullException(nameof(step));
            TAcc acc = start;
            foreach (var item in src)
                acc = step(acc, item);
            return acc;
        }

        public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(List<T> src, Func<T, TKey> keyOf)
            where TKey : notnull
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (keyOf == null) throw new ArgumentNullException(nameof(keyOf));
            var groups = new Dictionary<TKey, List<T>>();
            foreach (var item in src)
            {
                TKey key = keyOf(item);
                if (!groups.TryGetValue(key, out var list))
                {
                    list = new List<T>();
                    groups[key] = list;
                }
                list.Add(item);
            }
            return groups;
        }
    }
}