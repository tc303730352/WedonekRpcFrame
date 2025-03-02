using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Shield;
using WeDonekRpc.ModularModel.Shield.Model;

namespace WeDonekRpc.Modular.Domain
{
    internal class ResourceShield : DataSyncClass
    {

        private readonly string _Path;
        private volatile bool _IsShieId = true;
        private readonly ShieldType _ShieIdType = ShieldType.接口;
        public ResourceShield (string path, ShieldType shieldType)
        {
            this._ShieIdType = shieldType;
            this._Path = path;
        }

        public bool IsShieId => this._IsShieId;
        private long _BeOverTime = 0;
        protected override void SyncData ()
        {
            this._Init();
        }
        private void _Init ()
        {
            ShieldDatum shield = new CheckIsShieId
            {
                Path = this._Path,
                ShieldType = this._ShieIdType,
            }.Send();
            this._IsShieId = shield.IsShieId;
            this._BeOverTime = shield.OverTime == 0 ? int.MaxValue : shield.OverTime;
        }
        public void CheckIsOverTime (long now)
        {
            if (this._IsShieId && this._BeOverTime <= now)
            {
                this._IsShieId = false;
            }
        }

        public override void ResetLock ()
        {
            this._Init();
        }
    }
}
