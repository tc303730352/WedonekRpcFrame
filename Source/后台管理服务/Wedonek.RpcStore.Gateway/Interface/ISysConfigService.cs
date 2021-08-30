using HttpApiGateway.Model;

using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface ISysConfigService
        {
                long Add(SysConfigAddParam add);
                void Drop(long id);
                SysConfigDatum Get(long id);
                SysConfigInfo[] Query(PagingParam<QuerySysParam> query, out long count);
                void Set(SysConfigSet config);
        }
}