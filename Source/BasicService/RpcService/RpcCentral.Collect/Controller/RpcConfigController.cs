using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcCentral.Collect.Controller
{
    public class RpcConfigController : DataSyncClass
    {
        public RpcConfigController (long rpcMerId, long sysTypeId)
        {
            this.RpcMerId = rpcMerId;
            this.SystemTypeId = sysTypeId;
        }

        public long RpcMerId
        {
            get;
        }

        public long SystemTypeId
        {
            get;
        }
        /// <summary>
        /// 是否启用区域隔离
        /// </summary>
        public bool IsRegionIsolate { get; private set; }
        /// <summary>
        /// 隔离级别
        /// </summary>
        public bool IsolateLevel { get; private set; }

        public BalancedType BalancedType { get; private set; }
        /// <summary>
        /// 当前正式版号
        /// </summary>
        public int CurrentVer { get; private set; }

        public int Ver { get; private set; }

        protected override void SyncData ()
        {
            using (IocScope scope = UnityHelper.CreateTempScore())
            {
                int? ver = scope.Resolve<IRpcMerServerVerDAL>().GetCurrentVer(this.RpcMerId, this.SystemTypeId);
                if (ver.HasValue)
                {
                    this.CurrentVer = ver.Value;
                }
                MerConfig config = scope.Resolve<IRpcMerConfigCollect>().GetConfig(this.RpcMerId, this.SystemTypeId);
                if (config == null)
                {
                    this.IsolateLevel = false;
                    this.IsRegionIsolate = true;
                    this.BalancedType = BalancedType.avg;
                }
                else
                {
                    this.BalancedType = config.BalancedType;
                    this.IsolateLevel = config.IsolateLevel;
                    this.IsRegionIsolate = config.IsRegionIsolate;
                }
                this.Ver = Tools.GetRandom(10000, 999999);
            }
        }

        public void RefreshVer (int verNum)
        {
            if (this.CurrentVer != verNum)
            {
                int? ver = UnityHelper.Resolve<IRpcMerServerVerDAL>().GetCurrentVer(this.RpcMerId, this.SystemTypeId);
                this.CurrentVer = ver.GetValueOrDefault();
            }
        }
    }
}
