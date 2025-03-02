using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.DictateNode;
using RpcStore.RemoteModel.DictateNode.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class DictateNodeEvent : IRpcApiService
    {
        private readonly IDictateNodeService _Service;

        public DictateNodeEvent (IDictateNodeService service)
        {
            this._Service = service;
        }

        public long AddDictateNode (AddDictateNode add)
        {
            return this._Service.Add(add.Datum);
        }

        public void DeleteDictateNode (DeleteDictateNode obj)
        {
            this._Service.Delete(obj.Id);
        }

        public DictateNodeData GetDictateNode (GetDictateNode obj)
        {
            return this._Service.Get(obj.Id);
        }

        public DictateNodeData[] GetAllDictateNode ()
        {
            return this._Service.GetAllDictateNode();
        }

        public DictateNodeData[] GetDictateNodes (GetDictateNodes obj)
        {
            return this._Service.GetDictateNodes(obj.ParentId);
        }

        public TreeDictateNode[] GetDictateNodeTree ()
        {
            return this._Service.GetTrees();
        }

        public void SetDictateName (SetDictateName obj)
        {
            this._Service.Set(obj.Id, obj.Name);
        }
    }
}
