using RpcHelper;

namespace RpcModular.Model
{
        /// <summary>
        /// 授权配置
        /// </summary>
        public class AccreditConfig
        {
                /// <summary>
                /// 授权心跳时间(秒)
                /// </summary>
                public int HeartbeatTime { get; private set; } = 600;

                /// <summary>
                /// 授权信息本地过期时间
                /// </summary>
                public int ErrorVaildTime { get; set; } = 10;
                /// <summary>
                /// 授权信息最小同步时间
                /// </summary>
                public int MinCheckTime { get; set; } = 5;

                /// <summary>
                /// 授权信息最晚同步时间
                /// </summary>
                public int MaxCheckime { get; set; } = 60;
                /// <summary>
                /// 授权信息本地最小缓存时间
                /// </summary>
                public int MinCacheTime { get; set; } = 180;
                /// <summary>
                /// 授权信息本地最大缓存时间
                /// </summary>
                public int MaxCacheTime { get; set; } = 300;

                public int GetNextCheckTime()
                {
                        return HeartbeatTimeHelper.HeartbeatTime + Tools.GetRandom(this.MinCheckTime, this.MaxCheckime);
                }
                public int GetCacheVaildTime()
                {
                        return HeartbeatTimeHelper.HeartbeatTime + Tools.GetRandom(this.MinCacheTime, this.MaxCacheTime);
                }
        }
}
