using System.Collections.Generic;

namespace TweakIt
{
    public static class ListExtensions
    {
        public static int AddSorted<T>(this List<T> @this, T item, IComparer<T> comparer)
        {
            if (@this.Count == 0)
            {
                @this.Add(item);
                return @this.Count;
            }

            if (comparer.Compare(@this[@this.Count - 1], item) <= 0)
            {
                @this.Add(item);
                return @this.Count;
            }

            if (comparer.Compare(@this[0], item) >= 0)
            {
                @this.Insert(0, item);
                return 0;
            }

            var index = @this.BinarySearch(item, comparer);
            if (index < 0) index = ~index;
            @this.Insert(index, item);
            return index;
        }
    }
}