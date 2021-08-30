using System;
using System.Collections.Generic;

namespace RpcHelper
{
        public delegate bool ArrayCompare<T>(T one, T two);
        public class ArrayHelper
        {
                /// <summary>
                /// 通过排序比对长度一致的数组是否相同
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="one"></param>
                /// <param name="two"></param>
                /// <param name="match"></param>
                /// <returns></returns>
                public static bool CompareSortArray<T>(T[] one, T[] two, ArrayCompare<T> match)
                {
                        if (one.Length != two.Length)
                        {
                                return false;
                        }
                        Array.Sort(one);
                        Array.Sort(two);
                        return _CompareArray<T>(one, two, match);
                }
                public static bool CompareSortArray<T>(T[] one, T[] two, ArrayCompare<T> match, IComparer<T> comparer)
                {
                        if (one.Length != two.Length)
                        {
                                return false;
                        }
                        Array.Sort(one, 0, one.Length, comparer);
                        Array.Sort(two, 0, two.Length, comparer);
                        return _CompareArray<T>(one, two, match);
                }
                /// <summary>
                /// 通过排序比对长度一致的数组是否相同
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="one"></param>
                /// <param name="two"></param>
                /// <returns></returns>
                public static bool CompareSortArray<T>(T[] one, T[] two)
                {
                        if (one.Length != two.Length)
                        {
                                return false;
                        }
                        Array.Sort(one);
                        Array.Sort(two);
                        return _CompareArray<T>(one, two);
                }
                public static bool CompareSortArray<T>(T[] one, T[] two, IComparer<T> comparer)
                {
                        if (one.Length != two.Length)
                        {
                                return false;
                        }
                        Array.Sort(one, 0, one.Length, comparer);
                        Array.Sort(two, 0, two.Length, comparer);
                        return _CompareArray<T>(one, two);
                }
                private static bool _CompareArray<T>(T[] one, T[] two, ArrayCompare<T> match)
                {
                        for (int i = 0; i < one.Length; i++)
                        {
                                if (!match(one[i], two[i]))
                                {
                                        return false;
                                }
                        }
                        return true;
                }
                private static bool _CompareArray<T>(T[] one, T[] two)
                {
                        for (int i = 0; i < one.Length; i++)
                        {
                                if (!one[i].Equals(two[i]))
                                {
                                        return false;
                                }
                        }
                        return true;
                }
                /// <summary>
                /// 比对长度一致的数组是否相同(已排序)
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="one"></param>
                /// <param name="two"></param>
                /// <param name="match"></param>
                /// <returns></returns>
                public static bool CompareArray<T>(T[] one, T[] two, ArrayCompare<T> match)
                {
                        return one.Length == two.Length && _CompareArray(one, two, match);
                }
                public static bool CompareArray<T>(T[] one, T[] two)
                {
                        return one.Length == two.Length && _CompareArray(one, two);
                }
        }
}
