using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.SysEventConfig.Model;

namespace RpcStore.DAL.Repository
{
    internal class SystemEventConfigDAL : ISystemEventConfigDAL
    {
        private readonly IRpcExtendResource<SystemEventConfigModel> _Resource;

        public SystemEventConfigDAL (IRpcExtendResource<SystemEventConfigModel> resource)
        {
            this._Resource = resource;
        }

        public SystemEventItem[] Gets ()
        {
            return this._Resource.Gets<SystemEventItem>(a => a.IsEnable);
        }
        public SystemEventConfig Get (int id)
        {
            SystemEventConfigModel config = this._Resource.Get(a => a.Id == id);
            if (config == null)
            {
                throw new ErrorException("rpc.store.system.event.null");
            }
            return config.ConvertMap<SystemEventConfigModel, SystemEventConfig>();
        }

        public string GetName (int id)
        {
            return this._Resource.Get(a => a.Id == id, a => a.EventName);
        }
        public Dictionary<int, string> GetName (int[] ids)
        {
            return this._Resource.Gets(a => ids.Contains(a.Id), a => new
            {
                a.Id,
                a.EventName
            }).ToDictionary(a => a.Id, a => a.EventName);
        }
    }
}
