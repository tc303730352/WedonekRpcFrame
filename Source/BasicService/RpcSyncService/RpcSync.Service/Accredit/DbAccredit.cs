using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace RpcSync.Service.Accredit
{
    [IocName("db")]
    internal class DbAccredit : IAccreditCollect
    {
        private readonly IIocService _Unity;
        private readonly IAccreditToken _Token;
        public DbAccredit (IIocService unity)
        {
            this._Token = this._Unity.Resolve<IAccreditToken>("db");
            this._Unity = unity;
        }

        public bool Get (string accreditId, out IAccreditToken token)
        {
            token = this._Token;
            return token.Init(accreditId);
        }
        public void Accredit (AccreditToken token)
        {
            this._Token.Create(token);
        }
    }
}
