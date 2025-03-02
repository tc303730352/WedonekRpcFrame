namespace RpcCentral.Collect.Model
{
    public class ContrainerLogin
    {
        /// <summary>
        /// 容器组编号
        /// </summary>
        public int ContGroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器ID
        /// </summary>
        public string ContrainerId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器内部IP
        /// </summary>
        public string InternalIp { get; set; }
        /// <summary>
        /// 容器内部端口
        /// </summary>
        public int InternalPort { get; set; }
    }
}
