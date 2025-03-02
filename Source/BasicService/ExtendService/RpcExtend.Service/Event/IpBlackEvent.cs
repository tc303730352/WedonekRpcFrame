using RpcExtend.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.IpBlack;
using WeDonekRpc.ModularModel.IpBlack.Model;

namespace RpcExtend.Service.Event
{
    internal class IpBlackEvent : IRpcApiService
    {
        private readonly IIpBlackService _IpBack;

        public IpBlackEvent (IIpBlackService ipBack)
        {
            this._IpBack = ipBack;
        }


        /// <summary>
        /// 刷新Ip黑名单
        /// </summary>
        /// <param name="obj"></param>
        public void RefreshIpBlack (RefreshIpBlack obj)
        {
            this._IpBack.Refresh(obj.RpcMerId, obj.SystemType);
        }
        /// <summary>
        /// 同步IP黑名单
        /// </summary>
        /// <param name="sync">同步参数</param>
        /// <param name="source">来源</param>
        /// <returns></returns>
        public IpBlackList SyncIpBlack (SyncIpBlack sync, MsgSource source)
        {
            return this._IpBack.GetBlack(sync.LocalVer, source);
        }
    }
}
