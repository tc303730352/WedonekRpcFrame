
using RpcClient;

namespace RpcModularModel.Accredit.Msg
{
        [RpcModel.IRemoteBroadcast("AccreditRefresh", false, IsCrossGroup = false, IsExclude = true)]
        public class AccreditRefresh : RpcBroadcast
        {
                public string AccreditId
                {
                        get;
                        set;
                }

                public bool IsInvalid
                {
                        get;
                        set;
                }
        }
}
