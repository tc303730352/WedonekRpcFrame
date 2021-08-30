using System.Data;
using System.Text;

namespace SqlExecHelper
{
        public interface IBatchQuery : IBatchSql, System.IDisposable
        {
                bool GroupByOne<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where);
                bool GroupByOne<T>(string group, out T[] datas, params ISqlWhere[] where);
                bool ExecuteScalar<T>(string column, SqlFuncType funcType, out T value, params ISqlWhere[] where);
                bool ExecuteScalar<T>(string column, string funcType, out T value, params ISqlWhere[] where);
                StringBuilder GenerateSql(out IDataParameter[] param);
                bool Group<T>(string group, ISqlWhere[] having, out T data, params ISqlWhere[] where);
                bool Group<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where);
                bool Group<T>(string group, out T data, params ISqlWhere[] where);
                bool Group<T>(string group, out T[] datas, params ISqlWhere[] where);
                bool Group<T>(string group, string orderBy, ISqlWhere[] having, out T[] data, params ISqlWhere[] where);
                bool Group<T>(string group, string sortBy, out T[] datas, params ISqlWhere[] where);
                bool Group<T>(string[] groups, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where);
                bool Group<T>(string[] group, out T data, params ISqlWhere[] where);
                bool Group<T>(string[] group, out T[] datas, params ISqlWhere[] where);
                bool Group<T>(string[] groups, string orderBy, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where);
                bool Group<T>(string[] group, string sortBy, out T[] datas, params ISqlWhere[] where);
                bool Query<T>(out T[] datas, params ISqlWhere[] where);
                bool Query<T>(string column, out T[] datas, params ISqlWhere[] where);
                bool QueryByPaging<T>(string orderBy, int index, int size, out T[] datas, out long count, params ISqlWhere[] where);
                bool QueryByPaging<T>(string orderBy, int index, int size, out T[] datas, params ISqlWhere[] where);
                bool QueryBySort<T>(string orderBy, out T[] datas, params ISqlWhere[] where);
                bool QueryByTop<T>(int topNum, out T[] datas, params ISqlWhere[] where);
                bool QueryByTop<T>(int topNum, string orderBy, out T[] datas, params ISqlWhere[] where);
        }
}