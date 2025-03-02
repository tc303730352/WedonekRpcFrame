using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using System.Collections.Generic;

namespace RpcTaskModel.TaskItem.Model
{
    public class RpcParamConfig
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
        /// <summary>
        /// 服务器类型
        /// </summary>
        [EntrustValidate("_Check")]
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// 消息体
        /// </summary>
        public Dictionary<string, object> MsgBody { get; set; }

        private  bool _Check(RpcParamConfig config,out string error)
        {
            if(config.ServerId<=0 && config.SystemType.IsNull())
            {
                error = "task.item.rpc.target.null";
                return false;
            }
            error = null;
            return true;
        }

    }
}
