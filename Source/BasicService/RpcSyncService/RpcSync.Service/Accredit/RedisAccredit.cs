using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace RpcSync.Service.Accredit
{
    [IocName("redis")]
    internal class RedisAccredit : IAccreditCollect
    {
        private readonly IAccreditToken _Token;

        public RedisAccredit (IIocService unity)
        {
            this._Token = unity.Resolve<IAccreditToken>("redis");
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
