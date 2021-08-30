namespace RpcService.Model
{
        [System.Serializable]
        public class ReduceInRankConfig
        {
                /// <summary>
                /// 是否启用
                /// </summary>
                public bool IsEnable { get; set; }
                /// <summary>
                /// 触发限制错误数
                /// </summary>
                public int LimitNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 触发熔断的错误数
                /// </summary>
                public int FusingErrorNum { get; set; }

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
