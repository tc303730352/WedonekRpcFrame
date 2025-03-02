using WeDonekRpc.Model;

namespace WeDonekRpc.ExtendModel.RetryTask
{
    [IRemoteConfig("CloseTask", "sys.extend", IsProhibitTrace = true)]
    public class CloseTask
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        public long TaskId { get; set; }
    }
}
