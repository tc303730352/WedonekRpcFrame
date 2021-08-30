using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;

namespace SqlExecHelper.Update
{
        internal class SqlUpdate : ISqlBasic
        {
                public SqlUpdate(string table, RunParam param, ClassStructure obj, object data, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, param);
                        this.Column = obj.GetUpdateColumn(data);
                        this.Config = new Config.SqlBasicConfig(table);
                        this.Where = where;
                }
                public SqlUpdate(string table, RunParam param, ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, param);
                        this.Column = columns;
                        this.Config = new Config.SqlBasicConfig(table);
                        this.Where = where;
                }
                private readonly string _Name = null;
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
                public ISqlWhere[] Where
                {
                        get;
                }
                /// <summary>
                /// 修改的列
                /// </summary>
                public ISqlSetColumn[] Column
                {
                        get;
                }


                protected virtual void _InitOutput(StringBuilder sql)
                {

                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(256);
                        sql.AppendFormat("update {0} set ", this._Name);
                        List<IDataParameter> list = new List<IDataParameter>();
                        SqlTools.InitSetColumn(sql, this.Column, this.Config, list);
                        this._InitOutput(sql);
                        if (this.Where != null && this.Where.Length > 0)
                        {
                                SqlTools.InitWhere(sql, this.Where, this.Config, list);
                        }
                        param = list.ToArray();
                        return sql;
                }

        }
}
