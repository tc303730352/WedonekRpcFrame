using System;
using WeDonekRpc.Client.FileUp.Model;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Helper;
using WeDonekRpc.TcpServer.FileUp.Allot;
using WeDonekRpc.TcpServer.FileUp.Model;
using WeDonekRpc.TcpServer.FileUp.UpStream;

namespace WeDonekRpc.Client.FileUp
{
    internal class SocketFileUpEvent<Result> : FileSaveAllot<Result>, ISocketFileUpEvent<Result>
    {
        private readonly string _TypeName;
        private readonly IIocService _Ioc;
        private IocScope _IocScope;
        public SocketFileUpEvent (string name) : base(name)
        {
            this._TypeName = "Socket_" + name;
            this._Ioc = RpcClient.Ioc;
        }
        public IFileUpEvent<Result> UpEvent { get; set; }
        public void Install (IocBuffer buffer)
        {
            _ = buffer.RegisterInstance<ISocketFileUpEvent<Result>>(this, this._TypeName);
        }
        public sealed override object Clone ()
        {
            this._IocScope = this._Ioc.CreateTempScore();
            ISocketFileUpEvent<Result> clone = this._IocScope.Resolve<ISocketFileUpEvent<Result>>(this._TypeName);
            clone.UpEvent = this._Ioc.Resolve<IFileUpEvent<Result>>(this.DirectName);
            return clone;
        }
        protected override void End ()
        {
            this._IocScope.Dispose();
        }
        protected sealed override bool CheckAccredit (UpFile file, out string error)
        {
            IUpFileInfo upFile = new SocketUpFile(file);
            try
            {
                this.UpEvent.CheckFile(upFile);
                error = null;
                return true;
            }
            catch (Exception e)
            {
                ErrorException ex = ErrorException.FormatError(e);
                RpcLogSystem.AddFileUpError(this.DirectName, upFile, ex);
                error = ex.ToString();
                return false;
            }
        }

        protected sealed override ISaveStream GetUpTempFileStream (UpFile file)
        {
            string dir = this.UpEvent.GetFileSaveDir(new SocketUpFile(file));
            if (dir == null)
            {
                return base.GetUpTempFileStream(file);
            }
            return new SaveFileStream(file, dir);
        }

        protected sealed override bool UpComplate (UpFile file, UpFileResult upResult, out Result result, out string error)
        {
            IUpFileInfo upFile = new SocketUpFile(file);
            try
            {
                result = this.UpEvent.UpComplate(upFile, new SocketUpResult(upResult));
                error = null;
                return true;
            }
            catch (Exception e)
            {
                result = default;
                ErrorException ex = ErrorException.FormatError(e);
                RpcLogSystem.AddFileUpError(this.DirectName, upFile, ex);
                error = ex.ToString();
                return false;
            }
        }
    }
}
