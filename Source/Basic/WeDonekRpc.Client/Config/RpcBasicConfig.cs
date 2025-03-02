using System;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Config
{
    internal class RpcBasicConfig
    {
        public RpcBasicConfig ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("rpc");
            this.RpcServer = section.GetValue("RpcServer", Array.Empty<string>());
            this.AppId = section.GetValue("RpcAppId");
            this.AppSecret = section.GetValue("RpcAppSecret");
            this.RpcSystemType = section.GetValue("RpcSystemType");
            this.RpcServerIndex = section.GetValue("RpcIndex", 0);
            this.IsEnableError = section.GetValue("IsEnableError", true);
            this.ContainerType = section.GetValue("ContainerType", ContainerType.无);
            this.InsidePort = section.GetValue<int?>("InsidePort");
            this.HostPort = section.GetValue<int?>("HostPort");
        }
        /// <summary>
        /// 中控服务链接地址
        /// </summary>
        public string[] RpcServer { get; }

        /// <summary>
        /// 集群AppId
        /// </summary>
        public string AppId { get; }
        /// <summary>
        /// 集群链接密钥
        /// </summary>
        public string AppSecret { get; }
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public string RpcSystemType { get; }

        /// <summary>
        /// 服务节点索引
        /// </summary>
        public int RpcServerIndex { get; }

        /// <summary>
        /// 是否启用错误管理
        /// </summary>
        public bool IsEnableError { get; }

        /// <summary>
        /// 容器内部端口号
        /// </summary>
        public int? InsidePort
        {
            get;
        }
        /// <summary>
        /// 宿主端口
        /// </summary>
        public int? HostPort
        {
            get;
        }
        /// <summary>
        /// 容器类型
        /// </summary>
        public ContainerType ContainerType { get; } = ContainerType.无;

    }
}
