using System;
using System.Reflection;

using SqlExecHelper.Column;
using SqlExecHelper.Insert;
using SqlExecHelper.Join;
using SqlExecHelper.SetColumn;
using SqlExecHelper.SqlValue;

namespace SqlExecHelper.Cache
{
        internal class ClassStructure
        {
                public Type Type
                {
                        get;
                        private set;
                }
                public string[] ColumnName
                {
                        get;
                        private set;
                }



                public BasicColumn[] Column
                {
                        get;
                        private set;
                }
                private TableColumn[] _TableColumn = null;
                private BasicSqlColumn[] _BasicColmn = null;
                private SqlUpdateColumn[] _UpdateColumn = null;
                internal SqlUpdateColumn[] GetSetReturnColumn()
                {
                        if (this._UpdateColumn == null)
                        {
                                this._UpdateColumn = this.Column.Convert(a =>
                                {
                                        if (this._CheckIgnore(a, SqlIgnoreType.query))
                                        {
                                                return null;
                                        }
                                        return new SqlUpdateColumn(a);
                                });
                        }
                        return this._UpdateColumn;
                }
                internal SqlUpdateColumn[] GetSetReturnColumn(SqlEventPrefix prefix)
                {
                        return this.Column.Convert(a =>
                       {
                               if (this._CheckIgnore(a, SqlIgnoreType.query))
                               {
                                       return null;
                               }
                               return new SqlUpdateColumn(a, prefix);
                       });
                }
                internal ISqlSetColumn[] GetUpdateColumn(object data)
                {
                        return this.Column.Convert(a =>
                        {
                                if (this._CheckIgnore(a, SqlIgnoreType.update))
                                {
                                        return null;
                                }
                                return new SqlSetColumn(a) { Value = a.GetValue(data) };
                        });
                }

                public static ClassStructure GetStructure(Type type)
                {
                        PropertyInfo[] pros = type.GetProperties();
                        pros = pros.FindAll(a => a.CanWrite && a.ToString().IndexOf("Item [") == -1);
                        ClassStructure obj = new ClassStructure
                        {
                                Type = type,
                                Column = new BasicColumn[pros.Length],
                                ColumnName = new string[pros.Length]
                        };
                        pros.ForEach((a, i) =>
                        {
                                obj.ColumnName[i] = a.Name;
                                obj.Column[i] = BasicColumn.GetColumn(a);
                        });
                        return obj;
                }
                internal SqlColumn[] GetQueryColumn()
                {
                        return this.Column.Convert(a =>
                        {
                                if (this._CheckIgnore(a, SqlIgnoreType.query))
                                {
                                        return null;
                                }
                                return new SqlColumn(a);
                        });
                }
                internal SqlColumn[] GetQueryColumn(JoinTable main, JoinTable[] tables)
                {
                        return this.Column.Convert(a =>
                        {
                                if (this._CheckIgnore(a, SqlIgnoreType.query))
                                {
                                        return null;
                                }
                                else if (a.TableName == null)
                                {
                                        return new SqlColumn(a, main.Config);
                                }
                                else
                                {
                                        JoinTable table = tables.Find(b => b.TableName == a.TableName);
                                        return table != null ? new SqlColumn(a, table.Config) : new SqlColumn(a, main.Config);
                                }
                        });
                }

                internal object[] GetRow<T>(T data)
                {
                        return this.Column.Convert(a =>
                        {
                                if (this._CheckIgnore(a, SqlIgnoreType.insert))
                                {
                                        return null;
                                }
                                return a.GetValue(data);
                        });
                }
                private bool _CheckIgnore(BasicColumn column, SqlIgnoreType ignore)
                {
                        return column.IgnoreType != SqlIgnoreType.无 && (ignore & column.IgnoreType) == ignore;
                }
                internal TableColumn[] GetTableColumn()
                {
                        if (this._TableColumn != null)
                        {
                                return this._TableColumn;
                        }
                        this._TableColumn = this.Column.Convert(a =>
                        {
                                if (this._CheckIgnore(a, SqlIgnoreType.insert))
                                {
                                        return null;
                                }
                                return new TableColumn(a.Name, a.DbType);
                        });
                        return this._TableColumn;
                }

                public BasicSqlColumn[] GetBasicColumn()
                {
                        if (this._BasicColmn == null)
                        {
                                this._BasicColmn = this.Column.Convert(a =>
                                {
                                        if (this._CheckIgnore(a, SqlIgnoreType.query))
                                        {
                                                return null;
                                        }
                                        return new BasicSqlColumn(a.Name, a.AliasName);
                                });
                        }
                        return this._BasicColmn;
                }
                public IInsertSqlValue[] GetInsertValue(object data)
                {
                        return this.Column.Convert(a =>
                        {
                                if (this._CheckIgnore(a, SqlIgnoreType.insert))
                                {
                                        return null;
                                }
                                return new InsertSqlValue(a) { Value = a.GetValue(data) };
                        });
                }
        }
}
