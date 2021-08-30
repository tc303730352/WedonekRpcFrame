using SqlExecHelper.Column;
namespace SqlExecHelper.Config
{
        public class SqlBasicConfig : ISqlRunConfig
        {
                private readonly string _prefix = "@t_";
                public SqlBasicConfig(string table)
                {
                        this.TableName = table;
                }
                public SqlBasicConfig(string table, string prefix)
                {
                        this.TableName = table;
                        this._prefix = string.Concat("@", prefix, "_");
                }
                public string TableName
                {
                        get;
                        set;
                }

                private int _ParamIndex = 0;

                public string FormatColumn(SqlColumn column)
                {
                        return column.ToString();
                }

                public string GetParamName()
                {
                        return string.Concat(this._prefix, this._ParamIndex++);
                }

                public string FormatName(string name)
                {
                        return name;
                }
        }
}
