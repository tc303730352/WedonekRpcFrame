using WeDonekRpc.Helper;

namespace RpcExtend.Service.Config
{
    internal class ErrorEmailConfig
    {
        /// <summary>
        /// Email发送限定等级
        /// </summary>
        public LogGrade InfoLimitGrade { get; set; } = LogGrade.ERROR;

        /// <summary>
        /// 错误发送限定等级
        /// </summary>
        public LogGrade ErrorLimitGrade { get; set; } = LogGrade.ERROR;

        /// <summary>
        /// Email账户
        /// </summary>
        public string EmailAccount { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string[] Reciver { get; set; }

        /// <summary>
        /// Email账户密码
        /// </summary>
        public string EmailPwd { get; set; }

        /// <summary>
        /// 是否发送Email
        /// </summary>
        public bool IsSendEmail { get; set; } = false;
        /// <summary>
        /// 发件人显示名
        /// </summary>
        public string DisplayName { get; set; }
    }
}
