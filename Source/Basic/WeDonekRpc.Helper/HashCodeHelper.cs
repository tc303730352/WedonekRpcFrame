namespace WeDonekRpc.Helper
{
        public class HashCodeHelper
        {
                private const int _BKDRSeed = 131;
                private const int _RsB = 378551;
                private const int _RsA = 63689;
                private const int _JsHash = 1315423911;
                private const uint _PjwVal = 4294967295;
                private const uint _ElfVal = 4026531840;
                private const int _DjbHash = 5381;
                private static readonly string[] _HashList = new string[] { "def", "BKDR", "AP", "SDBM", "RS", "JS", "PJW", "ELF", "DJB" };

                public static string[] HashList => _HashList;

                public static int Hash(string str, string name)
                {
                        switch (name)
                        {
                                case "BKDR":
                                        return BKDR_Hash(str);
                                case "AP":
                                        return AP_Hash(str);
                                case "SDBM":
                                        return SDBM_Hash(str);
                                case "RS":
                                        return RS_Hash(str);
                                case "JS":
                                        return JS_Hash(str);
                                case "PJW":
                                        return PJW_Hash(str);
                                case "ELF":
                                        return ELF_Hash(str);
                                case "DJB":
                                        return DJB_Hash(str);
                                default:
                                        return str.GetHashCode();
                        }
                }
                /// <summary>
                /// BKDR Hasch方法
                /// </summary>
                /// <param name="str">字符串</param>
                /// <param name="seed">一个素数</param>
                /// <returns></returns>
                public static int BKDR_Hash(string str, int seed = _BKDRSeed)
                {
                        int hash = 0;
                        int count;
                        char[] bitarray = str.ToCharArray();
                        count = bitarray.Length;
                        while (count > 0)
                        {
                                hash = (hash * seed) + bitarray[bitarray.Length - count];
                                count--;
                        }

                        return hash & int.MaxValue;
                }
                /// <summary>
                /// APhash算法
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static int AP_Hash(string str)
                {
                        int hash = 0;
                        int i;
                        int count;
                        char[] bitarray = str.ToCharArray();
                        count = bitarray.Length;
                        for (i = 0; i < count; i++)
                        {
                                if ((i & 1) == 0)
                                {
                                        hash ^= (hash << 7) ^ (bitarray[i]) ^ (hash >> 3);
                                }
                                else
                                {
                                        hash ^= ~((hash << 11) ^ (bitarray[i]) ^ (hash >> 5));
                                }
                                count--;
                        }

                        return hash & int.MaxValue;
                }

                /// <summary>
                /// SDBM hash算法
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static int SDBM_Hash(string str)
                {
                        int hash = 0;
                        int count;
                        char[] bitarray = str.ToCharArray();
                        count = bitarray.Length;

                        while (count > 0)
                        {
                                // equivalent to: hash = 65599*hash + (*str++);
                                hash = bitarray[bitarray.Length - count] + (hash << 6) + (hash << 16) - hash;
                                count--;
                        }

                        return hash & int.MaxValue;
                }

                /// <summary>
                /// RS hash算法
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static int RS_Hash(string str)
                {
                        int a = _RsA;
                        int hash = 0;
                        int count;
                        char[] bitarray = str.ToCharArray();
                        count = bitarray.Length;
                        while (count > 0)
                        {
                                hash = (hash * a) + bitarray[bitarray.Length - count];
                                a *= _RsB;
                                count--;
                        }

                        return hash & int.MaxValue;
                }

                /// <summary>
                /// JS hash
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static int JS_Hash(string str)
                {
                        int hash = _JsHash;
                        int count;
                        char[] bitarray = str.ToCharArray();
                        count = bitarray.Length;
                        while (count > 0)
                        {
                                hash ^= (hash << 5) + bitarray[bitarray.Length - count] + (hash >> 2);
                                count--;
                        }

                        return hash & int.MaxValue;
                }

                /// <summary>
                /// PJW Hash算法
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static int PJW_Hash(string str)
                {
                        int BitsInUnignedInt = sizeof(int) * 8;
                        int ThreeQuarters = BitsInUnignedInt * 3 / 4;
                        int OneEighth = BitsInUnignedInt / 8;
                        int hash = 0;
                        unchecked
                        {
                                int HighBits = (int)_PjwVal << (BitsInUnignedInt - OneEighth);
                                int count;
                                char[] bitarray = str.ToCharArray();
                                count = bitarray.Length;
                                while (count > 0)
                                {
                                        hash = (hash << OneEighth) + bitarray[bitarray.Length - count];
                                        int test;
                                        if ((test = hash & HighBits) != 0)
                                        {
                                                hash = (hash ^ (test >> ThreeQuarters)) & (~HighBits);
                                        }
                                        count--;
                                }
                        }
                        return hash & int.MaxValue;
                }

                /// <summary>
                /// ELF hash算法
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static int ELF_Hash(string str)
                {
                        int hash = 0;
                        int count;
                        char[] bitarray = str.ToCharArray();
                        count = bitarray.Length;
                        unchecked
                        {
                                while (count > 0)
                                {
                                        hash = (hash << 4) + bitarray[bitarray.Length - count];
                                        int x;
                                        if ((x = hash & (int)_ElfVal) != 0)
                                        {
                                                hash ^= x >> 24;
                                                hash &= ~x;
                                        }
                                        count--;
                                }
                        }
                        return (hash & int.MaxValue);
                }


                /// <summary>
                /// DJB 哈希算法
                /// </summary>
                /// <param name="str"></param>
                /// <returns></returns>
                public static int DJB_Hash(string str)
                {
                        int hash = _DjbHash;
                        int count;
                        char[] bitarray = str.ToCharArray();
                        count = bitarray.Length;
                        while (count > 0)
                        {
                                hash += (hash << 5) + bitarray[bitarray.Length - count];
                                count--;
                        }

                        return hash & int.MaxValue;
                }
        }
}
