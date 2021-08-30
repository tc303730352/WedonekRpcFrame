using System.Collections.Generic;

using RpcClient.Attr;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModular.Domain;

namespace RpcModular.Service
{
        [UnityName("IdentityService")]
        internal class RpcIdentityService : IExtendService
        {
                private readonly IIdentityService _Service = null;

                public RpcIdentityService(IIdentityService service)
                {
                        this._Service = service;
                }
                public string Name => "IdentityService";



                public void Load(IRpcService service)
                {
                        //RpcClient.RpcClient.Unity.RegisterType(typeof(IUserIdentity), typeof(Model.RpcUserIdentity));
                        service.ReceiveMsg += this.Service_ReceiveMsg;
                        service.SendMsg += this.Service_SendMsg;
                }



                private void Service_SendMsg(string dictate, ref Dictionary<string, string> extend)
                {
                        if (this._Service.IdentityId != null)
                        {
                                extend.Add("identityId", this._Service.IdentityId);
                        }
                }

                private void Service_ReceiveMsg(IMsg msg)
                {
                        if (msg.Extend != null && msg.Extend.TryGetValue("identityId", out string val))
                        {
                                this._Service.IdentityId = val;
                        }
                        if (this._Service.Check())
                        {
                                IdentityDomain identity = this._Service.GetIdentity();
                                identity.CheckRoute(msg.MsgKey);
                        }

                }
        }
}
