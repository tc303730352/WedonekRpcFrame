using RpcModel;

namespace RpcService.Model.DAL_Model
{
        [System.Serializable]
        internal class RemoteServerModel
        {
                /// <summary>
                /// 服务名
                /// </summary>
                public string ServerName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务端Ip
                /// </summary>
                public string ServerIp
                {
                        get;
                        set;
                }
                /// <summary>
                /// 远程IP
                /// </summary>
                public string RemoteIp
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务端端口
                /// </summary>
                public int ServerPort
                {
                        get;
                        set;
                }


                /// <summary>
                /// 链接的公有Key
                /// </summary>
                public string PublicKey
                {
                        get;
                        set;
                }
                /// <summary>
                /// 所属组ID
                /// </summary>
                public long GroupId { get; set; }

                /// <summary>
                /// 所属系统类型
                /// </summary>
                public long SystemType { get; set; }
                /// <summary>
                /// API版本号
                /// </summary>
                public string ApiVer { get; set; }
                /// <summary>
                /// 服务状态
                /// </summary>
                public RpcServiceState ServiceState { get; set; }
                /// <summary>
                /// 绑定服务中心编号
                /// </summary>
                public int BindIndex { get; set; }
                /// <summary>
                /// 配置权值(大于10则获取远程配置优先)
                /// </summary>
                public short ConfigPrower { get; set; }

                /// <summary>
                /// 当前连接ip
                /// </summary>
                public string ConIp { get; set; }
                /// <summary>
                /// 所属区域
                /// </summary>
                public int RegionId { get; set; }
        }
}
