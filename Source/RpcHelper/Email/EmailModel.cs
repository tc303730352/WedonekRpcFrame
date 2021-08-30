namespace RpcHelper
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
                public string Title { get; set; }
                public string Content { get; set; }
                public string EmailAccount { get; set; }
                public string EmailPwd { get; set; }

                public string SmtpClient { get; set; }
                public int SmtpClientPort { get; set; }
        }
}
