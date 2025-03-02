using WeDonekRpc.Model;

namespace WeDonekRpc.ExtendModel.SysError
{
    [IRemoteConfig("SaveErrorLog", "sys.extend", isReply: false, IsProhibitTrace = true)]
    public class SaveErrorLog
    {
        public SysErrorLog[] Errors
        {
            get;
            set;
        }
    }
}
