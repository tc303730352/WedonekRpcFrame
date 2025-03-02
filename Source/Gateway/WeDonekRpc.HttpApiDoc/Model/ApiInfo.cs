namespace WeDonekRpc.HttpApiDoc.Model
{
    /// <summary>
    /// Api接口信息
    /// </summary>
    internal class ApiInfo
    {
        /// <summary>
        /// 接口基础配置
        /// </summary>
        public ApiModel BasicSet
        {
            get;
            set;
        }
        /// <summary>
        /// 提交的数据
        /// </summary>
        public ApiPostBody SubmitBody
        {
            get;
            set;
        }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public ReturnBody ReturnBody
        {
            get;
            set;
        }
        /// <summary>
        /// 数据示例
        /// </summary>
        public string JsonStr { get; set; }
        /// <summary>
        /// GET参数
        /// </summary>
        public ApiPostFormat[] GetParam { get; set; }

        /// <summary>
        /// 上传配置
        /// </summary>
        public object UpConfig { get; set; }
        public string GetStr { get; set; }
    }
}
