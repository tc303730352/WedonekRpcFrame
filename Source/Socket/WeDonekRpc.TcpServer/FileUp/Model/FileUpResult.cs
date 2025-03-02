namespace WeDonekRpc.TcpServer.FileUp.Model
{
        internal enum UpFileStatus
        {
                上传中 = 0,
                已结束 = 1,
                传输错误 = 2
        }
        internal class FileUpResult
        {
                public UpFileStatus UpStatus
                {
                        get;
                        set;
                }
                public string UpError
                {
                        get;
                        set;
                }
                public FileUpState UpState
                {
                        get;
                        set;
                }
                public byte[] UpResult { get; set; }
        }
}
