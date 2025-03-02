using System.Collections.Generic;

using WeDonekRpc.Helper.Config;
namespace WeDonekRpc.IOBuffer.Config
{
    public class BufferConfig
    {
        static BufferConfig ()
        {
            Dictionary<int, int> dic = LocalConfig.Local.GetValue("iobuffer:Init", new Dictionary<int, int>() {
                                { 256,200 },
                                { 512,200 },
                                { 1024,200 },
                                { 1536,120 },
                                { 2048,100 },
                                { 8192,50 },
                                { 3840,20 },
                                { 4608,20 },
                                { 5120,20 },
                                { 6400,20 },
                                { 7680,20 },
                                { 10240,20 },
                                { 20480,20 },
                        });

            BufferSpread = new BufferSpreadSet[dic.Count];
            int x = 0;
            foreach (KeyValuePair<int, int> i in dic)
            {
                BufferSpread[x++] = new BufferSpreadSet(i.Key, i.Value);
            }
        }
        /// <summary>
        /// 初始Buffer大小
        /// </summary>
        public static BufferSpreadSet[] BufferSpread { get; }
        /// <summary>
        /// 扩展Buffer大小
        /// </summary>
        public static int ExpansionBufferSize { get; set; } = LocalConfig.Local.GetValue("iobuffer:Expansion", 100);
        /// <summary>
        /// 初始扩展Buffer比例
        /// </summary>
        public static short ExpansionScale { get; internal set; } = LocalConfig.Local.GetValue<short>("iobuffer:Scale", 45);

        /// <summary>
        /// 最大限度
        /// </summary>

        public static int MaxBufferSize = LocalConfig.Local.GetValue<int>("iobuffer:Max", 1024 * 1024);
    }
}
