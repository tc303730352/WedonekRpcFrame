namespace WeDonekRpc.HttpApiGateway.FileUp.Model
{
    public class UpFileData
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
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
        /// <summary>
        /// 文件MD5
        /// </summary>
        public string FileMd5
        {
            get;
            set;
        }
    }

    public class UpFileData<T> : UpFileData
    {
        /// <summary>
        /// 表单
        /// </summary>
        public T Form
        {
            get;
            set;
        }
    }
}
