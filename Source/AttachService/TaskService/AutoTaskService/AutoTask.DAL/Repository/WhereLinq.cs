using System.Linq.Expressions;
using AutoTask.Model.DB;
using LinqKit;
using WeDonekRpc.Helper;
using RpcTaskModel.AutoTask.Model;
using RpcTaskModel.TaskLog.Model;

namespace AutoTask.DAL.Repository
{
    internal static class WhereLinq
    {
        public static Expression<Func<AutoTaskLogModel, bool>> ToWhere (this TaskLogQueryParam query)
        {
            ExpressionStarter<AutoTaskLogModel> where = LinqKit.PredicateBuilder.New<AutoTaskLogModel>(a => a.TaskId == query.TaskId);
            if (query.ItemId.HasValue)
            {
                where = where.And(c => c.ItemId == query.ItemId.Value);
            }
            if (query.IsFail.HasValue)
            {
                where = where.And(c => c.IsFail == query.IsFail.Value);
            }
            if (query.Begin.HasValue)
            {
                where = where.And(c => c.BeginTime >= query.Begin.Value);
            }
            if (query.End.HasValue)
            {
                where = where.And(c => c.BeginTime <= query.End.Value);
            }
            return where;
        }
        public static Expression<Func<AutoTaskModel, bool>> ToWhere (this TaskQueryParam query)
        {
            ExpressionStarter<AutoTaskModel> where = PredicateBuilder.New<AutoTaskModel>(a => a.RpcMerId == query.RpcMerId);
            if (query.RegionId.HasValue)
            {
                where = where.And(c => c.RegionId == query.RegionId.Value);
            }
            if (!query.QueryKey.IsNull())
            {
                where = where.And(c => c.TaskName.Contains(query.QueryKey));
            }
            if (query.IsExec.HasValue)
            {
                where = where.And(c => c.IsExec == query.IsExec.Value);
            }
            if (!query.TaskStatus.IsNull())
            {
                where = where.And(c => query.TaskStatus.Contains(c.TaskStatus));
            }
            if (query.Begin.HasValue)
            {
                where = where.And(c => c.LastExecTime >= query.Begin.Value);
            }
            if (query.End.HasValue)
            {
                where = where.And(c => c.LastExecTime <= query.End.Value);
            }
            return where;
        }
    }
}
