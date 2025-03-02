using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Model
{
    public class BroadcastSet
    {
        public string Dictate
        {
            get;
            set;
        }
        public BroadcastType? BroadcastType
        {
            get;
            set;
        }
        /// <summary>
        /// 是否排除来源
        /// </summary>
        public bool? IsExclude
        {
            get;
            set;
        } = true;
       
        /// <summary>
        /// 是否跨服务器组广播
        /// </summary>
        public bool? IsCrossGroup { get; set; }
        public string[] SysType
        {
            get;
            set;
        }
        public long[] ServerId
        {
            get;
            set;
        }
        public bool? IsOnly
        {
            get;
            set;
        }
        public long? RpcMerId
        {
            get;
            set;
        }
        public int? RegionId
        {
            get;
            set;
        }
    }
}
