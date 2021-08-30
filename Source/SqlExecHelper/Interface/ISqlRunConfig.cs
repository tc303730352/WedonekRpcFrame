using SqlExecHelper.Column;

namespace SqlExecHelper
{
        public interface ISqlRunConfig
        {
                string TableName { get; set; }

                /// <summary>
                /// 获取分配的参数名
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                string GetParamName();
                string FormatColumn(SqlColumn column);
                string FormatName(string columnName);
        }
}