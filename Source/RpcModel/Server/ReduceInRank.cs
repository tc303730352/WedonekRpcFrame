namespace RpcModel
{
        /// <summary>
        /// 降级配置
        /// </summary>
        [System.Serializable]
        public class ReduceInRank
        {
                /// <summary>
                /// 是否启动降级
                /// </summary>
                public bool IsEnable
                {
                        get;
                        set;
                }
                /// <summary>
                /// 触发限制错误数
                /// </summary>
                public int LimitNum
                {
                        get;
                        set;
                }

                /// <summary>
                /// 链接失败触发熔断次数
                /// </summary>
                public int FusingErrorNum
                {
                        get;
                        set;
                }

                /// <summary>
                /// 刷新统计数的时间(秒)
                /// </summary>
                public int RefreshTime { get; set; }
                /// <summary>
                /// 最短融断时长
                /// </summary>
                public int BeginDuration
                {
                        get;
                        set;
                }
                /// <summary>
                /// 最长熔断时长
                /// </summary>
                public int EndDuration
                {
                        get;
                        set;
                }
        }
}
