using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.ExtendModel.RetryTask
{
    [IRemoteConfig("AddRetryTask", "sys.extend", IsProhibitTrace = true)]
    public class AddRetryTask
    {
        public RetryTaskAdd Task { get; set; }
    }
}
