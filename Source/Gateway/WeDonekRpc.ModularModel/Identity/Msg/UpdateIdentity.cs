using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Identity.Msg
{
    /// <summary>
    /// 更新应用标识信息
    /// </summary>
    [IRemoteBroadcast("UpdateIdentity", IsCrossGroup = true, IsExclude = false, IsOnly = false)]
    public class UpdateIdentity : RpcBroadcast
    {
        public string AppId
        {
            get;
            set;
        }
    }
}
