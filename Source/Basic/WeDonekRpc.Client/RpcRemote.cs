using System.IO;
using System.Threading.Tasks;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.Client
{
    public class RpcBroadcast
    {
        public void Send ()
        {
            RemoteCollect.BroadcastMsg(this);
        }
        public async void SyncSend ()
        {
            await new Task(this.Send);
        }
        public async void SyncSend (int regionId)
        {
            await new Task(this._Send, new BroadcastSet
            {
                RegionId = regionId
            });
        }
        public async void SyncSend (int regionId, long rpcMerId)
        {
            await new Task(this._Send, new BroadcastSet
            {
                RpcMerId = rpcMerId,
                RegionId = regionId
            });
        }
        public async void SyncSend (long rpcMerId)
        {
            await new Task(this._Send, new BroadcastSet
            {
                RpcMerId = rpcMerId
            });
        }
        public async void SyncSend (BroadcastSet set)
        {
            await new Task(this._Send, set);
        }
        public void Send (BroadcastSet set)
        {
            RemoteCollect.BroadcastMsg(this, set);
        }
        private void _Send (object state)
        {
            BroadcastSet set = (BroadcastSet)state;
            RemoteCollect.BroadcastMsg(this, set);
        }
        public void Send (string dic)
        {
            RemoteCollect.BroadcastMsg(dic, this);
        }
        public void Send (string[] sysType)
        {
            RemoteCollect.BroadcastSysTypeMsg(this, sysType);
        }
        public void Send (string[] sysType, long rpcMerId)
        {
            RemoteCollect.BroadcastMsg(this, rpcMerId, sysType);
        }
        public void Send (long[] serverId)
        {
            RemoteCollect.BroadcastMsg(this, serverId);
        }
        public void Send (string dictate, string sysType)
        {
            RemoteCollect.BroadcastMsg(dictate, this, sysType);
        }
        public void Send (string dictate, string[] sysType)
        {
            RemoteCollect.BroadcastMsg(dictate, this, sysType);
        }
    }
    public class RpcRemote
    {
        public void SyncSend ()
        {
            _ = Task.Factory.StartNew(() =>
            {
                if (!RemoteCollect.Send(this, out string error))
                {
                    new ErrorException(error).SaveLog("Rpc_Msg");
                }
            });
        }
        public Task AsyncSend ()
        {
            return Task.Run(() =>
            {
                if (!RemoteCollect.Send(this, out string error))
                {
                    throw new ErrorException(error);
                }
            });
        }
        public void Send ()
        {
            if (!RemoteCollect.Send(this, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public bool Send (out string error)
        {
            return RemoteCollect.Send(this, out error);
        }
        public void Send (long serverId)
        {
            if (!RemoteCollect.Send(serverId, this, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public void Send (long rpcMerId, int? regionId)
        {
            if (!RemoteCollect.Send(rpcMerId, regionId, this, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public bool Send (long serverId, out string error)
        {
            return RemoteCollect.Send(serverId, this, out error);
        }
        public bool Send (string sysType, out string error)
        {
            return RemoteCollect.Send(sysType, this, out error);
        }
    }

    public class RpcRemoteArray<Result>
    {
        public bool Send (out Result[] datas, out string error)
        {
            return RemoteCollect.Send(this, out datas, out error);
        }
        public Result[] Send ()
        {
            if (!RemoteCollect.Send(this, out Result[] datas, out string error))
            {
                throw new ErrorException(error);
            }
            return datas;
        }
        public Task<Result[]> AsyncSend ()
        {
            return Task.Run<Result[]>(() =>
            {
                if (!RemoteCollect.Send(this, out Result[] data, out string error))
                {
                    throw new ErrorException(error);
                }
                return data;
            });
        }
    }
    public class RpcRemoteStream<Result>
    {
        public Result Send (Stream stream)
        {
            if (!RemoteCollect.SendStream(this, stream.ToBytes(), out Result data, out string error))
            {
                throw new ErrorException(error);
            }
            return data;
        }
        public Result Send (byte[] stream)
        {
            if (!RemoteCollect.SendStream(this, stream, out Result data, out string error))
            {
                throw new ErrorException(error);
            }
            return data;
        }
        public Task<Result> AsyncSend (byte[] stream)
        {
            return Task.Run<Result>(() =>
            {
                if (!RemoteCollect.SendStream(this, stream, out Result data, out string error))
                {
                    throw new ErrorException(error);
                }
                return data;
            });
        }
    }
    public class RpcRemoteStream
    {
        public void Send (Stream stream)
        {
            if (!RemoteCollect.SendStream(this, stream.ToBytes(), out string error))
            {
                throw new ErrorException(error);
            }
        }
        public void Send (byte[] stream)
        {
            if (!RemoteCollect.SendStream(this, stream, out string error))
            {
                throw new ErrorException(error);
            }
        }

    }
    public class RpcUpFile
    {
        /// <summary>
        /// 发送文件并返回一个上传任务
        /// </summary>
        /// <param name="path">文件物理路径</param>
        /// <param name="func">上传结果回调</param>
        /// <param name="progress">上传进度</param>
        /// <returns>上传任务</returns>
        /// <exception cref="ErrorException">错误</exception>
        public IUpTask Send (string path, UpFileAsync func, UpProgressAction progress)
        {
            if (!this.Send(new FileInfo(path), func, progress, out IUpTask upTask, out string error))
            {
                throw new ErrorException(error);
            }
            return upTask;
        }
        public IUpTask Send (long serverId, string path, UpFileAsync func, UpProgressAction progress)
        {
            if (!this.Send(serverId, new FileInfo(path), func, progress, out IUpTask upTask, out string error))
            {
                throw new ErrorException(error);
            }
            return upTask;
        }
        public bool Send (string path, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error)
        {
            return this.Send(new FileInfo(path), func, progress, out upTask, out error);
        }
        public bool Send (FileInfo file, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error)
        {
            return RemoteCollect.SendFile(this, file, func, progress, out upTask, out error);
        }
        public bool Send (long serverId, FileInfo file, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error)
        {
            return RemoteCollect.SendFile(serverId, this, file, func, progress, out upTask, out error);
        }
        public IUpTask Send (FileInfo file, UpFileAsync func, UpProgressAction progress)
        {
            if (!this.Send(file, func, progress, out IUpTask upTask, out string error))
            {
                throw new ErrorException(error);
            }
            return upTask;
        }
        public IUpTask Send (long serverId, FileInfo file, UpFileAsync func, UpProgressAction progress)
        {
            if (!this.Send(serverId, file, func, progress, out IUpTask upTask, out string error))
            {
                throw new ErrorException(error);
            }
            return upTask;
        }
        public bool Send (string path, UpFileAsync func, out IUpTask upTask, out string error)
        {
            return this.Send(new FileInfo(path), func, null, out upTask, out error);
        }
        public bool Send (FileInfo file, UpFileAsync func, out IUpTask upTask, out string error)
        {
            return this.Send(file, func, null, out upTask, out error);
        }

        public IUpTask Send (string path, UpFileAsync func)
        {
            if (!this.Send(path, func, null, out IUpTask upTask, out string error))
            {
                throw new ErrorException(error);
            }
            return upTask;
        }
        public IUpTask Send (FileInfo file, UpFileAsync func)
        {
            if (!this.Send(file, func, null, out IUpTask upTask, out string error))
            {
                throw new ErrorException(error);
            }
            return upTask;
        }
    }
    /// <summary>
    /// 远程调用基类-返回指定类型的数据
    /// </summary>
    /// <typeparam name="Result"></typeparam>
    public class RpcRemote<Result>
    {
        public bool Send (out Result datas, out string error)
        {
            return RemoteCollect.Send(this, out datas, out error);
        }
        public Result Send ()
        {
            if (!RemoteCollect.Send(this, out Result data, out string error))
            {
                throw new ErrorException(error);
            }
            return data;
        }
        public Task<Result> AsyncSend ()
        {
            return Task.Run(() =>
            {
                if (!RemoteCollect.Send(this, out Result data, out string error))
                {
                    throw new ErrorException(error);
                }
                return data;
            });
        }
        public Result Send (long serverId)
        {
            if (!RemoteCollect.Send(serverId, this, out Result data, out string error))
            {
                throw new ErrorException(error);
            }
            return data;
        }
        public bool Send (long serverId, out Result data, out string error)
        {
            return RemoteCollect.Send(serverId, this, out data, out error);
        }
    }
}
