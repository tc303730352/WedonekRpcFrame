using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.SqlValue;

namespace SqlExecHelper.Insert
{
        public class BatchInsert : ISqlBasic, IBatchInsert
        {
                private readonly IDAL _MyDAL = null;
                private BasicSqlColumn[] _Output = null;
                public BatchInsert(string table, IDAL myDAL)
                {
                        this._MyDAL = myDAL;
                        this.TableName = table;
                        this.Config = new Config.SqlBasicConfig(table);
                }
                public ISqlRunConfig Config
                {
                        get;
                }
                public TableColumn[] Column { get; set; }

                public int RowCount => this._Rows.Count;
                public string TableName { get; }

                private readonly List<object[]> _Rows = new List<object[]>();

                public void AddRow(params object[] datas)
                {
                        if (datas.Length == this.Column.Length)
                        {
                                this._Rows.Add(datas);
                        }
                }
                private void _LoadValues(StringBuilder sql, out IDataParameter[] param)
                {
                        List<IDataParameter> list = new List<IDataParameter>(this.Column.Length * this._Rows.Count);
                        this._Rows.ForEach(a =>
                        {
                                sql.Append("(");
                                this.Column.ForEach((b, i) =>
                                {
                                        if (!b.IsIdentify)
                                        {
                                                IInsertSqlValue t = new InsertSqlValue(b.Name, b.SqlDbType, b.Size) { Value = a[i], IsNull = b.IsNull };
                                                sql.Append(t.GetValue(this.Config, list));
                                                sql.Append(",");
                                        }
                                });
                                sql.Remove(sql.Length - 1, 1);
                                sql.Append("),");
                        });
                        sql.Remove(sql.Length - 1, 1);
                        param = list.ToArray();
                }
                public bool Save()
                {
                        return this._Rows.Count != 0 && (this._Rows.Count == 1 ? this._Insert(this._Rows[0]) : SqlHelper.ExecuteNonQuery(this, this._MyDAL) > 0);
                }
                #region 插入单条
                private bool _Insert(object[] datas)
                {

                        IInsertSqlValue[] values = this.Column.Convert(a => a.IsIdentify == false, (a, i) => new InsertSqlValue(a.Name, a.SqlDbType, a.Size) { Value = datas[i] });
                        ISqlBasic sql = new SqlInsert(values, this.Config);
                        return SqlHelper.ExecuteNonQuery(sql, this._MyDAL) > 0;
                }
                private bool _Insert(object[] row, out int[] datas)
                {
                        IInsertSqlValue[] values = this.Column.Convert(a => a.IsIdentify == false, (a, i) => new InsertSqlValue(a.Name, a.SqlDbType, a.Size) { Value = row[i] });
                        ISqlBasic sql = new SqlInsertById(values, this.Config, SqlDbType.Int);
                        if (!SqlHelper.Insert(sql, this._MyDAL, out int id))
                        {
                                datas = null;
                                return false;
                        }
                        datas = new int[] { id };
                        return true;
                }
                private bool _Insert(object[] row, out long[] datas)
                {
                        IInsertSqlValue[] values = this.Column.Convert(a => a.IsIdentify == false, (a, i) => new InsertSqlValue(a.Name, a.SqlDbType, a.Size) { Value = row[i] });
                        ISqlBasic sql = new SqlInsertById(values, this.Config, SqlDbType.BigInt);
                        if (!SqlHelper.Insert(sql, this._MyDAL, out int id))
                        {
                                datas = null;
                                return false;
                        }
                        datas = new long[] { id };
                        return true;
                }
                private bool _Insert<T>(object[] row, out T[] datas)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        IInsertSqlValue[] values = this.Column.Convert(a => a.IsIdentify == false, (a, i) => new InsertSqlValue(a.Name, a.SqlDbType, a.Size) { Value = row[i] });
                        ISqlBasic sql = new SqlInsertOut(values, this.Config, obj);
                        if (!SqlHelper.GetRow(sql, this._MyDAL, out T data))
                        {
                                datas = null;
                                return false;
                        }
                        datas = new T[] { data };
                        return true;
                }
                private bool _Insert<T>(object[] row, string column, out T[] datas)
                {
                        IInsertSqlValue[] values = this.Column.Convert(a => a.IsIdentify == false, (a, i) => new InsertSqlValue(a.Name, a.SqlDbType, a.Size) { Value = row[i] });
                        ISqlBasic sql = new SqlInsertOut(values, this.Config, column);
                        if (!SqlHelper.GetRow(sql, this._MyDAL, out T data))
                        {
                                datas = null;
                                return false;
                        }
                        datas = new T[] { data };
                        return true;
                }
                #endregion

                protected virtual void _InitFoot(StringBuilder sql)
                {

                }
                protected virtual string _GetOutput()
                {
                        if (this._Output == null || this._Output.Length == 0)
                        {
                                return string.Empty;
                        }
                        StringBuilder str = new StringBuilder(" output ", 64);
                        this._Output.ForEach(a =>
                        {
                                if (a.AliasName != null)
                                {
                                        str.AppendFormat("inserted.{0} as {1},", a.Name, a.AliasName);
                                }
                                else
                                {
                                        str.AppendFormat("inserted.{0},", a.Name);
                                }
                        });
                        str.Remove(str.Length - 1, 1);
                        return str.ToString();
                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(512);
                        sql.AppendFormat("insert into {0}({1}) {2} values", this.TableName, this.Column.Join(",", a => !a.IsIdentify, a => a.Name), this._GetOutput());
                        this._LoadValues(sql, out param);
                        this._InitFoot(sql);
                        return sql;
                }

                public bool Save<T>(out T[] datas)
                {
                        if (this._Rows.Count == 0)
                        {
                                datas = null;
                                return false;
                        }
                        else if (this._Rows.Count == 1)
                        {
                                return this._Insert(this._Rows[0], out datas);
                        }
                        else
                        {
                                ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                                this._Output = obj.GetBasicColumn();
                                return SqlHelper.GetTable(this, this._MyDAL, out datas);
                        }
                }
                public bool Save<T>(string column, out T[] datas)
                {
                        if (this._Rows.Count == 0)
                        {
                                datas = null;
                                return false;
                        }
                        else if (this._Rows.Count == 1)
                        {
                                return this._Insert(this._Rows[0], column, out datas);
                        }
                        else
                        {
                                this._Output = new BasicSqlColumn[]
                                {
                                        new BasicSqlColumn(column)
                                };
                                return SqlHelper.GetTable(this, this._MyDAL, out datas);
                        }
                }
                public bool Save(out int[] datas)
                {
                        if (this._Rows.Count == 0)
                        {
                                datas = null;
                                return false;
                        }
                        else
                        {
                                return this._Rows.Count == 1 ? this._Insert(this._Rows[0], out datas) : SqlHelper.BatchInsert(this, this._MyDAL, out datas);
                        }
                }
                public bool Save(out long[] datas)
                {
                        if (this._Rows.Count == 0)
                        {
                                datas = null;
                                return false;
                        }
                        else if (this._Rows.Count == 1)
                        {
                                return this._Insert(this._Rows[0], out datas);
                        }
                        else
                        {
                                return SqlHelper.BatchInsert(this, this._MyDAL, out datas);
                        }
                }
        }
}
