using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.DictateNode.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class DictateNodeService : IDictateNodeService
    {
        private readonly IDictateNodeCollect _Dictate;

        public DictateNodeService (IDictateNodeCollect dictate)
        {
            this._Dictate = dictate;
        }
        public long Add (DictateNodeAdd add)
        {
            if (add.ParentId != 0)
            {
                DictateNodeModel node = this._Dictate.Get(add.ParentId);
                if (node.IsEndpoint)
                {
                    throw new ErrorException("rpc.store.dictate.node.no.allow.add");
                }
            }
            return this._Dictate.Add(add);
        }
        public TreeDictateNode[] GetTrees ()
        {
            DictateNodeModel[] nodes = this._Dictate.Gets();
            return nodes.Convert(a => a.ParentId == 0, a => new TreeDictateNode
            {
                Id = a.Id,
                Name = a.DictateName,
                Children = this._GetTrees(a, nodes)
            });
        }
        private TreeDictateNode[] _GetTrees (DictateNodeModel node, DictateNodeModel[] list)
        {
            return list.Convert(a => a.ParentId == node.Id, a => new TreeDictateNode
            {
                Id = a.Id,
                Name = a.DictateName,
                Children = this._GetTrees(a, list)
            });
        }
        public void Delete (long id)
        {
            DictateNodeModel node = this._Dictate.Get(id);
            this._Dictate.Delete(node);
        }

        public DictateNodeData Get (long id)
        {
            DictateNodeModel node = this._Dictate.Get(id);
            return node.ConvertMap<DictateNodeModel, DictateNodeData>();
        }

        public DictateNodeData[] GetAllDictateNode ()
        {
            DictateNodeModel[] list = this._Dictate.Gets();
            return list.ConvertMap<DictateNodeModel, DictateNodeData>();
        }

        public DictateNodeData[] GetDictateNodes (long? parentId)
        {
            DictateNodeModel[] list;
            if (parentId.HasValue)
            {
                list = this._Dictate.Gets(parentId.Value);
            }
            else
            {
                list = this._Dictate.Gets();
            }
            if (list.IsNull())
            {
                return new DictateNodeData[0];
            }
            return list.ConvertMap<DictateNodeModel, DictateNodeData>();
        }

        public void Set (long id, string name)
        {
            DictateNodeModel node = this._Dictate.Get(id);
            this._Dictate.Set(node, name);
        }
    }
}
