namespace WeDonekRpc.TcpServer.FileUp.Model
{
        public class FileUpConfig
        {
                private int _BlockSize = 5 * (1024 * 1024);
                /// <summary>
                /// 块大小（KB）
                /// </summary>
                public int BlockSize
                {
                        get => this._BlockSize;
                        set => this._BlockSize = value * 1024;
                }
                /// <summary>
                /// 上传超时时间
                /// </summary>
                public int UpTimeOut
                {
                        get;
                        set;
                } = 30;
        }
}
