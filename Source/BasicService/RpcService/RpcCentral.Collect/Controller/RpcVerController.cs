using RpcCentral.Common;
using RpcCentral.DAL;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Controller
{
    public class RpcVerController : DataSyncClass
    {
        private readonly long _RpcMerId;
        private readonly int _VerNum;
        private readonly long _SystemTypeId;
        private long _VerId;
        private bool _IsEnable = false;
        private Dictionary<long, int> _VerRoute;

        public RpcVerController (long rpcMerId, long sysTypeId, int ver)
        {
            this._RpcMerId = rpcMerId;
            this._VerNum = ver;
            this._SystemTypeId = sysTypeId;
        }
        protected override void SyncData ()
        {
            using (IocScope scope = UnityHelper.CreateTempScore())
            {
                this._VerId = scope.Resolve<IServiceVerConfigDAL>().GetVerId(this._VerNum, this._SystemTypeId, this._RpcMerId);
                if (this._VerId == 0)
                {
                    this._IsEnable = false;
                    return;
                }
                this._VerRoute = scope.Resolve<IServiceVerRouteDAL>().GetVerRoute(this._VerId);
                this._IsEnable = true;
            }
        }

        public int GetVer (long toSysTypeId, int defVerNum)
        {
            if (!this._IsEnable)
            {
                return defVerNum;
            }
            return this._VerRoute.GetValueOrDefault(toSysTypeId, defVerNum);
        }

    }
}
