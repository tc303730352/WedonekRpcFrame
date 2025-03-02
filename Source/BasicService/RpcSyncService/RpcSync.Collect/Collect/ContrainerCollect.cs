using RpcSync.DAL;

namespace RpcSync.Collect.Collect
{
    internal class ContrainerCollect : IContrainerCollect
    {
        private readonly IContrainerDAL _BasicDAL;

        public ContrainerCollect (IContrainerDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public long[] GetIds (long groupId)
        {
            return this._BasicDAL.GetIds(groupId);
        }
    }
}
