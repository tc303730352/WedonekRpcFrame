using RpcSync.Collect;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Modular.Model;
using WeDonekRpc.ModularModel.Accredit.Model;
using WeDonekRpc.ModularModel.Accredit.Msg;

namespace RpcSync.Service.Accredit
{
    internal static class AccreditHelper
    {
        private const string _TimeConfigName = "sync:accredit:AccreditTime";

        private const int _DefTime = 86400;
        public static void Refresh ( this AccreditToken token, IRemoteServerGroupCollect remoteGroup, bool isInvalid = true )
        {
            string[] sysType = remoteGroup.GetServerTypeVal(token.RpcMerId);
            new AccreditRefresh
            {
                AccreditId = token.AccreditId,
                IsInvalid = isInvalid
            }.Send(sysType, token.RpcMerId);
        }
        public static TimeSpan GetDefAccreditTime ()
        {
            int sec = RpcClient.Config.GetConfigVal<int>(_TimeConfigName, _DefTime);
            return new TimeSpan(0, 0, sec);
        }
        public static string Merge ( this UserState state, UserState other )
        {
            state.Prower = state.Prower.Add(other.Prower).Distinct();
            if ( other.Param.Count > 0 )
            {
                foreach ( KeyValuePair<string, StateParam> i in other.Param )
                {
                    if ( !state.Param.ContainsKey(i.Key) )
                    {
                        state.Param.Add(i.Key, i.Value);
                    }
                }
            }
            return state.ToJson();
        }
        public static TimeSpan? GetOverTime ( this AccreditToken token )
        {
            return GetAccreditTime(token.Expire);
        }
        public static TimeSpan? GetAccreditTime ( DateTime? expire )
        {
            if ( !expire.HasValue )
            {
                return AccreditHelper.GetDefAccreditTime();
            }
            DateTime now = DateTime.Now;
            if ( expire.Value <= now )
            {
                return null;
            }
            return expire.Value - now;
        }
    }
}
