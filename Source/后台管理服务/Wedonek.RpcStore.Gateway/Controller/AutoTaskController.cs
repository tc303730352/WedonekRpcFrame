using HttpApiGateway.Model;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Model;
namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 任务管理
        /// </summary>
        internal class AutoTaskController : HttpApiGateway.ApiController
        {
                private readonly IAutoTaskService _AutoTask = null;

                public AutoTaskController(IAutoTaskService task)
                {
                        this._AutoTask = task;
                }
                /// <summary>
                /// 添加任务
                /// </summary>
                /// <param name="task">任务信息</param>
                /// <returns>任务Id</returns>
                public long Add(AutoTask task)
                {
                        return this._AutoTask.AddAutoTask(task);
                }
                /// <summary>
                /// 删除任务
                /// </summary>
                /// <param name="id">任务Id</param>
                public void Drop([NumValidate("rpc.task.id.error", 1)] long id)
                {
                        this._AutoTask.DropAutoTask(id);
                }
                /// <summary>
                /// 获取任务详细
                /// </summary>
                /// <param name="id">任务Id</param>
                /// <returns>任务详细</returns>
                public AutoTaskList Get([NumValidate("rpc.task.id.error", 1)] long id)
                {
                        return this._AutoTask.GetAutoTask(id);
                }
                /// <summary>
                /// 查询
                /// </summary>
                /// <param name="param">查询参数</param>
                /// <param name="count">任务总量</param>
                /// <returns>任务列表</returns>
                public AutoTaskDatum[] Query(PagingParam<QueryTaskParam> param, out long count)
                {
                        return this._AutoTask.QueryTask(param, out count);
                }
                /// <summary>
                /// 修改
                /// </summary>
                /// <param name="param">修改信息</param>
                public void Set(AutoTaskSetParam param)
                {
                        this._AutoTask.SetAutoTask(param);
                }
        }
}
