using SqlExecHelper.Config;

namespace SqlExecHelper.TempTable
{
        internal class BatchTempTable : System.IDisposable
        {
                protected readonly SqlTempTable _TempTable = null;
                private readonly string _Alias = null;
                protected readonly IDAL _MyDAL = null;

                protected readonly string FullTableName;
                public BatchTempTable(string table, RunParam param, string alias, IDAL myDAL) : this(table, alias, myDAL)
                {
                        this.FullTableName = SqlTools.GetTableName(table, alias, param);
                }
                public BatchTempTable(string table, IDAL myDAL, string alias) : this(table, alias, myDAL)
                {
                        this.FullTableName = SqlTools.FormatTableName(table, alias);
                }
                private BatchTempTable(string table, string alias, IDAL myDAL)
                {
                        this._Alias = string.IsNullOrEmpty(alias) ? table : alias;
                        this._MyDAL = myDAL;
                        this._TempTable = new SqlTempTable(myDAL);
                        this.TableName = table;
                        this.Config = new SqlBatchConfig(table, this._Alias);
                }
                public SqlTableColumn[] Column
                {
                        get => this._TempTable.Column;
                        set => this._TempTable.Column = value;
                }
                public string TableName { get; }

                public int RowCount => this._TempTable.RowCount;
                public ISqlRunConfig Config { get; }

                public void AddRow(params object[] datas)
                {
                        this._TempTable.AddRow(datas);
                }
                public void Dispose()
                {
                        this._TempTable.Dispose();
                }
        }
}
