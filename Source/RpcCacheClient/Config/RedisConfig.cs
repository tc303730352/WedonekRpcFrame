namespace RpcCacheClient.Config
{
        /// <summary>
        /// Redis配置
        /// </summary>
        public class RedisConfig
        {
                /// <summary>
                /// 默认DB
                /// </summary>
                public int DefaultDatabase
                {
                        get;
                        set;
                } = 0;
                /// <summary>
                /// 是否应允许管理操作
                /// </summary>
                public bool AllowAdmin
                {
                        get;
                        set;
                } = true;
                /// <summary>
                /// Ping间隔
                /// </summary>
                public int KeepAlive
                {
                        get;
                        set;
                } = 60;
                /// <summary>
                /// 链接重试次数
                /// </summary>
                public int ConnectRetry
                {
                        get;
                        set;
                } = 3;
                /// <summary>
                /// 同步超时
                /// </summary>
                public int SyncTimeout
                {
                        get;
                        set;
                } = 5000;
                /// <summary>
                /// 异步超时
                /// </summary>
                public int AsyncTimeout
                {
                        get;
                        set;
                } = 5000;
                /// <summary>
                /// 链接超时
                /// </summary>
                public int ConnectTimeout
                {
                        get;
                        set;
                } = 5000;
                /// <summary>
                /// 响应超时
                /// </summary>
                public int ResponseTimeout
                {
                        get;
                        set;
                } = 5000;
                /// <summary>
                /// 链接地址
                /// </summary>
                public string[] ServerIp
                {
                        get;
                        set;
                }
                /// <summary>
                /// 登陆名
                /// </summary>
                public string UserName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 密码
                /// </summary>
                public string Pwd
                {
                        get;
                        set;
                }
                /// <summary>
                /// SocketManager读写器线程使用ThreadPriority.AboveNormal
                /// </summary>
                public bool HighPrioritySocketThreads { get; set; } = true;
                /// <summary>
                /// 是否检查证书吊销列表
                /// </summary>
                public bool CheckCertificateRevocation { get; set; } = false;
                /// <summary>
                /// 用于通过sentinel解析服务的服务名称。
                /// </summary>
                public string ServiceName { get; set; }
        }
}
