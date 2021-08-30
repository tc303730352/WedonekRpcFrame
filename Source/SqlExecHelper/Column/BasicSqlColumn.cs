namespace SqlExecHelper.Column
{
        internal class BasicSqlColumn
        {
                public BasicSqlColumn(string name) : this(name, name)
                {
                }
                public BasicSqlColumn(string name, string aliasName)
                {
                        this.Name = name;
                        this.AliasName = aliasName;
                }

                public string Name { get; }
                public string AliasName { get; }
        }
}
