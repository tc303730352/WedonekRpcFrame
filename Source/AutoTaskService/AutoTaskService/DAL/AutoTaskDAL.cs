using AutoTaskService.Model;

using SqlExecHelper;

namespace AutoTaskService.DAL
{
        internal class AutoTaskDAL : SqlExecHelper.SqlBasicClass
        {
                public AutoTaskDAL() : base("AutoTaskList")
                {

                }
                public bool GetAllTask(out BasicTask[] tasks)
                {
                        return this.Get(out tasks, new ISqlWhere[] {
                                new SqlWhere("RegionId", System.Data.SqlDbType.Int){Value=RpcClient.RpcClient.CurrentSource.RegionId}
                        });
                }

                internal bool GetTask(long id, out RemoteTask task)
                {
                        return this.GetRow("Id", id, out task);
                }
        }
}
