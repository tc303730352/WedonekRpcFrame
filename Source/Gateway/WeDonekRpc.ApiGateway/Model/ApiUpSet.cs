namespace WeDonekRpc.ApiGateway.Model
{
    public class ApiUpSet
    {
        /// <summary>
        /// 限定一次请求上传文件数量（0 无限制）
        /// </summary>
        public int LimitFileNum
        {
            get;
            set;
        }
        /// <summary>
        /// 文件扩展
        /// </summary>
        public string[] Extension
        {
            get;
            set;
        }

        /// <summary>
        /// 最大文件大小
        /// </summary>
        public int MaxSize
        {
            get;
            set;
        }
        /// <summary>
        /// 分块上传大小
        /// </summary>
        public int BlockUpSize
        {
            get;
            set;
        }
        /// <summary>
        /// 临时文件保存目录
        /// </summary>
        public string TempFileSaveDir
        {
            get;
            set;
        }
        /// <summary>
        /// 是否计算MD5
        /// </summary>
        public bool IsGenerateMd5
        {
            get;
            set;
        }
    }
}
