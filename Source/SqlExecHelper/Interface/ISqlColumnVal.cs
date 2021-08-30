using System.Collections.Generic;
using System.Data;

namespace SqlExecHelper
{
        public interface ISqlColumnVal
        {
                object Value { get; set; }
                string GetValue(ISqlRunConfig config, List<IDataParameter> param);
        }
}