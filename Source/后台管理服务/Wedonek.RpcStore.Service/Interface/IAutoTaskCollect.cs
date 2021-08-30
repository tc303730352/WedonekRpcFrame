using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IAutoTaskCollect
        {
                long AddAutoTask(AutoTask task);
                void DropAutoTask(long id);
                AutoTaskList GetAutoTask(long id);
                AutoTaskDatum[] QueryTask(QueryTaskParam query, int index, int size, out long count);
                void SetAutoTask(AutoTaskSetParam param);
        }
}