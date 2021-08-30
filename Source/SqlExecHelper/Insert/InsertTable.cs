using System.Data;

namespace SqlExecHelper.Insert
{
        internal class InsertTable : IInsertTable
        {
                private readonly IDAL _MyDAL = null;

                public InsertTable(string tableName, IDAL myDAL)
                {
                        this.TableName = tableName;
                        this._MyDAL = myDAL;
                        this._Table = new DataTable(tableName);
                }
                public string TableName
                {
                        get;
                }
                public int RowCount => this._Table.Rows.Count;
                public TableColumn[] Column
                {
                        get;
                        set;
                }
                private readonly DataTable _Table = null;
                private bool _IsInit = false;
                private bool _InitTable()
                {
                        if (this.Column == null || this.Column.Length == 0)
                        {
                                return false;
                        }
                        else if (!this._IsInit)
                        {
                                this._IsInit = true;
                                foreach (TableColumn col in this.Column)
                                {
                                        this._Table.Columns.Add(col.GetColumn());
                                }
                        }
                        return this._IsInit;
                }

                public void AddRow(params object[] datas)
                {
                        if (!this._IsInit && !this._InitTable())
                        {
                                return;
                        }
                        else if (datas.Length == this.Column.Length)
                        {
                                for (int i = 0; i < datas.Length; i++)
                                {
                                        TableColumn column = this.Column[i];
                                        object val = datas[i];
                                        if (column.IsNull && val == null)
                                        {
                                                continue;
                                        }
                                        datas[i] = SqlToolsHelper.FormatValue(column.SqlDbType, val);
                                }
                                this._Table.Rows.Add(datas);
                        }
                }

                public bool Save()
                {
                        return this._Table.Rows.Count != 0 && this._MyDAL.InsertTable(this._Table);
                }
        }
}
