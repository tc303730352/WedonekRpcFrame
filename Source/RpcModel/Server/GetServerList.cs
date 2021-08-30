namespace RpcModel
{
        /// <summary>
        /// 获取节点服务列表
        /// </summary>
        public class GetServerList
        {
                /// <summary>
                /// 节点类型
                /// </summary>
                public string SystemType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 节点所在区域
                /// </summary>
                public int RegionId { get; set; }
                /// <summary>
                /// 集群Id
                /// </summary>
                public long RpcMerId { get; set; }
                /// <summary>
                /// 限定使用区域
                /// </summary>
                public int LimitRegionId { get; set; }
        }
}
