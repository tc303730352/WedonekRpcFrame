using System.Collections.Generic;

namespace RpcService.Model
{
        public class CurrentSysState
        {
                /// <summary>
                /// 链接数
                /// </summary>
                public int ConNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 平均响应时间
                /// </summary>
                public Dictionary<long, int> ArgTime { get; set; }
        }
}
