using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.HttpApiGateway.Interface;
namespace WeDonekRpc.HttpApiGateway.Idempotent
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public class KeyIdempotent : IKeyIdempotent
    {
        private readonly ITokenIdempotent _Service;

        public KeyIdempotent (IIdempotentConfig config, IIocService unity)
        {
            this._Service = unity.Resolve<ITokenIdempotent>(config.SaveType.ToString());
        }

        public StatusSaveType SaveType => this._Service.SaveType;


        public void Dispose ()
        {
            this._Service.Dispose();
        }

        public bool SubmitToken (string tokenId)
        {
            return this._Service.SubmitToken(tokenId);
        }
    }
}
