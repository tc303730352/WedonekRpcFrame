namespace WeDonekRpc.TcpClient.UpFile.Model
{
        /// <summary>
        /// 文件上传状态
        /// </summary>
        public class FileUpState
        {
                /// <summary>
                /// 已经上传总量
                /// </summary>
                public long AlreadyUpNum
                {
                        get;
                        set;
                }

                /// <summary>
                /// 块大小
                /// </summary>
                public int BlockSize
                {
                        get;
                        set;
                }
                /// <summary>
                /// 块数量
                /// </summary>
                public ushort BlockNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 未上传的块
                /// </summary>
                public int[] NullBlock
                {
                        get;
                        set;
                }
                /// <summary>
                /// 开始上传的块索引
                /// </summary>
                public int BeginBlock
                {
                        get;
                        set;
                }
        }
}
