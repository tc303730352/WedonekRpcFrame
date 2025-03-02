using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Config;

namespace RpcStore.Collect.Config
{
    internal class DictConfig : IDictConfig
    {
        public DictConfig (ISysConfig config)
        {
            IConfigSection section = config.GetSection("Dict");
            section.AddRefreshEvent(this._Refresh);
        }

        private void _Refresh (IConfigSection section, string name)
        {
            this.ConfigItemShow = section.GetValue("ConfigItemShow", "001");
            this.LogGroup = section.GetValue("LogGroup", "002");
        }

        public string ConfigItemShow
        {
            get;
            private set;
        } = "001";

        public string LogGroup { get; private set; } = "002";
    }
}
