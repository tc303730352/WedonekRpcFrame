using WeDonekRpc.Model;

namespace WeDonekRpc.ExtendModel.RetryTask
{
    [IRemoteConfig("CancelTask", "sys.extend", IsProhibitTrace = true)]
    public class CancelTask
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        public string IdentityId { get; set; }
    }
}
