using WeDonekRpc.Client;
using WeDonekRpc.Helper.Config;

namespace RpcExtend.Service.Config
{
    internal class SysConfig
    {
        static SysConfig ()
        {
            IConfigSection section = RpcClient.Config.GetSection("error");
            section.AddRefreshEvent(_InitConfig);
        }
        private static void _InitConfig (IConfigSection config, string name)
        {
            ErrorConfig = config.GetValue<ErrorEmailConfig>(new ErrorEmailConfig());
        }
        public static ErrorEmailConfig ErrorConfig
        {
            get;
            private set;
        }
    }
}
