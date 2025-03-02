using System;
using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.Model
{
    /// <summary>
    /// 服务器广播配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class IRemoteBroadcast : Attribute
    {
        /// <summary>
        /// 远程方法
        /// </summary>
        public string SysDictate { get; set; }
        /// <summary>
        /// 服务器组
        /// </summary>
        public string[] TypeVal { get; set; }
        /// <summary>
        /// 是否选择唯一的服务器进行广播
        /// </summary>
        public bool IsOnly { get; set; } = true;

        /// <summary>
        /// 是否禁止链路
        /// </summary>
        public bool IsProhibitTrace { get; set; }
        /// <summary>
        /// 服务器列表
        /// </summary>
        public long[] ServerId { get; set; }

        /// <summary>
        /// 远端通讯配置
        /// </summary>
        public RemoteSet RemoteConfig
        {
            get;
            set;
        }
        /// <summary>
        /// 广播方式
        /// </summary>
        public BroadcastType BroadcastType
        {
            get;
            set;
        }
        public IRemoteBroadcast (string dictate, long rpcMerId, int regionId) : this(dictate)
        {
            this.RpcMerId = rpcMerId;
            this.RegionId = regionId;
        }
        public IRemoteBroadcast (string dictate, long rpcMerId) : this(dictate)
        {
            this.RpcMerId = rpcMerId;
        }
        public IRemoteBroadcast (string dictate, int regionId) : this(dictate)
        {
            this.RegionId = regionId;
        }
        public IRemoteBroadcast (string dictate, bool isOnly, params string[] typeVal) : this(dictate, isOnly)
        {
            this.TypeVal = typeVal;
        }
        public IRemoteBroadcast (string dictate, IRemoteBroadcast config, params string[] typeVal) : this(dictate, config.IsOnly)
        {
            this.TypeVal = typeVal;
            this.RemoteConfig = config.RemoteConfig;
        }
        public IRemoteBroadcast (string dictate, bool isOnly) : this(dictate)
        {
            this.IsOnly = isOnly;
        }
        public IRemoteBroadcast (string dictate, bool isOnly, params long[] serverId) : this(dictate, isOnly)
        {
            this.ServerId = serverId;
        }
        public IRemoteBroadcast (bool isOnly, params string[] typeVal)
        {
            this.IsOnly = isOnly;
            this.TypeVal = typeVal;
        }

        public IRemoteBroadcast (params string[] typeVal)
        {
            this.TypeVal = typeVal;
        }
        public IRemoteBroadcast (string dictate)
        {
            this.SysDictate = dictate;
        }
        /// <summary>
        /// 是否排除来源
        /// </summary>
        public bool IsExclude
        {
            get;
            set;
        } = true;
        /// <summary>
        /// 是否验证数据格式
        /// </summary>
        public bool IsValidate
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否跨服务器组广播
        /// </summary>
        public bool IsCrossGroup { get; set; }

        /// <summary>
        /// 限定的服务器组
        /// </summary>
        public long? RpcMerId { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// 初始化广播配置
        /// </summary>
        /// <param name="type"></param>
        public void InitConfig (Type type, int retryNum)
        {
            if (string.IsNullOrEmpty(this.SysDictate))
            {
                this.SysDictate = type.Name;
            }
            this.IsValidate = DataValidateHepler.CheckIsValidate(type);
            this.RemoteConfig?.InitConfig(type, retryNum);
        }
        public void InitConfig (int retryNum)
        {
            this.RemoteConfig?.InitConfig(retryNum);
        }
    }
}
