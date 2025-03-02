namespace WeDonekRpc.TcpServer.FileUp.Model
{
        internal class UpFileInfo
        {
                /// <summary>
                /// 文件名
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
                /// 上传参数
                /// </summary>
                public byte[] UpParam
                {
                        get;
                        set;
                }
                /// <summary>
                /// 文件校验码
                /// </summary>
                public string FileSign { get; set; }
                /// <summary>
                /// 是否是MD5
                /// </summary>
                public bool IsMd5 { get; set; }
        }
}
