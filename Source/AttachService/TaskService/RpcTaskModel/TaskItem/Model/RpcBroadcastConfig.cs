using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using System.Collections.Generic;

namespace RpcTaskModel.TaskItem.Model
{
    /// <summary>
    /// 远程广播配置
    /// </summary>
    public class RpcBroadcastConfig
    {
        /// <summary>
        /// 广播指令
        /// </summary>
        [NullValidate("task.item.rpc.dictate.null")]
        public string SysDictate { get; set; }

        /// <summary>
        /// 发送配置
        /// </summary>
        public RpcRemoteSet RemoteSet
        {
            get;
            set;
        }
        /// <summary>
        /// 远程服务节点地址
        /// </summary>
        [EntrustValidate("_Check")]
        public string[] TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 节点列表
        /// </summary>
        public long[] ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否限定唯一
        /// </summary>
        public bool IsOnly
        {
            get;
            set;
        }
        /// <summary>
        /// 广播方式
        /// </summary>
        [EnumValidate("task.item.rpc.broadcastType.error",typeof(BroadcastType))]
        public BroadcastType BroadcastType
        {
            get;
            set;
        }
        /// <summary>
        /// 消息体
        /// </summary>
        public Dictionary<string, object> MsgBody { get; set; }

        private bool _Check(RpcBroadcastConfig config, out string error)
        {
            if (config.ServerId.IsNull() && config.TypeVal.IsNull())
            {
                error = "task.item.rpc.target.null";
                return false;
            }
            error = null;
            return true;
        }

    }
}
