using RpcStore.DAL;
using WeDonekRpc.Helper;

namespace RpcStore.Collect.lmpl
{
    internal class ContainerCollect : IContainerCollect
    {
        private readonly IContainerDAL _BasicDAL;

        public ContainerCollect (IContainerDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public Dictionary<long, string> GetInternalAddr (long[] ids)
        {
            if (ids.IsNull())
            {
                return null;
            }
            return this._BasicDAL.GetInternalAddr(ids);
        }
        public void Clear (long id)
        {
            this._BasicDAL.Clear(id);
        }


        public string GetInternalAddr (long id)
        {
            return this._BasicDAL.GetInternalAddr(id);
        }
    }
}
