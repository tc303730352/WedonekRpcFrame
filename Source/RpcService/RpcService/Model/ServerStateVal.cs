namespace RpcService.Model
{
        public class ServerStateVal
        {
                /// <summary>
                /// TCP链接存活量
                /// </summary>
                public int ConNum
                {
                        get;
                        set;
                }

                /// <summary>
                /// 平均延迟
                /// </summary>
                public int AvgTime
                {
                        get;
                        set;
                }
                /// <summary>
                /// 最终分数
                /// </summary>
                public int Fraction
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务器ID
                /// </summary>
                public long ServerId { get; set; }
                /// <summary>
                /// 服务器MAC
                /// </summary>
                public string Mac { get; set; }
        }
}
