using System.Collections.Generic;

using RpcHelper.Config;
namespace SocketBuffer.Config
{
        public class BufferConfig
        {
                static BufferConfig()
                {
                        Dictionary<int, int> dic = LocalConfig.Local.GetValue("socket_buffer:init", new Dictionary<int, int>() {
                                { 256,50 },
                                { 512,50 },
                                { 1024,30 },
                                { 1536,30 },
                                { 2048,30 },
                                { 2560,30 },
                                { 3840,10 },
                                { 4608,10 },
                                { 5120,10 },
                                { 6400,5 },
                                { 7680,5 },
                                { 10240,5 },
                                { 20480,5 },
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
                public static int ExpansionBufferSize { get; set; } = LocalConfig.Local.GetValue("socket_buffer:Expansion", 100);
                /// <summary>
                /// 初始扩展Buffer比例
                /// </summary>
                public static short ExpansionScale { get; internal set; } = LocalConfig.Local.GetValue<short>("socket_buffer:Scale", 25);

                /// <summary>
                /// 最大限度
                /// </summary>

                public static int MaxBufferSize = LocalConfig.Local.GetValue<int>("socket_buffer:Max", 1024 * 1024);
        }
}
