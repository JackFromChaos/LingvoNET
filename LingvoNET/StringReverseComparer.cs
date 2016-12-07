using System;
using System.Collections.Generic;

namespace LingvoNET
{
    public class StringReverseComparer<T> : IComparer<T>
    {
        public int Compare(T obj1, T obj2)
        {
            return CompareStrings(obj1.ToString(), obj2.ToString());
        }

        public static int CompareStrings(string word1, string word2)
        {
            var res = 1;
            var l1 = word1.Length;
            var l2 = word2.Length;
            var l = Math.Min(l1, l2);
            for (int i = 1; i <= l; i++)
            {
                var r = word1[l1 - i].CompareTo(word2[l2 - i]);
                if (r != 0)
                    return r > 0 ? res : - res;
                res++;
            }

            return Math.Sign(l1.CompareTo(l2)) * res;
        }
    }
}