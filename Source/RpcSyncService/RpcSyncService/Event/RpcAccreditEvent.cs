using RpcModel;

using RpcModularModel.Accredit;
using RpcModularModel.Accredit.Model;

using RpcSyncService.Collect;

namespace RpcSyncService.Event
{
        /// <summary>
        /// 登陆授权服务
        /// </summary>
        internal class RpcAccreditEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 申请授权
                /// </summary>
                /// <param name="apply"></param>
                /// <param name="source"></param>
                /// <returns></returns>
                public ApplyAccreditRes ApplyAccredit(ApplyAccredit apply, MsgSource source)
                {
                        return AccreditCollect.ApplyAccredit(apply, source);
                }
                /// <summary>
                /// 设置授权状态
                /// </summary>
                /// <param name="obj">参数</param>
                /// <param name="source"></param>
                /// <returns></returns>
                public static SetUserStateRes SetUserState(SetUserState obj, MsgSource source)
                {
                        return AccreditCollect.SetUserState(obj, source);
                }
                /// <summary>
                /// 设置授权
                /// </summary>
                /// <param name="obj"></param>
                /// <param name="source"></param>
                /// <returns></returns>
                public static ApplyAccreditRes SetAccredit(SetAccredit obj, MsgSource source)
                {
                        return AccreditCollect.ToUpdateAccredit(obj, source);
                }
                /// <summary>
                /// 踢出授权
                /// </summary>
                /// <param name="obj"></param>
                /// <param name="source"></param>
                public static void KickOutAccredit(KickOutAccredit obj, MsgSource source)
                {
                        AccreditCollect.KickOutAccredit(obj.CheckKey, source);
                }
                /// <summary>
                /// 获取授权信息
                /// </summary>
                /// <param name="obj"></param>
                /// <param name="source"></param>
                /// <returns></returns>
                public static AccreditDatum GetAccredit(GetAccredit obj, MsgSource source)
                {
                        return AccreditCollect.GetAccredit(obj.AccreditId, source);
                }
                /// <summary>
                /// 检查授权
                /// </summary>
                /// <param name="obj"></param>
                /// <param name="error"></param>
                /// <returns></returns>
                public static bool CheckAccredit(CheckAccredit obj, out string error)
                {
                        return Collect.AccreditCollect.CheckAccredit(obj, out error);
                }
                /// <summary>
                /// 取消授权
                /// </summary>
                /// <param name="cancel"></param>
                /// <param name="source"></param>
                public static void CancelAccredit(CancelAccredit cancel, MsgSource source)
                {
                        Collect.AccreditCollect.CancelAccredit(cancel.AccreditId, cancel.CheckKey, source);
                }
        }
}
