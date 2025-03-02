namespace WeDonekRpc.HttpApiGateway.FileUp.Model
{
    public class UpBasicFile : UpFileData
    {
        /// <summary>
        /// 任务Key
        /// </summary>
        public string TaskKey { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        public object Extend { get; set; }
        /// <summary>
        /// 表单数据
        /// </summary>
        public object Form { get; set; }
    }
}
