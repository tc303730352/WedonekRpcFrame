using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ServerGroup;
using RpcStore.RemoteModel.ServerGroup.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class ServerGroupEvent : IRpcApiService
    {
        private readonly IServerGroupService _Service;
        public ServerGroupEvent (IServerGroupService service)
        {
            this._Service = service;
        }

        public long AddServerGroup (AddServerGroup add)
        {
            return this._Service.AddGroup(add.Group);
        }
        public ServerGroupList[] GetServerGroupList (GetServerGroupList obj)
        {
            return this._Service.GetList(obj.ServerType);
        }
        public void CheckGroupTypeVal (CheckGroupTypeVal obj)
        {
            this._Service.CheckIsRepeat(obj.TypeVal);
        }

        public void DeleteServerGroup (DeleteServerGroup obj)
        {
            this._Service.Delete(obj.Id);
        }

        public ServerGroupDatum GetServerGroup (GetServerGroup obj)
        {
            return this._Service.GetGroup(obj.Id);
        }

        public ServerGroupItem[] GetAllServerGroup ()
        {
            return this._Service.GetGroups();
        }


        public void SetServerGroup (SetServerGroup obj)
        {
            this._Service.SetGroup(obj.Id, obj.Name);
        }
    }
}
