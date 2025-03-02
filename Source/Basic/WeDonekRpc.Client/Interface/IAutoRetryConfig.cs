using WeDonekRpc.ExtendModel.RetryTask.Model;

namespace WeDonekRpc.Client.Interface
{
    public interface IAutoRetryConfig
    {
        RetryConfig GetRetrySet (string key);
    }
}