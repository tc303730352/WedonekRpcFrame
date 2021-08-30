using HttpApiGateway.Model;

using RpcModel.Model;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{

        internal class ServerDictateLimitService : IServerDictateLimitService
        {
                private readonly IServerDictateLimitCollect _Dictate = null;


                public ServerDictateLimitService(IServerDictateLimitCollect server)
                {
                        this._Dictate = server;
                }

                public long Add(AddDictateLimit add)
                {
                        return this._Dictate.AddDictateLimit(add);
                }

                public void Drop(long id)
                {
                        this._Dictate.DropDictateLimit(id);
                }
                public ServerDictateLimitData Get(long id)
                {
                        return this._Dictate.GetDictateLimit(id);
                }
                public ServerDictateLimitData[] Query(PagingParam<DictateQueryParam> param, out long count)
                {
                        return this._Dictate.Query(param.Param.ServerId, param.Param.Dictate, param.ToBasicPaging(), out count);
                }

                public void Set(long id, ServerDictateLimit limit)
                {
                        this._Dictate.SetDictateLimit(id, limit);
                }
        }
}
