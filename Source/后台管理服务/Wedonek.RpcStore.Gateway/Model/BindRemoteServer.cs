namespace Wedonek.RpcStore.Gateway.Model
{
        public class BindRemoteServer
        {
                /// <summary>
                /// 数据Id
                /// </summary>
                public long Id
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
                /// 服务节点名称
                /// </summary>
                public string ServerName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否在线
                /// </summary>
                public bool IsOnline
                {
                        get;
                        set;
                }
                /// <summary>
                /// 类型值
                /// </summary>
                public string SystemName
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
