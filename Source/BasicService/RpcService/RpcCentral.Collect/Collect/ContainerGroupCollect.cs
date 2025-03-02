using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class ContainerGroupCollect : IContainerGroupCollect
    {
        private readonly IContainerGroupDAL _Service;
        private readonly ICacheController _Cache;
        public ContainerGroupCollect (IContainerGroupDAL service, ICacheController cache)
        {
            this._Cache = cache;
            this._Service = service;
        }

        public BasicContainerGroup GroupLogin (string mac, string localIp)
        {
            BasicContainerGroup group = this._Get(mac);
            if (group.HostIp != localIp)
            {
                this._Service.SetLocalIp(group.Id, localIp);
                string key = string.Concat("ContGroup_", mac);
                _ = this._Cache.Set(key, group);
            }
            return group;
        }
        private BasicContainerGroup _Get (string mac)
        {
            string key = string.Concat("ContGroup_", mac);
            if (!this._Cache.TryGet(key, out BasicContainerGroup group))
            {
                group = this._Service.Find(mac);
                if (group == null)
                {
                    group = new BasicContainerGroup
                    {
                        Id = 0
                    };
                    _ = this._Cache.Set(key, group, new TimeSpan(0, 0, 30));
                }
                else
                {
                    _ = this._Cache.Set(key, group);
                }
            }
            if (group.Id == 0)
            {
                throw new ErrorException("rpc.container.group.not.find");
            }
            return group;
        }
    }
}
