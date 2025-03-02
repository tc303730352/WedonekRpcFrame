using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;

namespace WeDonekRpc.Modular.Service
{
    [IocName("IdentityService")]
    internal class RpcIdentityService : IRpcExtendService
    {
        private readonly IIdentityService _Service = null;

        public RpcIdentityService (IIdentityService service)
        {
            this._Service = service;
        }

        public void Load (IRpcService service)
        {
            service.ReceiveMsg += this.Service_ReceiveMsg;
            service.SendIng += this.Service_SendIng;
        }

        private void Service_SendIng (ref SendBody send, int sendNum)
        {
            if (this._Service.IdentityId != null)
            {
                send.config.SetAppIdentity(send.model, this._Service.IdentityId);
            }
        }



        private void Service_ReceiveMsg (IMsg msg)
        {
            if (msg.Extend != null && msg.Extend.TryGetValue("identityId", out string val))
            {
                this._Service.IdentityId = val;
            }
        }
    }
}
