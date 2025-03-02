namespace Store.Gatewary.Modular.Model
{
    internal class StoreUser
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get;
            set;
        }
        /// <summary>
        /// 头像
        /// </summary>
        public string Head { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public string[] Prower
        {
            get;
            set;
        }
    }
}
