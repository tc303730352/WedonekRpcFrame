using RpcStore.RemoteModel.SysEventConfig.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ISysEventConfigService
    {
        SystemEventConfig Get (int id);
        SystemEventItem[] GetItems ();
    }
}