namespace WeDonekRpc.TcpClient.UpFile.Model
{
        internal class FileAccreditResult : FileBasicResult
        {
                public int PageId
                {
                        get;
                        set;
                }
                public FileUpState UpState
                {
                        get;
                        set;
                }
        }
}
