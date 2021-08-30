namespace SqlExecHelper
{
        public interface IBatchMerge : IBatchSql, System.IDisposable
        {
                bool Insert();
                bool Insert<T>(out T[] datas);
                bool Insert<T>(string column, out T[] datas);
                bool InsertOrUpdate(params ISqlWhere[] where);
                bool InsertOrUpdate<T>(out T[] datas, params ISqlWhere[] where);
                bool Update(params ISqlWhere[] where);
                bool Update<T>(out T[] datas, params ISqlWhere[] where);

                bool InsertOrUpdate<T>(string column, SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where);
                bool InsertOrUpdate<T>(SqlEventPrefix prefix, out T[] datas, params ISqlWhere[] where);
        }
}