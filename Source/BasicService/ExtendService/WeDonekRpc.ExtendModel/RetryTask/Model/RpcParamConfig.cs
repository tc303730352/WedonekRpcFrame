using System.Collections.Generic;

namespace WeDonekRpc.ExtendModel.RetryTask.Model
{
    public class RpcParamConfig
    {
        /// <summary>
        /// 指令
        /// </summary>
        public string SysDictate
        {
            get;
            set;
        }
        /// <summary>
        /// 发送参数配置
        /// </summary>
        public RpcRemoteSet RemoteSet
        {
            get;
            set;
        }
        /// <summary>
        /// 服务器类型
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long? ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 集群ID
        /// </summary>
        public long? RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 消息体
        /// </summary>
        public Dictionary<string, object> MsgBody { get; set; }
        public int? RegionId { get; set; }
    }
}
