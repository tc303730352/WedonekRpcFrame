using SqlExecHelper.Cache;

namespace SqlExecHelper.Column
{
        public class SqlColumn
        {
                public SqlColumn(string name)
                {
                        this.Name = name;
                }
                public SqlColumn(string name, string aliasName)
                {
                        this.Name = name;
                        this._AliasName = aliasName;
                        this._ColumnFunc = null;
                }
                public SqlColumn(string name, SqlFuncType func, string aliasName) : this(name, aliasName)
                {
                        this._ColumnFunc = SqlToolsHelper.GetSqlFunc(func);
                }
                public SqlColumn(string name, SqlFuncType func) : this(name)
                {
                        this._ColumnFunc = SqlToolsHelper.GetSqlFunc(func);
                }
                public SqlColumn(string name, string aliasName, string func) : this(name, aliasName)
                {
                        this._ColumnFunc = func;
                }

                public SqlColumn(BasicColumn column)
                {
                        this.Name = column.Name;
                        this._AliasName = column.AliasName;
                        this._ColumnFunc = column.ColumnFunc;
                }
                public SqlColumn(BasicColumn column, ISqlRunConfig config)
                {
                        this.Name = config.FormatName(column.Name);
                        this._AliasName = column.AliasName;
                        this._ColumnFunc = column.ColumnFunc;
                }

                public string Name { get; internal set; }

                private readonly string _AliasName = null;

                private readonly string _ColumnFunc;

                private string _GetName()
                {
                        return this._AliasName == null ? this.Name : string.Format("{0} as {1}", this.Name, this._AliasName);
                }
                public override string ToString()
                {
                        if (this._ColumnFunc == null)
                        {
                                return this._GetName();
                        }
                        else if (this._AliasName == null)
                        {
                                return string.Format("{0}({1})", this._ColumnFunc, this.Name);
                        }
                        else
                        {
                                return string.Format("{0}({1}) as {2}", this._ColumnFunc, this.Name, this._AliasName);
                        }
                }
        }
}
