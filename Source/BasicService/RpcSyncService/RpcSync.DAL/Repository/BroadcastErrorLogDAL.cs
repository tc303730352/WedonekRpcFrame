using RpcSync.Model.DB;

namespace RpcSync.DAL.Repository
{
    internal class BroadcastErrorLogDAL : IBroadcastErrorLogDAL
    {
        private IRpcExtendResource<BroadcastErrorLogModel> _BasicDAL;
        public BroadcastErrorLogDAL(IRpcExtendResource<BroadcastErrorLogModel> dal)
        {
            _BasicDAL = dal;
        }
        public void AddErrorLog(BroadcastErrorLogModel[] logs)
        {
            this._BasicDAL.Insert(logs);
        }
    }
}
