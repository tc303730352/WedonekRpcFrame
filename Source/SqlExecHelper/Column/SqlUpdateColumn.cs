using SqlExecHelper.Cache;

namespace SqlExecHelper.Column
{
        public class SqlUpdateColumn
        {
                private readonly string _Name;
                private readonly string _AliasName;
                private readonly string _EventPrefix;
                public SqlUpdateColumn(string name, SqlEventPrefix prefix)
                {
                        this._Name = name;
                        this._EventPrefix = prefix.ToString();
                }
                public SqlUpdateColumn(BasicColumn obj)
                {
                        this._Name = obj.Name;
                        this._EventPrefix = obj.SqlPrefix.ToString();
                        this._AliasName = obj.AliasName;
                }
                public SqlUpdateColumn(BasicColumn obj, SqlEventPrefix prefix)
                {
                        this._Name = obj.Name;
                        this._EventPrefix = prefix.ToString();
                        this._AliasName = obj.AliasName;
                }
                public SqlUpdateColumn(string name, SqlEventPrefix prefix, string aliasName) : this(name, prefix)
                {
                        this._AliasName = aliasName;
                }
                public override string ToString()
                {
                        return this._AliasName == null
                                ? string.Format("{0}.{1},", this._EventPrefix, this._Name)
                                : string.Format("{0}.{1} as {2},", this._EventPrefix, this._Name, this._AliasName);
                }
        }
}
