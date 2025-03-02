using RpcStore.RemoteModel.DictateNode;
using RpcStore.RemoteModel.DictateNode.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class DictateNodeService : IDictateNodeService
    {
        public long AddDictateNode(DictateNodeAdd datum)
        {
            return new AddDictateNode
            {
                Datum = datum,
            }.Send();
        }

        public void DeleteDictateNode(long id)
        {
            new DeleteDictateNode
            {
                Id = id,
            }.Send();
        }

        public DictateNodeData[] GetAllDictateNode()
        {
            return new GetAllDictateNode().Send();
        }

        public DictateNodeData GetDictateNode(long id)
        {
            return new GetDictateNode
            {
                Id = id,
            }.Send();
        }

        public DictateNodeData[] GetDictateNodes(long? parentId)
        {
            return new GetDictateNodes
            {
                ParentId = parentId,
            }.Send();
        }

        public TreeDictateNode[] GetDictateNodeTree()
        {
            return new GetDictateNodeTree().Send();
        }

        public void SetDictateName(long id, string name)
        {
            new SetDictateName
            {
                Id = id,
                Name = name,
            }.Send();
        }

    }
}
