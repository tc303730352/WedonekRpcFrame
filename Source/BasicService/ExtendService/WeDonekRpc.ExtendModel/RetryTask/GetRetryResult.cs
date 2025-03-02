using WeDonekRpc.Model;

namespace WeDonekRpc.ExtendModel.RetryTask
{
    [IRemoteConfig("GetRetryResult", "sys.extend", IsProhibitTrace = true)]
    public class GetRetryResult
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        public string IdentityId { get; set; }
    }
}
