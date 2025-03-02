using System.Collections.Generic;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Model;

namespace WeDonekRpc.Model.CurConfig
{
    [IRemoteConfig("SetCurConfig", "sys.sync", isReply: false, IsProhibitTrace = true)]
    public class SetCurConfig
    {
        /// <summary>
        /// 配置项
        /// </summary>
        public Dictionary<string, ConfigItemModel> Config;
    }
}
