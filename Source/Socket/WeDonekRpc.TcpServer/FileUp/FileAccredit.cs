using System.Threading;

using WeDonekRpc.TcpServer.FileUp.Collect;
using WeDonekRpc.TcpServer.FileUp.Model;
using WeDonekRpc.TcpServer.Interface;

using WeDonekRpc.Helper;

namespace WeDonekRpc.TcpServer.FileUp
{

    internal class FileAccredit : System.IDisposable
    {
        private static int _PageId = 0;

        private static int _GetPageId()
        {
            return Interlocked.Increment(ref _PageId);
        }

        private volatile UpFileStatus _UpFileStatus = UpFileStatus.上传中;

        private string _UpError = null;
        private byte[] _UpResult = null;
        public FileAccredit(UpFile file, IStreamAllot allot)
        {
            this._StreamAllot = allot;
            this._File = file;
            this.PageId = _GetPageId();
        }
        public int PageId
        {
            get;
        }

        internal UpFileStatus UpFileStatus => this._UpFileStatus;

        private volatile int _HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;

        private readonly UpFile _File = null;


        private readonly IStreamAllot _StreamAllot = null;

        private static readonly int _Begin = 6;
        internal void WriteStream(ushort blockId, byte[] stream)
        {
            this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
            if (this._StreamAllot.Write(blockId, stream, _Begin, stream.Length - _Begin))
            {
                this._EndUp();
            }
        }
        private void _SetUpResult(UpFileStatus status, string error)
        {
            if (status == UpFileStatus.传输错误)
            {
                this._UpError = error;
                this._UpFileStatus = status;
                this._StreamAllot.UpError(this._File, error);
            }
            else if (!this._StreamAllot.UpComplate(this._File, out this._UpResult, out this._UpError))
            {
                this._UpFileStatus = UpFileStatus.传输错误;
            }
            else
            {
                this._UpFileStatus = status;
            }
        }
        private void _EndUp()
        {
            FileAccreditCollect.RemoveBind(this._StreamAllot.FileId);
            if (!this._StreamAllot.SaveFileStream(out string error))
            {
                this._SetUpResult(UpFileStatus.传输错误, error);
            }
            else
            {
                this._SetUpResult(UpFileStatus.已结束, null);
            }
        }
        internal bool SyncUpState(out FileUpState state, out string error)
        {
            if (!this._StreamAllot.InitFile(this._File, out error))
            {
                state = null;
                return false;
            }
            else
            {
                FileAccreditCollect.BindAccredit(this._StreamAllot.FileId, this.PageId);
                state = this._StreamAllot.GetFileUpState();
                return true;
            }
        }
        public bool CheckIsOverTime(int now)
        {
            if (this._UpFileStatus != UpFileStatus.上传中)
            {
                return this._HeartbeatTime <= (now - 10);
            }
            else if (this._HeartbeatTime <= (now - this._StreamAllot.UpConfig.UpTimeOut))
            {
                FileAccreditCollect.RemoveBind(this._StreamAllot.FileId);
                this._StreamAllot.UpTimeOut();
                this._SetUpResult(UpFileStatus.传输错误, "socket.file.up.timeout");
                return true;
            }
            return false;
        }
        internal FileUpResult GetUpResult()
        {
            if (this._UpFileStatus == UpFileStatus.上传中)
            {
                return new FileUpResult
                {
                    UpStatus = _UpFileStatus,
                    UpState = this._StreamAllot.GetFileUpState()
                };
            }
            return new FileUpResult
            {
                UpError = _UpError,
                UpStatus = _UpFileStatus,
                UpResult = this._UpResult
            };
        }

        public void Dispose()
        {
            this._StreamAllot.Dispose();
        }
    }
}
