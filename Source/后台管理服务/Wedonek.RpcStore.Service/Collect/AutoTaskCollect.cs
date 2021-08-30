using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;
using RpcHelper;
namespace Wedonek.RpcStore.Service.Collect
{
        internal class AutoTaskCollect : BasicCollect<AutoTaskDAL>, IAutoTaskCollect
        {
                public long AddAutoTask(AutoTask task)
                {
                        if (!this.BasicDAL.AddAutoTask(task, out long id))
                        {
                                throw new ErrorException("rpc.task.add.error");
                        }
                        return id;
                }
                public void DropAutoTask(long id)
                {
                        if (!this.BasicDAL.DropAutoTask(id))
                        {
                                throw new ErrorException("rpc.task.drop.error");
                        }
                }
                public AutoTaskList GetAutoTask(long id)
                {
                        if (!this.BasicDAL.GetAutoTask(id, out AutoTaskList task))
                        {
                                throw new ErrorException("rpc.task.get.error");
                        }
                        return task;
                }
                public void SetAutoTask(AutoTaskSetParam param)
                {
                        if (!this.BasicDAL.SetAutoTask(param))
                        {
                                throw new ErrorException("rpc.task.drop.error");
                        }
                }
                public AutoTaskDatum[] QueryTask(QueryTaskParam query, int index, int size, out long count)
                {
                        if (!this.BasicDAL.QueryTask(query, index, size, out AutoTaskDatum[] tasks, out count))
                        {
                                throw new ErrorException("rpc.task.query.error");
                        }
                        return tasks;
                }
        }
}
