using System;
using System.IO;

using WeDonekRpc.TcpServer.FileUp.Model;

namespace WeDonekRpc.TcpServer.FileUp.UpStream
{
        public class UpLockStream : IUpLockFile
        {
                private Stream _FileLock;

                private FileLockState _LockState = null;

                public bool LoadLock(out string error)
                {
                        if (!this._LoadStream(out this._FileLock, out error))
                        {
                                return false;
                        }
                        this._LockState = new FileLockState();
                        if (this._FileLock.Length != 0)
                        {
                                this._LockState.Load(this._FileLock);
                        }
                        error = null;
                        return true;
                }
                protected virtual bool _LoadStream(out Stream stream, out string error)
                {
                        stream = null;
                        error = "socket.up.lock.no.realize";
                        return false;
                }

                public bool InitLock(UpFile file, int blockSize, out string error)
                {
                        if (this._FileLock.Length == 0)
                        {
                                this._LockState.FileSize = file.FileSize;
                                this._LockState.FileSign = file.FileSign;
                                this._LockState.IsMd5 = file.IsMd5;
                                this._LockState.BlockSize = blockSize;
                                this._LockState.BlockNum = (ushort)Math.Ceiling(file.FileSize / (decimal)blockSize);
                                this._LockState.Init(this._FileLock);
                        }
                        error = null;
                        return true;
                }

                public FileUpState GetUpState()
                {
                        return this._LockState.GetUpState();
                }

                public bool BeginWrite(ushort blockId, out int position)
                {
                        return this._LockState.BeginWrite(blockId, out position);
                }

                public bool EndWrite(ushort blockId, int count)
                {
                        return this._LockState.EndWrite(this._FileLock, blockId, count);
                }

                public void Dispose()
                {
                        if (this._FileLock != null)
                        {
                                this._FileLock.Close();
                                this._FileLock.Dispose();
                                this._FileLock = null;
                        }
                }

                public virtual void UnLock()
                {
                        if (this._FileLock != null)
                        {
                                this._FileLock.Flush();
                        }
                        this.Dispose();
                }

                public bool CheckFileSign(ISaveStream stream)
                {
                        string sign = FileUpHelper.GetFileKey(this._LockState, stream.GetStream());
                        return sign == this._LockState.FileSign;
                }
        }
}
