namespace RpcModel
{
        public class MsgSource
        {
                /// <summary>
                /// 服务组ID
                /// </summary>
                public long SourceGroupId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务组值
                /// </summary>
                public string GroupTypeVal { get; set; }
                /// <summary>
                /// 服务节点类型值
                /// </summary>
                public string SystemType { get; set; }
                /// <summary>
                /// 服务节点类型ID
                /// </summary>
                public long SystemTypeId { get; set; }
                /// <summary>
                /// 服务节点ID
                /// </summary>
                public long SourceServerId { get; set; }
                /// <summary>
                /// 服务集群ID
                /// </summary>
                public long RpcMerId { get; set; }
                /// <summary>
                /// 所在区域Id
                /// </summary>
                public int RegionId { get; set; }
        }
}
