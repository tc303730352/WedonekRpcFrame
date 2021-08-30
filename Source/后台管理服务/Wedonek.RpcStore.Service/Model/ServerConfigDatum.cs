using System;

using RpcModel;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 服务节点配置
        /// </summary>
        public class ServerConfigDatum
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
                /// 远程Ip
                /// </summary>
                public string RemoteIp
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
                /// 服务组别Id
                /// </summary>
                public long GroupId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务类别Id
                /// </summary>
                public long SystemType
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
                /// 权值
                /// </summary>
                public int Weight
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
                /// 所在中控服务的编号
                /// </summary>
                public int BindIndex
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
                /// 区域Id
                /// </summary>
                public int RegionId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 链接公钥
                /// </summary>
                public string PublicKey
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务状态
                /// </summary>
                public RpcServiceState ServiceState
                {
                        get;
                        set;
                }
                /// <summary>
                /// 客户端版本号
                /// </summary>
                public string ApiVer
                {
                        get;
                        set;
                }
                /// <summary>
                /// 最后离线日期
                /// </summary>
                public DateTime LastOffliceDate
                {
                        get;
                        set;
                }
                /// <summary>
                /// 添加时间
                /// </summary>
                public DateTime AddTime
                {
                        get;
                        set;
                }
        }
}
