using RpcHelper;

using RpcManageClient;

using RpcModel;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class RpcMerCollect : BasicCollect<RpcMerDAL>, IRpcMerCollect
        {
                private IRpcServerCollect _RpcServer => this.GetCollect<IRpcServerCollect>();
                private IRemoteGroupCollect _Group => this.GetCollect<IRemoteGroupCollect>();
                public RpcMer GetRpcMer(long id)
                {
                        if (!this.BasicDAL.GetMer(id, out RpcMer mer))
                        {
                                throw new ErrorException("rpc.mer.get.error");
                        }
                        else if (mer == null)
                        {
                                throw new ErrorException("rpc.mer.not.find");
                        }
                        return mer;
                }

                public void SetMer(long id, RpcMerSetParam datum)
                {
                        if (!this.BasicDAL.SetMer(id, datum))
                        {
                                throw new ErrorException("rpc.mer.set.error");
                        }
                        this._RpcServer.RefreshMer(id);
                }
                public void DropMer(long id)
                {
                        if (this._Group.CheckIsExists(id))
                        {
                                throw new ErrorException("rpc.mer.bind.server");
                        }
                        else if (!this.BasicDAL.DropMer(id))
                        {
                                throw new ErrorException("rpc.mer.drop.error");
                        }
                        this._RpcServer.RefreshMer(id);
                }
                public long AddMer(RpcMerDatum mer)
                {
                        if (!this.BasicDAL.AddMer(mer, out long id))
                        {
                                throw new ErrorException("rpc.mer.add.error");
                        }
                        return id;
                }
                public RpcMer[] Query(string name, IBasicPage paging, out long count)
                {
                        if (!this.BasicDAL.Query(name, paging, out RpcMer[] mers, out count))
                        {
                                throw new ErrorException("rpc.mer.query.error");
                        }
                        return mers;
                }
        }
}
