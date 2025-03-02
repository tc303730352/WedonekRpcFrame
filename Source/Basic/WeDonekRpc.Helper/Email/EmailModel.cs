namespace WeDonekRpc.Helper.Email
{
    public class EmailModel
    {
        /// <summary>
        /// 发件人显示名
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }
        /// <summary>
        /// 设置多个收件人
        /// </summary>
        public string[] Reciver
        {
            get;
            set;
        }
        /// <summary>
        /// 设置抄送人
        /// </summary>
        public string[] CC
        {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发送者邮箱账号
        /// </summary>
        public string EmailAccount { get; set; }
        /// <summary>
        /// 发送者邮箱密码
        /// </summary>
        public string EmailPwd { get; set; }
        /// <summary>
        /// 邮箱服务地址
        /// </summary>
        public string SmtpClient { get; set; }
        /// <summary>
        /// 邮箱服务端口号
        /// </summary>
        public int SmtpClientPort { get; set; }
    }
}
