namespace RpcCentral.Common.Config
{
    public class RpcServerSysConfig
    {
        /// <summary>
        /// 服务状态变更是否通知
        /// </summary>
        public bool ServerStateNotice
        {
            get;
            set;
        }


        /// <summary>
        /// Email列表
        /// </summary>
        public string[] EmailList
        {
            get;
            set;
        }
        /// <summary>
        /// 发件人显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 发件人账号
        /// </summary>
        public string EmailAccount { get; set; }

        /// <summary>
        /// 发件人密码
        /// </summary>
        public string EmailPwd { get; set; }
    }
}
