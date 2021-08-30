using SqlExecHelper.Column;

namespace SqlExecHelper.Config
{
        internal class SqlBatchConfig : ISqlRunConfig
        {
                public SqlBatchConfig(string table, string alias)
                {
                        this.TableName = table;
                        this._AliasName = alias;
                }
                public string TableName
                {
                        get;
                        set;
                }
                private readonly string _AliasName;

                private int _ParamIndex = 0;

                public string FormatColumn(SqlColumn column)
                {
                        column.Name = this.FormatName(column.Name);
                        return column.ToString();
                }

                public string GetParamName()
                {
                        return string.Concat("@", this._AliasName, "_", this._ParamIndex++);
                }

                public string FormatName(string name)
                {
                        return string.Concat(this._AliasName, ".", name);
                }
        }
}
