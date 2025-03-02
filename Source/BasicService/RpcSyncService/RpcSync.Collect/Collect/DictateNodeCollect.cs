using RpcSync.DAL;
using RpcSync.Model;

namespace RpcSync.Collect.Collect
{
    internal class DictateNodeCollect : IDictateNodeCollect
    {
        private IDictateNodeDAL _DictateNodeDAL;

        public DictateNodeCollect(IDictateNodeDAL nodeDAL)
        {
            _DictateNodeDAL = nodeDAL;
        }

        public DictateNode[] GetDictateNode()
        {
            return _DictateNodeDAL.GetDictateNode();
        }
    }
}
