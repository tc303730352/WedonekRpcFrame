namespace RpcCentral.Model
{
    public class ContainerDatum
    {
        /// <summary>
        /// 容器内部IP
        /// </summary>
        public string InternalIp { get; set; }

        /// <summary>
        /// 容器内部端口
        /// </summary>
        public int InternalPort { get; set; }

        /// <summary>
        /// 宿主Mac
        /// </summary>
        public string HostMac { get; set; }

        /// <summary>
        /// 宿主Ip
        /// </summary>
        public string HostIp { get; set; }
    }
}
