using WeDonekRpc.Helper.Validate;
namespace WeDonekRpc.Model
{
    public class ContainerInfo
    {
        /// <summary>
        /// 容器Id
        /// </summary>
        [NullValidate("rpc.container.id.null")]
        public string ContainerId
        {
            get;
            set;
        }
        /// <summary>
        /// 内部端口号
        /// </summary>
        public int? InsidePort
        {
            get;
            set;
        }
        /// <summary>
        /// 宿主机IP
        /// </summary>
        public string HostIp { get; set; }
        /// <summary>
        /// 宿主端口
        /// </summary>
        public int? HostPort { get; set; }
        /// <summary>
        /// 本地Ip
        /// </summary>
        public string LocalIp { get; set; }

    }
}
