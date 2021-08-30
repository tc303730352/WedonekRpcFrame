namespace SqlExecHelper
{
        public interface IBatchDrop : IBatchSql, System.IDisposable
        {
                int Drop(params ISqlWhere[] where);
                bool Drop<T>(out T[] datas, params ISqlWhere[] where);
                bool Drop<T>(string column, out T[] datas, params ISqlWhere[] where);
        }
}