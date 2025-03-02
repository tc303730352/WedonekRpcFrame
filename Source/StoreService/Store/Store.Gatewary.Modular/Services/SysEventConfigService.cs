using RpcStore.RemoteModel.SysEventConfig;
using RpcStore.RemoteModel.SysEventConfig.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class SysEventConfigService : ISysEventConfigService
    {
        public SystemEventConfig Get (int id)
        {
            return new GetSystemEvent
            {
                Id = id
            }.Send();
        }

        public SystemEventItem[] GetItems ()
        {
            return new GetSysEventItems().Send();
        }
    }
}
