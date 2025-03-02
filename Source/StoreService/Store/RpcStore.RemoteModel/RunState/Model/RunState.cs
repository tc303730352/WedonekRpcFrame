using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.RunState.Model
{
    /// <summary>
    /// 运行状态
    /// </summary>
    public class RunState : ServerRunState
    {
      
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是容器
        /// </summary>
        public bool IsContainer
        {
            get;
            set;
        }
        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState ServiceState
        {
            get;
            set;
        }
    }
}
