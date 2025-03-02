using System.Text.RegularExpressions;
using RpcStore.RemoteModel.CurConfig.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点当前配置信息
    /// </summary>
    internal class ServerCurConfigApi : ApiController
    {
        private readonly IServerCurConfigService _Service;
        private static readonly Regex _Regex = new Regex(@"Initial Catalog=(.+){1};User ID=(.+){1};Password=(.+){1};");
        public ServerCurConfigApi (IServerCurConfigService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取服务节点的配置信息
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public CurConfigModel Get ([NumValidate("rpc.store.server.id.null", 1)] long serverId)
        {
            CurConfigModel datum = this._Service.Get(serverId);
            if (!base.UserState.CheckPrower("rpc.store.admin"))
            {
                this._HideConfig(datum.Configs);
            }
            return datum;
        }
        private void _HideConfig (ServerCurConfig[] configs)
        {
            configs.ForEach(c =>
            {
                if (c.Value.IsNotNull())
                {
                    c.Value = _Regex.Replace(c.Value, string.Empty);
                }
                if (!c.Children.IsNull())
                {
                    this._HideConfig(c.Children);
                }
            });
        }
    }
}
