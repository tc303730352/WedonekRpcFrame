using WeDonekRpc.Model;

namespace WeDonekRpc.ExtendModel.RetryTask
{
    [IRemoteConfig("RestartTask", "sys.extend", IsReply = false, IsProhibitTrace = true)]
    public class RestartTask
    {
        public string IdentityId { get; set; }
    }
}
