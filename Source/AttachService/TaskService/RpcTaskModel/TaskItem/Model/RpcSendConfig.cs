using WeDonekRpc.Helper.Validate;

namespace RpcTaskModel.TaskItem.Model
{
    /// <summary>
    /// 发送配置
    /// </summary>
    public class RpcSendConfig
    {
        /// <summary>
        /// 指令
        /// </summary>
        [NullValidate("task.item.rpc.dictate.null")]
        public string SysDictate
        {
            get;
            set;
        }
        public RpcRemoteSet RemoteSet
        {
            get;
            set;
        }
    }
}
