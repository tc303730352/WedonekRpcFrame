using HttpApiGateway.Model;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IAutoTaskService
        {
                long AddAutoTask(AutoTask task);
                void DropAutoTask(long id);
                AutoTaskList GetAutoTask(long id);
                AutoTaskDatum[] QueryTask(PagingParam<QueryTaskParam> param, out long count);
                void SetAutoTask(AutoTaskSetParam param);
        }
}