using System.Collections.Generic;

using SqlExecHelper;
using SqlExecHelper.SetColumn;

using RpcHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class AutoTaskDAL : SqlBasicClass
        {
                public AutoTaskDAL() : base("AutoTaskList")
                {

                }

                public bool AddAutoTask(AutoTask task, out long id)
                {
                        return this.Insert(task, out id);
                }
                public bool DropAutoTask(long id)
                {
                        return this.Drop("Id", id) != -2;
                }
                public bool GetAutoTask(long id, out AutoTaskList task)
                {
                        return this.GetRow("Id", id, out task);
                }
                public bool SetAutoTask(AutoTaskSetParam param)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("TaskName", System.Data.SqlDbType.NVarChar,50){ Value=param.TaskName},
                                new SqlSetColumn("TaskType", System.Data.SqlDbType.SmallInt){ Value=param.TaskType},
                                new SqlSetColumn("TaskTimeSpan", System.Data.SqlDbType.Int){ Value=param.TaskTimeSpan},
                                new SqlSetColumn("TaskPriority", System.Data.SqlDbType.Int){ Value=param.TaskPriority},
                                new SqlSetColumn("SendType", System.Data.SqlDbType.SmallInt){ Value=param.SendType},
                                new SqlSetColumn("SendParam", System.Data.SqlDbType.NText){ Value=param.SendParam},
                                new SqlSetColumn("VerNum", System.Data.SqlDbType.Int, SqlSetType.递加){ Value=1}
                        }, "Id", param.Id);
                }
                public bool QueryTask(QueryTaskParam query, int index, int size, out AutoTaskDatum[] tasks, out long count)
                {
                        List<ISqlWhere> wheres = new List<ISqlWhere>();
                        if (!query.TaskName.IsNull())
                        {
                                wheres.Add(new LikeSqlWhere("TaskName", 50) { Value = query.TaskName });
                        }
                        if (query.RpcMerId.HasValue)
                        {
                                wheres.Add(new SqlWhere("RpcMerId", System.Data.SqlDbType.BigInt) { Value = query.RpcMerId.Value });
                        }
                        if (query.TaskType.HasValue)
                        {
                                wheres.Add(new SqlWhere("TaskType", System.Data.SqlDbType.SmallInt) { Value = query.TaskType.Value });
                        }
                        return this.Query("Id desc", index, size, out tasks, out count, wheres.ToArray());
                }
        }
}
