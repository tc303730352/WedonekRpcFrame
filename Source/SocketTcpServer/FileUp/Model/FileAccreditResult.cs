namespace SocketTcpServer.FileUp.Model
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
