using System.Data;
using System.Data.SqlClient;

namespace SqlExecHelper
{
        public delegate void BeginTranEvent(IDAL dal);
        public delegate void EndTranEvent(IDAL dal);
        public interface IDAL : System.IDisposable
        {
                string ConName { get; }
                IDAL BeginTrans();

                event BeginTranEvent TranBegin;

                event EndTranEvent TranEnd;

                void CommitTrans();
                bool BackUpDatabase(string name, string path, string show);
                int ExecuteNonQuery(string sql, params IDataParameter[] param);
                bool ExecuteReader(string sql, out SqlDataReader reader, params IDataParameter[] parameter);
                bool ExecuteScalar(string sql, out object res, params IDataParameter[] parameter);
                bool GetDataRow(string sql, out DataRow row, params IDataParameter[] param);
                bool GetDataSet(string sql, out DataSet dataSet, params IDataParameter[] parameter);
                bool GetDataTable(string sql, out DataTable table, params IDataParameter[] parameter);
                bool InsertTable(DataTable table, SqlBulkCopyOptions options = SqlBulkCopyOptions.Default);
                void RollbackTrans();
                void DropTable(string tableName);
        }
}