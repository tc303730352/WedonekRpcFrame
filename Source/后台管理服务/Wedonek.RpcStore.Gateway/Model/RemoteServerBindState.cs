namespace Wedonek.RpcStore.Gateway.Model
{
        /// <summary>
        /// 服务节点绑定状态
        /// </summary>
        public class RemoteServerBindState : RemoteServer
        {
                /// <summary>
                /// 组别名
                /// </summary>
                public string GroupName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否绑定
                /// </summary>
                public bool IsBind
                {
                        get;
                        set;
                }
                /// <summary>
                /// 绑定Id
                /// </summary>
                public long BindId
                {
                        get;
                        set;
                }
        }
}
