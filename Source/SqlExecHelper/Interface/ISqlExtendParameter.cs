using System.Data;

namespace SqlExecHelper
{
        public interface ISqlExtendParameter
        {
                ParameterDirection Direction { get; set; }
                SqlDbType DbType { get; }
                int Size { get; }
                object Value { get; set; }
                string ParamName { get; }
                void InitParam(ISqlRunConfig config);
                IDataParameter GetParameter();

        }
}