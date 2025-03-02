using RpcCentral.Collect.Model;
using RpcCentral.Common;
using WeDonekRpc.Helper;
namespace RpcCentral.Collect.Controller
{
    public class TransmitConfigController : DataSyncClass
    {
        private static readonly string _DefVer = "-1";
        public TransmitConfigController (long systemType, long rpcMerId)
        {
            this._RpcMerId = rpcMerId;
            this._SystemType = systemType;
        }
        private readonly long _RpcMerId;
        private readonly long _SystemType;

        private Transmit[] _Transmit;

        protected override void SyncData ()
        {
            using (IocScope scope = UnityHelper.CreateTempScore())
            {
                ITransmitHelper helper = scope.Resolve<ITransmitHelper>();
                this._Transmit = helper.GetTransmits(this._RpcMerId, this._SystemType);
            }
        }
        public bool TryGets (int ver, int curVer, ref string tranVer, out Transmit[] list)
        {
            list = this._Gets(ver, curVer);
            if (list.IsNull())
            {
                if (tranVer == _DefVer)
                {
                    return false;
                }
                tranVer = _DefVer;
                return true;
            }
            string newVer = list.Join('_', a => a.Key).GetMd5();
            if (tranVer == newVer)
            {
                return false;
            }
            tranVer = newVer;
            return true;
        }
        private Transmit[] _Gets (int ver, int curVer)
        {
            Transmit[] list = this._Transmit.FindAll(c => c.Ver == ver);
            if (!list.IsNull())
            {
                return list;
            }
            else if (ver == curVer)
            {
                return null;
            }
            return this._Transmit.FindAll(c => c.Ver == curVer);
        }
    }
}
