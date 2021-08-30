using HttpApiGateway.Model;

using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IErrorService
        {
                long AddError(ErrorAddDatum add);
                ErrorAddDatum GetError(long id, string lang);
                ErrorData[] QueryError(PagingParam<ErrorParam> query, out long count);
                void SetErrorMsg(SetErrorParam param);
        }
}