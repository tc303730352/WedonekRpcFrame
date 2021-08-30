namespace Wedonek.RpcStore.Service.Model
{
        public class RemoteGroupAddParam
        {
                /// <summary>
                /// 集群Id
                /// </summary>
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务节点Id
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 所属区域
                /// </summary>
                public int RegionId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务节点类型
                /// </summary>
                public long SystemType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务节点类型值
                /// </summary>
                public string TypeVal
                {
                        get;
                        set;
                }
                /// <summary>
                /// 权重
                /// </summary>
                public int Weight
                {
                        get;
                        set;
                }
        }
}
