namespace SqlExecHelper
{
        public interface IBatchUpdate : IBatchSql, System.IDisposable
        {
                bool Update(ISqlSetColumn[] columns, out int rowNum, params ISqlWhere[] where);
                bool Update(ISqlSetColumn[] columns, params ISqlWhere[] where);
                bool Update<Result>(string column, SqlEventPrefix prefix, out Result[] results, ISqlSetColumn[] columns, params ISqlWhere[] where);
                bool Update(params ISqlWhere[] where);
                bool Update(out int rowNum, params ISqlWhere[] where);
                bool Update<T>(out T[] datas, params ISqlWhere[] where);
                bool Update<T>(out T[] datas, SqlEventPrefix prefix, params ISqlWhere[] where);
                bool Update<T>(string column, out T[] datas, params ISqlWhere[] where);
                bool Update<T>(string column, SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where);
        }
}