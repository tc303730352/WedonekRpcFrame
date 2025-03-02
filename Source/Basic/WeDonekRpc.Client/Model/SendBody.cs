using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Model
{
    public struct SendBody
    {
        public IRemoteConfig config;
        public string dicate;
        public TcpRemoteMsg msg;
        public object model;
        public IRemote remote;

        internal readonly bool CheckIsRetry (int retryNum, string error)
        {
            return this.config.IsAllowRetry && retryNum <= WebConfig.RpcConfig.MaxRetryNum;
        }
        internal readonly IRemoteResult Send (int retryNum)
        {
            this.msg.Retry = retryNum;
            if (this.remote.SendData(this.dicate, this.config, this.msg, out TcpRemoteReply reply, out string error))
            {
                return new IRemoteResult(this.remote.ServerId, reply, this.config.IsReply);
            }
            return new IRemoteResult(this.remote.ServerId, error);
        }
    }
}
