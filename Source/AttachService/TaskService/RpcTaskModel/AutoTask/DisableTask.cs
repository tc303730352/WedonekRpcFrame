using WeDonekRpc.Client;

namespace RpcTaskModel.AutoTask
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class DisableTask : RpcRemote<bool>
    {
        public long TaskId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否强制结束任务
        /// </summary>
        public bool IsEndTask
        {
            get;
            set;
        }
    }
}
