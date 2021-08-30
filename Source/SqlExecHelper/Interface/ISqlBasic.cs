using System.Data;
using System.Text;

namespace SqlExecHelper
{
        public interface ISqlBasic
        {

                StringBuilder GenerateSql(out IDataParameter[] param);
        }
}