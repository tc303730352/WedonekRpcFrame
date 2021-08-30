using HttpApiGateway.Model;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class AutoTaskService : IAutoTaskService
        {
                private readonly IAutoTaskCollect _AutoTask = null;

                public AutoTaskService(IAutoTaskCollect task)
                {
                        this._AutoTask = task;
                }

                public long AddAutoTask(AutoTask task)
                {
                        return this._AutoTask.AddAutoTask(task);
                }

                public void DropAutoTask(long id)
                {
                        this._AutoTask.DropAutoTask(id);
                }

                public AutoTaskList GetAutoTask(long id)
                {
                        return this._AutoTask.GetAutoTask(id);
                }

                public AutoTaskDatum[] QueryTask(PagingParam<QueryTaskParam> param, out long count)
                {
                        return this._AutoTask.QueryTask(param.Param, param.Index, param.Size, out count);
                }

                public void SetAutoTask(AutoTaskSetParam param)
                {
                        this._AutoTask.SetAutoTask(param);
                }
        }
}
