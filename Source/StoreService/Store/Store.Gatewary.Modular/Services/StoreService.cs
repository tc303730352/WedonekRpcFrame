using System.Collections.Generic;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Modular;
using WeDonekRpc.Modular.Model;
using WeDonekRpc.ModularModel.Accredit.Model;

namespace Store.Gatewary.Modular.Services
{
    internal class StoreService : IStoreService
    {
        private readonly IConfigCollect _Config;
        private readonly IAccreditService _Accredit;
        public StoreService (IConfigCollect config, IAccreditService accredit)
        {
            this._Config = config;
            this._Accredit = accredit;
        }

        public LoginResult Login (StoreLogin login)
        {
            StoreUser user = this._Config.GetValue<StoreUser>(login.LoginName);
            if (user == null)
            {
                throw new ErrorException("rpc.store.loginName.not.find");
            }
            else if (user.Pwd != login.LoginPwd)
            {
                throw new ErrorException("rpc.store.loginPwd.error");
            }
            else
            {
                string appId = "RpcStore_" + login.LoginName;
                UserState state = new UserState()
                {
                    Prower = user.Prower,
                    Param = new Dictionary<string, StateParam>
                    {
                        {"LoginName",new StateParam(login.LoginName) },
                        {"Head",new StateParam(user.Head) }
                    }
                };
                string accreditId = this._Accredit.AddAccredit(appId, state);
                return new LoginResult
                {
                    AccreditId = accreditId,
                    UserName = login.LoginName,
                    UserHead = user.Head,
                    Introduction = user.Introduction,
                    Prower = user.Prower
                };
            }
        }

        public void LoginOut (string accreditId)
        {
            this._Accredit.CancelAccredit(accreditId);
        }
    }
}
