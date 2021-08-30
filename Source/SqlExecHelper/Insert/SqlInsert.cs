using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Config;
using SqlExecHelper.SqlValue;

namespace SqlExecHelper.Insert
{
        internal class SqlInsert : ISqlBasic
        {
                public SqlInsert(ClassStructure obj, string table, object val)
                {
                        this.TableName = table;
                        this.Column = obj.GetInsertValue(val);
                        this.Config = new SqlBasicConfig(table);
                }
                public SqlInsert(IInsertSqlValue[] values, ISqlRunConfig config)
                {
                        this.TableName = config.TableName;
                        this.Column = values;
                        this.Config = config;
                }
                public SqlInsert(IInsertSqlValue[] values, string table)
                {
                        this.TableName = table;
                        this.Column = values;
                        this.Config = new SqlBasicConfig(table);
                }
                public ISqlRunConfig Config
                {
                        get;
                }
                /// <summary>
                /// 表明
                /// </summary>
                public string TableName
                {
                        get;
                }

                /// <summary>
                /// 查询参数
                /// </summary>
                public IInsertSqlValue[] Column
                {
                        get;
                }

                protected virtual void _InitFoot(StringBuilder sql, List<IDataParameter> param)
                {

                }
                protected virtual string _GetOutput()
                {
                        return string.Empty;
                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder("insert into ", 256);
                        sql.Append(this.TableName);
                        sql.Append("(");
                        List<IDataParameter> list = new List<IDataParameter>();
                        StringBuilder str = new StringBuilder(128);
                        this.Column.ForEach(a =>
                        {
                                sql.Append(a.ColumnName);
                                sql.Append(",");
                                str.Append(a.GetValue(this.Config, list));
                                str.Append(",");
                        });
                        sql.Remove(sql.Length - 1, 1);
                        str.Remove(str.Length - 1, 1);
                        sql.AppendFormat(") {1} values({0})", str.ToString(), this._GetOutput());
                        this._InitFoot(sql, list);
                        param = list.ToArray();
                        return sql;
                }


        }
}
