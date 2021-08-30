namespace Wedonek.RpcStore.Gateway.Model
{
        /// <summary>
        /// 服务关联
        /// </summary>
        public class ServerRelation
        {
                /// <summary>
                /// 服务Id
                /// </summary>
                public long Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务名
                /// </summary>
                public string ServerName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务Ip
                /// </summary>
                public string ServerIp
                {
                        get;
                        set;
                }

                /// <summary>
                /// 服务端口
                /// </summary>
                public int ServerPort
                {
                        get;
                        set;
                }
                /// <summary>
                /// 链接Ip
                /// </summary>
                public string ConIp
                {
                        get;
                        set;
                }

                /// <summary>
                /// 服务Mac
                /// </summary>
                public string ServerMac
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务编号
                /// </summary>
                public int ServerIndex
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
                /// 系统名
                /// </summary>
                public string SystemName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 组名
                /// </summary>
                public string GroupName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否远程
                /// </summary>
                public bool IsRemote
                {
                        get;
                        set;
                }
        }
}
