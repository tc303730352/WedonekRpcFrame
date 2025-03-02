namespace WeDonekRpc.HttpApiGateway.FileUp.Model
{
    public class BlockDatum
    {
        /// <summary>
        /// 块大小
        /// </summary>
        public int BlockSize
        {
            get;
            set;
        }
        /// <summary>
        /// 未上传索引
        /// </summary>
        public int[] NoUpIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 已上传
        /// </summary>
        public long AlreadyUp
        {
            get;
            set;
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize
        {
            get;
            set;
        }
    }

    public class BlockUpSate
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public string TaskId
        {
            get;
            set;
        }
        /// <summary>
        /// 上传状态
        /// </summary>
        public BlockUpState UpState
        {
            get;
            set;
        }
        /// <summary>
        /// 分块信息
        /// </summary>
        public BlockDatum Block
        {
            get;
            set;
        }
        /// <summary>
        /// 结果
        /// </summary>
        public dynamic Result
        {
            get;
            set;
        }
    }
}
