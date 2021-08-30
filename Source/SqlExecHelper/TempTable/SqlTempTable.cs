using System;
using System.Data;

using SqlExecHelper.Config;

namespace SqlExecHelper.TempTable
{
        internal class SqlTempTable : IBatchSql
        {
                private readonly IDAL _MyDAL = null;
                public SqlTempTable(IDAL myDAL)
                {
                        this._MyDAL = myDAL;
                        string table = string.Concat("Temp_", Guid.NewGuid().ToString("N"));
                        this._TableName = table;
                        this.TableName = string.Format("{0} as a with(nolock)", table);
                        this.Config = new SqlBatchConfig(table, "a");
                }
                private readonly string _TableName = null;
                public ISqlRunConfig Config
                {
                        get;
                }
                public int RowCount => this._Table.Rows.Count;
                public string TableName
                {
                        get;
                }
                public SqlTableColumn[] Column
                {
                        get;
                        set;
                }
                private DataTable _Table = null;



                private bool _IsInit = false;

                private bool _IsSave = false;
                private void _InitTable()
                {
                        if (this._IsInit)
                        {
                                return;
                        }
                        this._IsInit = true;
                        this._Table = new DataTable(this._TableName);
                        this.Column.ForEach(a =>
                        {
                                this._Table.Columns.Add(a.Name, SqlToolsHelper.GetType(a.SqlDbType));
                        });
                }

                public void AddRow(object[] datas)
                {
                        if (datas.Length != this.Column.Length)
                        {
                                return;
                        }
                        else
                        {
                                this._InitTable();
                                for (int i = 0; i < datas.Length; i++)
                                {
                                        SqlTableColumn column = this.Column[i];
                                        datas[i] = column.FormatValue(datas[i]);
                                }
                                this._Table.Rows.Add(datas);
                        }
                }

                public bool Save()
                {
                        if (this._IsSave)
                        {
                                return true;
                        }
                        else if (this._Table.Rows.Count == 0)
                        {
                                return false;
                        }
                        else
                        {
                                string table = SqlTools.GetTableSql(this._Table.TableName, this.Column);
                                if (this._MyDAL.ExecuteNonQuery(table) == -2)
                                {
                                        return false;
                                }
                                else
                                {
                                        this._IsSave = this._MyDAL.InsertTable(this._Table);
                                        return this._IsSave;
                                }
                        }
                }

                public void Dispose()
                {
                        this._MyDAL.DropTable(this._TableName);
                }
        }
}
