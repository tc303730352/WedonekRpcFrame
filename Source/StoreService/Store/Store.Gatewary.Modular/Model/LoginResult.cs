namespace Store.Gatewary.Modular.Model
{
    public class LoginResult
    {
        /// <summary>
        /// 授权ID
        /// </summary>
        public string AccreditId { get; set; }

        /// <summary>
        /// 账号名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHead { get; set; }

        /// <summary>
        /// 登陆权限
        /// </summary>
        public string[] Prower { get; set; }
        public object Introduction { get; set; }
    }
}
