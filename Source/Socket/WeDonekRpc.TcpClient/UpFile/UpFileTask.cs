using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Manage;
using WeDonekRpc.TcpClient.Model;
using WeDonekRpc.TcpClient.UpFile.Model;

namespace WeDonekRpc.TcpClient.UpFile
{

    internal class UpFileTask : DataSyncClass
    {
        public string TaskId
        {
            get;
        }
        /// <summary>
        /// 上传进度
        /// </summary>
        public UpFileProgress UpProgress => this._UpProgress;

        /// <summary>
        /// 已上传数量
        /// </summary>
        public long AlreadyUpNum => Interlocked.Read(ref this._AlreadyUpNum);

        /// <summary>
        /// 上传超时的错误状态
        /// </summary>
        private static readonly FileUpRes _UpOverTime = new FileUpRes
        {
            UpError = "socket.file.up.timeout",
            UpStatus = UpFileStatus.传输错误
        };
        /// <summary>
        /// 上传取消的错误状态
        /// </summary>
        private static readonly FileUpRes _UpCancel = new FileUpRes
        {
            UpError = "socket.file.up.cancel",
            UpStatus = UpFileStatus.传输错误
        };
        private volatile UpFileProgress _UpProgress = UpFileProgress.待上传;

        private readonly FileInfo _File;

        private readonly string _ServerId = null;

        private readonly object _UpParam = null;

        private readonly UpFileAsync _Async = null;

        private readonly UpProgressAction _Progress = null;

        private FileStream _FileStream = null;

        private int _BeginTime = 0;

        private byte[] _PageId = null;

        private int[] _NullBlock = null;

        private bool _IsNullBlock = false;

        private int _NullBlockIndex = -1;

        private int _Size = 0;

        private long _FileSize = 0;

        private int _PageSum = 0;

        private int _BeginIndex = 0;

        private int _EndTime = 0;

        internal void Cancel ()
        {
            if ( this._UpProgress != UpFileProgress.待上传 )
            {
                this._UpProgress = UpFileProgress.已取消;
                this._UpEnd(_UpCancel);
            }
        }

        /// <summary>
        /// 已经上传总量
        /// </summary>
        private long _AlreadyUpNum = 0;
        public UpFileTask ( string direct, object param, FileInfo file, UpFileAsync async, string serverId, UpProgressAction progress )
        {
            this._UpParam = param;
            this._DirectName = direct;
            this.TaskId = string.Join("_", direct, file.FullName, serverId).GetMd5();
            this._Async = async;
            this._ServerId = serverId;
            this._File = file;
            this._Progress = progress;
        }
        private readonly string _DirectName = null;

        public override void Dispose ()
        {
            base.Dispose();
            if ( this._FileStream != null )
            {
                this._FileStream.Close();
                this._FileStream.Dispose();
            }
        }
        protected override void SyncData ()
        {
            this._LoadFile();
            this._FileAccredit();
        }

        protected void _LoadFile ()
        {
            try
            {
                this._FileStream = this._File.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                this._FileSize = this._FileStream.Length;
            }
            catch ( IOException )
            {
                throw new ErrorException("socket.up.file.open.error", new Dictionary<string, string>
                                {
                                        { "FileName",this._File.Name},
                                        { "FilePath",this._File.FullName},
                                        { "FileSize",this._FileSize.ToString()}
                                });
            }
        }

