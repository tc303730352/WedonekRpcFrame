using System;

using RpcClient.Model;

using RpcModel;

using SocketTcpServer.FileUp.Allot;
using SocketTcpServer.FileUp.Model;
using SocketTcpServer.FileUp.UpStream;
using SocketTcpServer.Interface;

using RpcHelper;

namespace RpcClient.Route
{
        /// <summary>
        /// 文件上传事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Result"></typeparam>
        [Attr.IgnoreIoc]
        public class FileUpEvent<T, Result> : FileSaveAllot<Result> where T : class
        {
                private readonly string _TypeName = null;
                public FileUpEvent(string name) : base(name)
                {
                        this._TypeName = this.GetType().FullName;
                }
                public sealed override object Clone()
                {
                        return RpcClient.Unity.Resolve<IStreamAllot>(this._TypeName);
                }
                protected sealed override bool CheckAccredit(UpFile file, out string error)
                {
                        UpFileDatum<T> datum = this._GetFileDatum(file);
                        return this.CheckFile(datum, out error);
                }
                private UpFileDatum<T> _GetFileDatum(UpFile file)
                {
                        TcpRemoteMsg msg = file.GetData<TcpRemoteMsg>();
                        return new UpFileDatum<T>
                        {
                                FileName = file.FileName,
                                FileSize = file.FileSize,
                                Param = msg.MsgBody.Json<T>(),
                                Source = msg.Source
                        };
                }
                protected virtual bool CheckFile(UpFileDatum<T> file, out string error)
                {
                        try
                        {
                                this.CheckFile(file);
                                error = null;
                                return true;
                        }
                        catch (Exception e)
                        {
                                ErrorException ex = ErrorException.FormatError(e);
                                RpcLogSystem.AddFileUpError(this.DirectName, file, ex);
                                error = ex.ToString();
                                return false;
                        }
                }
                protected virtual void CheckFile(UpFileDatum<T> file)
                {
                }
                protected sealed override ISaveStream GetUpTempFileStream(UpFile file)
                {
                        UpFileDatum<T> datum = this._GetFileDatum(file);
                        string dir = this.GetFileSaveDir(datum);
                        if (dir == null)
                        {
                                return base.GetUpTempFileStream(file);
                        }
                        return new SaveFileStream(file, dir);
                }
                protected virtual string GetFileSaveDir(UpFileDatum<T> file)
                {
                        return null;
                }
                protected sealed override bool UpComplate(UpFile file, UpFileResult upResult, out Result result, out string error)
                {
                        UpFileDatum<T> datum = this._GetFileDatum(file);
                        try
                        {
                                result = this.UpComplate(datum, upResult);
                                error = null;
                                return true;
                        }
                        catch (Exception e)
                        {
                                result = default;
                                ErrorException ex = ErrorException.FormatError(e);
                                RpcLogSystem.AddFileUpError(this.DirectName, datum, ex);
                                error = ex.ToString();
                                return false;
                        }
                }
                protected virtual Result UpComplate(UpFileDatum<T> file, UpFileResult stream)
                {
                        return default;
                }
        }
}
