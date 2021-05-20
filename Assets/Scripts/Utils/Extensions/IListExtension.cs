using System;
using System.Collections.Generic;
using System.Linq;

public static class IListExtension {

    private static Random rng = new Random();

    public static bool AddDistinct<T>(this IList<T> list, T element) {
        if (list.Contains(element)) { return false; }
        list.Add(element);
        return true;
    }

    public static void AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> listToBeAdded) {
        foreach (T element in listToBeAdded) {
            list.AddDistinct(element);
        }
    }

    public static T GetNext<T>(this IList<T> list, T current, int offset, bool loop = true) {
        int index = list.IndexOf(current);

        index += offset;

        if (!loop) {
            if (index >= list.Count || index < 0) {
                return default(T);
            }
        }

        while (index < 0) {
            index += list.Count;
        }
        index = index % list.Count;

        return list[index];
    }

    public static T GetAtIndex<T>(this IList<T> list, int index, bool loop = true) {
        if (list.Count == 0) { return default(T); }

        if (loop) {
            int loopIndex = index;
            while (loopIndex >= list.Count) {
                loopIndex -= list.Count;
            }
            while (loopIndex < 0) {
                loopIndex += list.Count;
            }
            return list[loopIndex];
        } else {
            index = UnityEngine.Mathf.Clamp(index, 0, list.Count - 1);
        }

        return list[index];
    }

    public static void Shuffle<T>(this IList<T> list) { 
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    public static T Random<T>(this IList<T> list, Random random) {
        return list[random.Next(0, list.Count)];
    }

    public static T Random<T>(this IEnumerable<T> enumerable) {
        int count = enumerable.Count();

        if(count == 0) {
            return default(T);
        }

        int index = rng.Next(0, count);
        return enumerable.ElementAt(index);
    }

    public static T Random<T>(this IEnumerable<T> enumerable, Random random) {
        int count = enumerable.Count();

        if (count == 0) {
            return default(T);
        }

        int index = random.Next(0, count);
        return enumerable.ElementAt(index);
    }

    public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector) {
        float totalWeight = sequence.Sum(weightSelector);
        // The weight we are after...
        float itemWeightIndex = UnityEngine.Random.value * totalWeight;
        float currentWeightIndex = 0;

        foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) }) {
            currentWeightIndex += item.Weight;

            // If we've hit or passed the weight we are after for this item then it's the one we want....
            if (currentWeightIndex >= itemWeightIndex)
                return item.Value;

        }

        return default(T);

    }

    public static T GetLast<T>(this IList<T> list) {
        if (list.Count == 0) { return default(T); }
        return list[list.Count - 1];
    }

    public static void Foreach<T>(this IEnumerable<T> list, Action<T> action) {
        foreach (T item in list) {
            action(item);
        }
    }
}

public static class IArrayExtension {

    private static Random rng = new Random();

    public static void Shuffle<T>(this T[] list) {
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}