using System.IO;
using System.Threading.Tasks;

using RpcClient.Collect;

using SocketTcpClient.UpFile;

using RpcHelper;

namespace RpcClient
{
        public class RpcBroadcast
        {
                public void Send()
                {
                        RemoteCollect.BroadcastMsg(this);
                }
                public async void SyncSend()
                {
                        await new Task(this.Send);
                }
                public void Send(string dic)
                {
                        RemoteCollect.BroadcastMsg(dic, this);
                }
                public void Send(string[] sysType)
                {
                        RemoteCollect.BroadcastSysTypeMsg(this, sysType);
                }
                public void Send(string[] sysType, long rpcMerId)
                {
                        RemoteCollect.BroadcastMsg(this, rpcMerId, sysType);
                }

                public void Send(string dictate, string sysType)
                {
                        RemoteCollect.BroadcastMsg(dictate, this, sysType);
                }
                public void Send(string dictate, string[] sysType)
                {
                        RemoteCollect.BroadcastMsg(dictate, this, sysType);
                }
        }
        public class RpcRemote
        {
                public void SyncSend()
                {
                        Task.Run(() =>
                        {
                                if (!RemoteCollect.Send(this, out string error))
                                {
                                        new ErrorException(error).SaveLog("Rpc_Msg");
                                }
                        });
                }
                public void Send()
                {
                        if (!RemoteCollect.Send(this, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }
                public bool Send(out string error)
                {
                        return RemoteCollect.Send(this, out error);
                }
                public void Send(long serverId)
                {
                        if (!RemoteCollect.Send(serverId, this, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }
                public bool Send(long serverId, out string error)
                {
                        return RemoteCollect.Send(serverId, this, out error);
                }
                public bool Send(string sysType, out string error)
                {
                        return RemoteCollect.Send(sysType, this, out error);
                }
        }
        /// <summary>
        /// 远程分页类消息的封装
        /// </summary>
        /// <typeparam name="Result"></typeparam>
        public class RpcRemoteByPaging<Result>
        {
                public bool Send(out Result[] datas, out long count, out string error)
                {
                        return RemoteCollect.Send(this, out datas, out count, out error);
                }
                public Result[] Send(out long count)
                {
                        if (!RemoteCollect.Send(this, out Result[] datas, out count, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        return datas;
                }
        }
        public class RpcRemoteArray<Result>
        {
                public bool Send(out Result[] datas, out string error)
                {
                        return RemoteCollect.Send(this, out datas, out error);
                }
                public Result[] Send()
                {
                        if (!RemoteCollect.Send(this, out Result[] datas, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        return datas;
                }
        }
        public class RpcUpFile
        {
                public UpTask Send(string path, UpFileAsync func, UpProgress progress)
                {
                        if (!this.Send(new FileInfo(path), func, progress, out UpTask upTask, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        return upTask;
                }
                public bool Send(string path, UpFileAsync func, UpProgress progress, out UpTask upTask, out string error)
                {
                        return this.Send(new FileInfo(path), func, progress, out upTask, out error);
                }
                public bool Send(FileInfo file, UpFileAsync func, UpProgress progress, out UpTask upTask, out string error)
                {
                        return RemoteCollect.SendFile(this, file, func, progress, out upTask, out error);
                }
                public UpTask Send(FileInfo file, UpFileAsync func, UpProgress progress)
                {
                        if (!this.Send(file, func, progress, out UpTask upTask, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        return upTask;
                }
                public bool Send(string path, UpFileAsync func, out UpTask upTask, out string error)
                {
                        return this.Send(new FileInfo(path), func, null, out upTask, out error);
                }
                public bool Send(FileInfo file, UpFileAsync func, out UpTask upTask, out string error)
                {
                        return this.Send(file, func, null, out upTask, out error);
                }

                public UpTask Send(string path, UpFileAsync func)
                {
                        if (!this.Send(path, func, null, out UpTask upTask, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        return upTask;
                }
                public UpTask Send(FileInfo file, UpFileAsync func)
                {
                        if (!this.Send(file, func, null, out UpTask upTask, out string error))
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
                public bool Send(out Result datas, out string error)
                {
                        return RemoteCollect.Send(this, out datas, out error);
                }
                public Result Send()
                {
                        if (!RemoteCollect.Send(this, out Result data, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        return data;
                }
        }
}
