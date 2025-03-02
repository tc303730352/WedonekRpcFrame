using AutoTask.Gateway.Interface;
using AutoTask.Gateway.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerRegion;
using RpcTaskModel.AutoTask;
using RpcTaskModel.AutoTask.Model;
namespace AutoTask.Gateway.Service
{
    internal class AutoTaskService : IAutoTaskService
    {
        public bool Set (long id, AutoTaskSet set)
        {
            return new SetTask
            {
                Id = id,
                Datum = set
            }.Send();
        }
        public PagingResult<AutoTaskDatum> Query (TaskQueryParam query, IBasicPage paging)
        {
            AutoTaskBasic[] list = new QueryTask
            {
                Index = paging.Index,
                Size = paging.Size,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc,
                NextId = paging.NextId,
                QueryParam = query,
            }.Send(out int count);
            if (list.IsNull())
            {
                return new PagingResult<AutoTaskDatum>(count);
            }
            AutoTaskDatum[] tasks = list.ConvertMap<AutoTaskBasic, AutoTaskDatum>();
            Dictionary<int, string> ranges = new GetRegionNames
            {
                Ids = list.Where(a => a.RegionId.HasValue).Select(a => a.RegionId.Value).Distinct().ToArray()
            }.Send();
            tasks.ForEach(c =>
            {
                if (c.RegionId.HasValue)
                {
                    c.RegionName = ranges.GetValueOrDefault(c.RegionId.Value);
                }
            });
            return new PagingResult<AutoTaskDatum>(tasks, count);
        }
        public AutoTaskInfo Get (long id)
        {
            return new GetTask
            {
                TaskId = id
            }.Send();
        }
        public long Add (AutoTaskAdd add)
        {
            return new AddTask
            {
                Datum = add
            }.Send();
        }
        public bool Enable (long id)
        {
            return new EnableTask
            {
                TaskId = id
            }.Send();
        }
        public bool Disable (long id, bool isEndTask)
        {
            return new DisableTask
            {
                TaskId = id,
                IsEndTask = isEndTask
            }.Send();
        }
        public void Delete (long id)
        {
            new DeleteTask
            {
                TaskId = id
            }.Send();
        }

        public AutoTaskData GetDatum (long id)
        {
            AutoTaskInfo task = new GetTask
            {
                TaskId = id
            }.Send();
            AutoTaskData dto = task.ConvertMap<AutoTaskInfo, AutoTaskData>();
            dto.RegionName = dto.RegionId.HasValue ? new GetRegionName
            {
                Id = task.RegionId.Value
            }.Send() : null;
            return dto;
        }
    }
}