        private void _UpEnd ( FileUpRes res )
        {
            UpFileTaskCollect.RemoveTask(this.TaskId);
            int span = 0;
            if ( res.UpStatus == UpFileStatus.已结束 )
            {
                this._SendProgress(100, this._FileSize);
                span = this._EndTime - this._BeginTime;
            }
            IFileUpResult result = new FileUpResult(res.UpResult)
            {
                IsSuccess = res.UpStatus == UpFileStatus.已结束,
                Error = res.UpError,
                File = this._File,
                UpParam = this._UpParam,
                ConsumeTime = span
            };
            this._Async.Invoke(result);
        }
        private void _InitUpProgress ( FileUpState state )
        {
            _ = Interlocked.Exchange(ref this._AlreadyUpNum, state.AlreadyUpNum);
            this._BeginIndex = state.BeginBlock;
            this._NullBlock = state.NullBlock;
            this._IsNullBlock = this._NullBlock != null && this._NullBlock.Length > 0;
        }
        private void _FileAccredit ()
        {
            byte[] myByte = this._UpParam == null ? null : ToolsHelper.SerializationData(this._UpParam.GetType(), this._UpParam);
            string sign = UpFileHelper.GetFileCheckKey(this._FileSize, this._FileStream, out bool isMd5);
            FilePage data = new FilePage
            {
                File = new UpFileInfo
                {
                    UpParam = myByte,
                    FileName = this._File.Name,
                    FileSize = this._FileStream.Length,
                    FileSign = sign,
                    IsMd5 = isMd5
                },
                DirectName = this._DirectName
            };
            Page page = Page.GetUpFilePage(this._ServerId, "FileAccredit", data);
            if ( !PageManage.Send(page, out FileAccreditResult result, out string error) )
            {
                throw new ErrorException("socket.file.accredit.error", new Dictionary<string, string> {
                                        { "Error",error},
                                        { "FilePage",data.ToJson()}
                                });
            }
            else if ( result.IsError )
            {
                throw new ErrorException(result.ErrorCode, new Dictionary<string, string> {
                                        { "Error",error},
                                        { "FilePage",data.ToJson()}
                                });
            }
            else
            {
                this._PageId = BitConverter.GetBytes(result.PageId);
                this._InitUpProgress(result.UpState);
                this._Size = result.UpState.BlockSize;
                this._PageSum = (int)Math.Ceiling(this._FileSize / (decimal)this._Size) - 1;
            }
        }
        private struct _Page
        {
            public int Index;
            public int Len;
        }
        private readonly LockHelper _Lock = new LockHelper();
        private void _SendStream ( int index )
        {
            int skip = index * this._Size;
            byte[] myByte = null;
            int len = this._Size;
            if ( index == this._PageSum && ( skip + this._Size ) > this._FileSize )
            {
                myByte = new byte[this._FileSize - skip + 6];
                len = myByte.Length - 6;
            }
            else
            {
                myByte = new byte[this._Size + 6];
            }
            this._PageId.CopyTo(myByte, 0);
            BitConverter.GetBytes((ushort)index).CopyTo(myByte, 4);
            if ( this._Lock.GetLock() )
            {
                this._FileStream.Position = skip;
                _ = this._FileStream.Read(myByte, 6, len);
                this._Lock.Exit();
            }
            Page page = Page.GetUpFileStream(this._ServerId, myByte);
            PageManage.Send(page, new Async(this._SendComplate), new _Page
            {
                Index = index,
                Len = len
            });
        }
        private void _SendProgress ( int progress, long num )
        {
            this._Progress?.Invoke(this._File, progress, num);
        }
        private void _SendComplate ( IAsyncEvent e )
        {
            if ( this._UpProgress == UpFileProgress.已取消 )
            {
                return;
            }
            this.HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
            _Page page = (_Page)e.Arg;
            if ( !e.IsError )
            {
                long num = Interlocked.Add(ref this._AlreadyUpNum, page.Len);
                int progress = (int)( num * 100 / this._FileSize );
                if ( progress == 100 )
                {
                    this._EndTime = HeartbeatTimeHelper.HeartbeatTime;
                    Thread.Sleep(100);
                    this._SyncUpState();
                    return;
                }
                this._SendProgress(progress, num);
                this._SendPage();
                return;
            }
            else if ( ClientManage.CheckClientIsUsable(this._ServerId) )
            {
                this._SendStream(page.Index);
            }
            else if ( this._UpProgress == UpFileProgress.上传中 )
            {
                this._UpProgress = UpFileProgress.连接断开;
            }
        }
        public void CheckUpState ( int sendTimeOut )
        {
            if ( this.HeartbeatTime <= sendTimeOut )
            {
                this._UpEnd(_UpOverTime);
            }
            else if ( this._UpProgress == UpFileProgress.连接断开 && ClientManage.Ping(this._ServerId) )
            {
                this._SyncUpState();
            }
        }
        private void _SyncUpState ()
        {
            if ( !this._SyncUpState(out string error) )
            {
                new LogInfo(error, LogGrade.WARN, "Socket_Client")
                {
                    LogTitle = "同步上传状态错误"
                }.Save();
            }
        }
        private bool _SyncUpState ( out string error )
        {
            Page page = Page.GetUpFilePage(this._ServerId, "SyncUpState", this._PageId);
            if ( !PageManage.Send(page, out SyncUpResult result, out error) )
            {
                return false;
            }
            else if ( result.IsError )
            {
                error = result.ErrorCode;
                return false;
            }
            else if ( result.Result.UpStatus != UpFileStatus.上传中 )
            {
                this._UpEnd(result.Result);
                return true;
            }
            else
            {
                this._InitUpProgress(result.Result.UpState);
                this._SendPage();
                return true;
            }
        }
        private void _SendPage ()
        {
            int index = Interlocked.Increment(ref this._BeginIndex);
            if ( index <= this._PageSum )
            {
                this._SendStream(index);
            }
            else if ( this._IsNullBlock )
            {
                index = Interlocked.Increment(ref this._NullBlockIndex);
                if ( index < this._NullBlock.Length )
                {
                    this._SendStream(this._NullBlock[index]);
                }
            }
        }
        public async void BeginTask ()
        {
            if ( this._UpProgress != UpFileProgress.上传中 )
            {
                this._UpProgress = UpFileProgress.上传中;
                this._BeginTime = HeartbeatTimeHelper.HeartbeatTime;
                await Task.Run(() =>
                 {
                     for ( int i = 0 ; i < 10 ; i++ )
                     {
                         this._SendPage();
                     }
                 });
            }
        }

        internal long GetUpSpeed ( out long upNum, out int time )
        {
            upNum = Interlocked.Read(ref this._AlreadyUpNum);
            time = HeartbeatTimeHelper.HeartbeatTime - this._BeginTime;
            return upNum == 0 || time == 0 ? 0 : upNum / time / 1024;
        }
    }
}
