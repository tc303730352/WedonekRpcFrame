using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Helper;

namespace RpcExtend.Collect
{
    public static class LinqHelper
    {
        public static long GetOneRetryTime (this RetryConfig config)
        {
            if (config.Begin.HasValue)
            {
                return config.Begin.Value;
            }
            return DateTime.Now.AddSeconds(config.Interval[0]).ToLong();
        }
        public static long GetRetryTime (this RetryConfig config, int retryNum)
        {
            if (retryNum >= config.Interval.Length)
            {
                retryNum = config.Interval.Length - 1;
            }
            return DateTime.Now.AddSeconds(config.Interval[retryNum]).ToLong();

        }
    }
}
