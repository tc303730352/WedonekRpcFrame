using RpcManageClient;

using RpcModel;
using RpcModel.Model;
using RpcHelper;
using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class ServerDictateLimitCollect : BasicCollect<ServerDictateLimitDAL>, IServerDictateLimitCollect
        {
                private IRpcServerCollect _RpcServer => this.GetCollect<IRpcServerCollect>();
                public long AddDictateLimit(AddDictateLimit add)
                {
                        if (this.BasicDAL.AddDictateLimit(add, out long id))
                        {
                                this._RpcServer.RefreshDictateLimit(add.ServerId, add.Dictate);
                                return id;
                        }
                        throw new ErrorException("rpc.dictate.add.error");
                }
                public ServerDictateLimitData GetDictateLimit(long id)
                {
                        if (!this.BasicDAL.GetDictateLimit(id, out ServerDictateLimitData limit))
                        {
                                throw new ErrorException("rpc.dictate.get.error");
                        }
                        else if (limit == null)
                        {
                                throw new ErrorException("rpc.dictate.not.find");
                        }
                        return limit;
                }
                public void DropDictateLimit(long id)
                {
                        ServerDictateLimitData limit = this.GetDictateLimit(id);
                        if (this.BasicDAL.DropDictateLimit(limit.Id))
                        {
                                this._RpcServer.RefreshDictateLimit(limit.ServerId, limit.Dictate);
                                return;
                        }
                        throw new ErrorException("rpc.dictate.drop.error");
                }
                public void SetDictateLimit(long id, ServerDictateLimit limit)
                {
                        ServerDictateLimitData data = this.GetDictateLimit(id);
                        if (this.BasicDAL.SetDictateLimit(id, limit))
                        {
                                this._RpcServer.RefreshDictateLimit(data.ServerId, limit.Dictate);
                                return;
                        }
                        throw new ErrorException("rpc.dictate.set.error");
                }
                public ServerDictateLimitData[] Query(long serverId, string dictate, IBasicPage paging, out long count)
                {
                        if (this.BasicDAL.Query(serverId, dictate, paging, out ServerDictateLimitData[] limits, out count))
                        {
                                return limits;
                        }
                        throw new ErrorException("rpc.dictate.query.error");
                }
        }
}
