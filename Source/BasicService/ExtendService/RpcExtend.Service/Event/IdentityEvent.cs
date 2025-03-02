using WeDonekRpc.Client.Interface;
using WeDonekRpc.ModularModel.Identity;
using WeDonekRpc.ModularModel.Identity.Model;
using WeDonekRpc.ModularModel.Identity.Msg;

namespace RpcExtend.Service.Event
{
    /// <summary>
    /// 身份标识
    /// </summary>
    internal class IdentityEvent : IRpcApiService
    {
        private readonly Interface.IIdentityService _Service;

        public IdentityEvent (Interface.IIdentityService service)
        {
            this._Service = service;
        }
        public void RefreshIdentity (RefreshIdentity refresh)
        {
            this._Service.Refresh(refresh);
        }
        /// <summary>
        /// 获取身份标识
        /// </summary>
        /// <param name="obj">参数</param>
        /// <param name="source">事件原</param>
        /// <returns>身份标识</returns>
        public IdentityDatum GetIdentity (GetIdentity obj)
        {
            return this._Service.GetIdentity(obj.IdentityId);
        }
    }
}
