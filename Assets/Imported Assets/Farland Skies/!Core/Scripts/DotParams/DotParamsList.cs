using System;
using System.Collections.Generic;
#pragma warning disable 693

namespace Borodar.FarlandSkies.Core.DotParams
{
    public class DotParamsList<T> : SortedList<float, T>
    {
        //---------------------------------------------------------------------
        // Ctors
        //---------------------------------------------------------------------

        public DotParamsList(int capacity) : base(capacity) { }

        //---------------------------------------------------------------------
        // Public
        //---------------------------------------------------------------------

        public int FindIndexPerTime(float time)
        {
            return BinarySearch(Keys, time);
        }

        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------

        private static int BinarySearch<T>(IList<T> list, T value)
        {
            if (list == null) throw new ArgumentNullException("list");

            var comp = Comparer<T>.Default;
            int lo = 0, hi = list.Count - 1;
            while (lo < hi)
            {
                var m = (hi + lo) / 2;  // this might overflow; be careful.
                if (comp.Compare(list[m], value) < 0) lo = m + 1;
                else hi = m - 1;
            }
            if (comp.Compare(list[lo], value) < 0) lo++;
            return lo;
        }
    }
}