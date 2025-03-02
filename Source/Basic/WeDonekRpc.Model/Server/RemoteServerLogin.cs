using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.Model.Server
{
    /// <summary>
    /// 远程服务器登陆
    /// </summary>
    public class RemoteServerLogin
    {
        /// <summary>
        /// 服务器类型
        /// </summary>
        [NullValidate("rpc.server.type.null")]
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务器MAC
        /// </summary>
        [EntrustValidate("_Check")]
        [FormatValidate("rpc.server.mac.error", ValidateFormat.MAC)]
        public string ServerMac
        {
            get;
            set;
        }
        /// <summary>
        /// 服务器编号
        /// </summary>
        public int ServerIndex { get; set; }

        /// <summary>
        /// 进程信息
        /// </summary>
        public ProcessDatum Process { get; set; }

        /// <summary>
        /// 容器类型
        /// </summary>
        [EnumValidate("rpc.container.type.error", typeof(ContainerType))]
        public ContainerType ContainerType { get; set; }
        /// <summary>
        /// 容器信息
        /// </summary>
        public ContainerInfo Container { get; set; }


    }
}
