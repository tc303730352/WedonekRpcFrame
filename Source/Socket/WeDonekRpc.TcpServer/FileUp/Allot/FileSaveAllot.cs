using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Helper;
using WeDonekRpc.TcpServer.FileUp.Model;
using WeDonekRpc.TcpServer.FileUp.UpStream;
using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.TcpServer.FileUp.Allot
{
    public class FileSaveAllot<Result> : IStreamAllot
    {
        public string FileId => this._FileStream.FileId;

        private IUpLockFile _FileLock = null;

        private ISaveStream _FileStream = null;


        /// <summary>
        /// 块大小（MB）
        /// </summary>
        public FileUpConfig UpConfig
        {
            get;
            set;
        }
        public FileSaveAllot(string direct)
        {
            this.DirectName = direct;
            this.UpConfig = Config.SocketConfig.UpConfig;
            this.HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
        }
        public FileSaveAllot(string direct, int blockSize, int timeOut = 60)
        {
            this.DirectName = direct;
            this.UpConfig = new FileUpConfig
            {
                BlockSize = blockSize,
                UpTimeOut = timeOut
            };
        }
        public string DirectName { get; }


        public int HeartbeatTime
        {
            get;
            private set;
        }

        public bool CheckIsExists()
        {
            return this._FileStream.CheckIsExists();
        }
        public void DeleteFile()
        {
            this._FileStream.DeleteFile();
        }
        public bool InitFile(UpFile file, out string error)
        {
            if (!this._LoadLockFile(file, out error))
            {
                return false;
            }
            else
            {
                return this._FileStream.CreateStream(file.FileSize, out error);
            }
        }

        private bool _LoadLockFile(UpFile file, out string error)
        {
            if (!this._FileStream.Lock(out this._FileLock, out error))
            {
                return false;
            }
            else
            {
                return this._FileLock.InitLock(file, this.UpConfig.BlockSize, out error);
            }
        }
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool FileAccredit(UpFile file, out string error)
        {
            if (!this.CheckAccredit(file, out error))
            {
                return false;
            }
            else
            {
                this._FileStream = this.GetUpTempFileStream(file);
                return true;
            }
        }
        protected virtual ISaveStream GetUpTempFileStream(UpFile file)
        {
            return new SaveFileStream(file);
        }
        protected virtual bool CheckAccredit(UpFile file, out string error)
        {
            error = "socket.file.up.save.no.unrealized";
            return false;
        }
        public bool Write(ushort blockId, byte[] stream, int begin, int count)
        {
            if (this._FileLock.BeginWrite(blockId, out int position))
            {
                this.HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
                this._FileStream.Write(position, stream, begin, count);
                return this._FileLock.EndWrite(blockId, count);
            }
            return false;
        }
        public bool SaveFileStream(out string error)
        {
            if (this._FileLock.CheckFileSign(this._FileStream))
            {
                this._FileLock.UnLock();
                this._FileStream.SaveStream();
                error = null;
                return true;
            }
            else
            {
                this._FileLock.UnLock();
                this._FileStream.Clear();
                error = "socket.up.file.error";
                return false;
            }
        }
        public bool UpComplate(UpFile file, out byte[] result, out string error)
        {
            if (!this.UpComplate(file, new UpFileResult(this._FileStream), out Result res, out error))
            {
                result = null;
                return false;
            }
            result = ToolsHelper.SerializationData(typeof(Result), res);
            return true;
        }
        public void UpError(UpFile file, string error)
        {
            FileUpState state = this._FileLock.GetUpState();
            this.UpError(file, state, error);
        }

        protected virtual bool UpComplate(UpFile file, UpFileResult upResult, out Result result, out string error)
        {
            result = default;
            error = null;
            return true;
        }
        protected virtual void UpError(UpFile file, FileUpState state, string error)
        {

        }

        public void Dispose()
        {
            if (this._FileStream != null)
            {
                this._FileStream.Dispose();
            }
            if (this._FileLock != null)
            {
                this._FileLock.Dispose();
            }
            this.End();
        }
        protected virtual void End()
        {

        }

        public FileUpState GetFileUpState()
        {
            return this._FileLock.GetUpState();
        }

        public void UpTimeOut()
        {
            this._FileLock.UnLock();
        }

    }
}
