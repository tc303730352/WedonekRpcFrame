using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class RemoteServerBindState
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Ip
        /// </summary>
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 远程Ip
        /// </summary>
        public string RemoteIp
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServerPort
        {
            get;
            set;
        }
        /// <summary>
        /// 服务组别Id
        /// </summary>
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类别Id
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Mac
        /// </summary>
        public string ServerMac
        {
            get;
            set;
        }
        /// <summary>
        /// 服务编号
        /// </summary>
        public int ServerIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 容器
        /// </summary>
        public long? ContainerId
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
        /// 是否在线
        /// </summary>
        public bool IsOnline
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        public int RegionId
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
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public string ApiVer
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public long VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 最后离线日期
        /// </summary>
        public DateTime LastOffliceDate
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
        /// <summary>
        /// 所属类别
        /// </summary>
        public string SystemName
        {
            get;
            set;
        }

        /// <summary>
        /// 组别名
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }
        /// <summary>
        /// 机房名
        /// </summary>
        public string RegionName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否绑定
        /// </summary>
        public bool IsBind
        {
            get;
            set;
        }
        /// <summary>
        /// 绑定Id
        /// </summary>
        public long BindId
        {
            get;
            set;
        }
    }
}
