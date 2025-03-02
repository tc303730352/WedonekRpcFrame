using RpcSync.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Accredit;
using WeDonekRpc.ModularModel.Accredit.Model;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 登陆授权服务
    /// </summary>
    internal class RpcAccreditEvent : IRpcApiService
    {
        private readonly IAccreditServer _Server;


        public RpcAccreditEvent (IAccreditServer server)
        {
            this._Server = server;
        }
        /// <summary>
        /// 申请授权
        /// </summary>
        /// <param name="apply"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public ApplyAccreditRes ApplyAccredit (ApplyAccredit apply, MsgSource source)
        {
            return this._Server.ApplyAccredit(apply, source);
        }
        /// <summary>
        /// 设置授权状态
        /// </summary>
        /// <param name="obj">参数</param>
        /// <returns></returns>
        public SetUserStateRes SetUserState (SetUserState obj)
        {
            return this._Server.SetUserState(obj);
        }
        /// <summary>
        /// 设置授权
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ApplyAccreditRes SetAccredit (SetAccredit obj)
        {
            return this._Server.ToUpdate(obj);
        }
        /// <summary>
        /// 踢出授权
        /// </summary>
        /// <param name="obj"></param>
        public void KickOutAccredit (KickOutAccredit obj)
        {
            this._Server.KickOutAccredit(obj.CheckKey);
        }
        /// <summary>
        /// 获取授权信息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public AccreditDatum GetAccredit (GetAccredit obj, MsgSource source)
        {
            return this._Server.GetAccredit(obj.AccreditId, source);
        }
        /// <summary>
        /// 检查授权
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CheckAccredit (CheckAccredit obj)
        {
            return this._Server.CheckAccredit(obj);
        }
        /// <summary>
        /// 取消授权
        /// </summary>
        /// <param name="cancel"></param>
        public void CancelAccredit (CancelAccredit cancel)
        {
            this._Server.CancelAccredit(cancel.AccreditId, cancel.CheckKey);
        }
    }
}
