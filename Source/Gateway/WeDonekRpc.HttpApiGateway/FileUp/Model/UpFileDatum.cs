namespace WeDonekRpc.HttpApiGateway.FileUp.Model
{
    public class UpFileDatum : UpFileData
    {
        /// <summary>
        /// 任务Key
        /// </summary>
        public string TaskKey { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        public object Extend { get; set; }
    }
}
