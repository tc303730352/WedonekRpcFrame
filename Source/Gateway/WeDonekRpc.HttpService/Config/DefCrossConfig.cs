namespace WeDonekRpc.HttpService.Config
{
    public class DefCrossConfig
    {
        public bool IsEnable { get; set; } = true;
        /// <summary>
        /// 允许跨域
        /// </summary>
        public bool AllowCredentials
        {
            get;
            set;
        } = true;

        /// <summary>
        /// 跨域限定允许访问来源
        /// </summary>
        public string[] AllowUrlReferrer
        {
            get;
            set;
        }
        /// <summary>
        /// 跨域限定头部
        /// </summary>
        public string AllowHead { get; set; } = "*";
        /// <summary>
        /// 有效时间
        /// </summary>
        public string MaxAge
        {
            get;
            set;
        } = "3600";

        /// <summary>
        /// 允许的请求方式 ,号分隔
        /// </summary>
        public string Method
        {
            get;
            set;
        } = "POST,GET,OPTIONS";

    }
}
