using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Column;
using SqlExecHelper.Interface;
using SqlExecHelper.Join;

namespace SqlExecHelper
{
        internal class SqlTools
        {
                public static void InitWhereColumn(IBatchSql table, StringBuilder sql, ISqlRunConfig config)
                {
                        table.Column.ForEach(a =>
                        {
                                if (((SqlColType.查询 | SqlColType.标识) & a.ColType) == 0)
                                {
                                        return;
                                }
                                sql.AppendFormat("{0} {2} {1} and ", config.FormatName(a.Name), table.Config.FormatName(a.Name), SqlToolsHelper.GetSymbol(a.QueryType));
                        });
                        sql.Remove(sql.Length - 4, 4);
                }
                public static void InitColumn(IBatchSql table, StringBuilder sql, ISqlRunConfig config)
                {
                        table.Column.ForEach(a => sql.AppendFormat("{0}={1},", config.FormatName(a.Name), table.Config.FormatName(a.Name)));
                        sql.Remove(sql.Length - 1, 1);
                }
                public static string GetInsertColumn(IBatchSql table)
                {
                        StringBuilder str = new StringBuilder();
                        table.Column.ForEach(a =>
                        {
                                if ((SqlColType.添加 & a.ColType) != SqlColType.添加 || a.ColType == SqlColType.标识)
                                {
                                        return;
                                }
                                str.Append(a.Name);
                                str.Append(",");
                        });
                        str.Remove(str.Length - 1, 1);
                        return str.ToString();
                }
                public static void InitInsertColumn(IBatchSql table, StringBuilder sql)
                {
                        table.Column.ForEach(a =>
                        {
                                if ((SqlColType.添加 & a.ColType) != SqlColType.添加 || (SqlColType.标识 & a.ColType) == SqlColType.标识)
                                {
                                        return;
                                }
                                sql.Append(table.Config.FormatName(a.Name));
                                sql.Append(",");
                        });
                        sql.Remove(sql.Length - 1, 1);
                }
                public static void InitSetColumn(IBatchSql table, StringBuilder sql, ISqlRunConfig config)
                {
                        table.Column.ForEach(a =>
                        {
                                if ((SqlColType.修改 & a.ColType) != SqlColType.修改 || (SqlColType.标识 & a.ColType) == SqlColType.标识)
                                {
                                        return;
                                }
                                else if (a.SetType == SqlSetType.等于)
                                {
                                        sql.AppendFormat("{0}={1},", config.FormatName(a.Name), table.Config.FormatName(a.Name));
                                }
                                else if (a.SetType == SqlSetType.递加)
                                {
                                        sql.AppendFormat("{0}={0}+{1},", config.FormatName(a.Name), table.Config.FormatName(a.Name));
                                }
                                else
                                {
                                        sql.AppendFormat("{0}={0}-{1},", config.FormatName(a.Name), table.Config.FormatName(a.Name));
                                }
                        });
                        sql.Remove(sql.Length - 1, 1);
                }
                public static void InitSetColumn(IBatchSql table, StringBuilder sql, ISqlRunConfig config, ISqlSetColumn[] setColumn, List<IDataParameter> param)
                {
                        table.Column.ForEach(a =>
                        {
                                if ((SqlColType.修改 & a.ColType) != SqlColType.修改 || a.ColType == SqlColType.标识)
                                {
                                        return;
                                }
                                else if (a.SetType == SqlSetType.递减)
                                {
                                        sql.AppendFormat("{0}={0}-{1},", config.FormatName(a.Name), table.Config.FormatName(a.Name));
                                }
                                else if (a.SetType == SqlSetType.递加)
                                {
                                        sql.AppendFormat("{0}={0}+{1},", config.FormatName(a.Name), table.Config.FormatName(a.Name));
                                }
                                else
                                {
                                        sql.AppendFormat("{0}={1},", config.FormatName(a.Name), table.Config.FormatName(a.Name));
                                }
                        });
                        if (setColumn != null)
                        {
                                setColumn.ForEach(a =>
                                {
                                        a.GenerateSql(sql, config, param);
                                });
                        }
                        sql.Remove(sql.Length - 1, 1);
                }
                public static string GetTableSql(string tableName, SqlTableColumn[] columns)
                {
                        StringBuilder table = new StringBuilder();
                        table.AppendFormat("create table dbo.[{0}](", tableName);
                        foreach (SqlTableColumn col in columns)
                        {
                                table.AppendFormat("[{0}] {1} {2},", col.Name, SqlTools.GetSqlDbTypeStr(col), "not null");
                        }
                        table.Remove(table.Length - 1, 1);
                        table.Append(")");
                        return table.ToString();
                }
                internal static string GetSqlDbTypeStr(SqlTableColumn col)
                {
                        switch (col.SqlDbType)
                        {
                                case SqlDbType.Decimal:
                                        return string.Format("[decimal]({0},{1})", col.Size, col.DecimalPoint);
                                case SqlDbType.VarChar:
                                        return string.Format("[varchar]({0})", col.Size == 0 ? "max" : col.Size.ToString());
                                case SqlDbType.NVarChar:
                                        return string.Format("[nvarchar]({0})", col.Size == 0 ? "max" : col.Size.ToString());
                                case SqlDbType.Char:
                                        return string.Format("[char]({0})", col.Size == 0 ? "8000" : col.Size.ToString());
                                case SqlDbType.Binary:
                                        return string.Format("[binary]({0})", col.Size == 0 ? "8000" : col.Size.ToString());
                                default:
                                        return string.Format("[{0}]", col.SqlDbType.ToString());
                        }
                }
                public static void InitColumn(StringBuilder sql, SqlColumn[] columns, ISqlRunConfig config)
                {
                        if (columns.Length == 1)
                        {
                                sql.Append(config.FormatColumn(columns[0]));
                        }
                        else
                        {
                                columns.ForEach(a =>
                                {
                                        sql.Append(config.FormatColumn(a));
                                        sql.Append(",");
                                });
                                sql.Remove(sql.Length - 1, 1);
                        }
                }
                public static void InitColumn(StringBuilder sql, SqlColumn[] columns)
                {
                        if (columns.Length == 1)
                        {
                                sql.Append(columns[0].ToString());
                                return;
                        }
                        columns.ForEach(a =>
                        {
                                sql.Append(a.ToString());
                                sql.Append(",");
                        });
                        sql.Remove(sql.Length - 1, 1);
                }
                public static void InitOrderBy(StringBuilder sql, string orderby, ISqlRunConfig config)
                {
                        if (orderby != null)
                        {
                                sql.Append(" order by ");
                                if (orderby.IndexOf(',') == -1)
                                {
                                        sql.Append(config.FormatName(orderby));
                                }
                                else
                                {
                                        orderby.Split(',').ForEach(a =>
                                        {
                                                sql.Append(config.FormatName(a));
                                                sql.Append(",");
                                        });
                                        sql.Remove(sql.Length - 1, 1);
                                }
                        }
                }



                internal static void InitOrderBy(StringBuilder sql, IOrderBy[] orderBy, JoinTable main, JoinTable[] table)
                {
                        sql.Append(" order by ");
                        if (orderBy.Length == 1)
                        {
                                sql.Append(orderBy[0].GetOrderBy(main.Config));
                                return;
                        }
                        orderBy.ForEach(a =>
                        {
                                if (a.Table != null && a.Table != main.TableName)
                                {
                                        JoinTable i = table.Find(c => c.TableName == a.Table);
                                        sql.Append(a.GetOrderBy(i.Config));
                                }
                                else
                                {
                                        sql.Append(a.GetOrderBy(main.Config));
                                }
                                sql.Append(",");
                        });
                        sql.Remove(sql.Length - 1, 1);
                }

                public static void InitHead(StringBuilder sql, int topNum)
                {
                        if (topNum != -1)
                        {
                                sql.AppendFormat("top {0} ", topNum);
                        }
                }

                public static void InitGroup(StringBuilder sql, string orderBy, string[] group, ISqlWhere[] having, ISqlRunConfig config, List<IDataParameter> param)
                {
                        _InitGroupBy(sql, group);
                        _InitHaving(sql, having, config, param);
                        InitOrderBy(sql, orderBy, config);
                }
                internal static void InitSetColumn(StringBuilder sql, ISqlSetColumn[] column, ISqlRunConfig main, ISqlRunConfig join, List<IDataParameter> param)
                {
                        if (column.Length == 1)
                        {
                                _InitSetColumn(sql, column[0], main, join, param);
                        }
                        else
                        {
                                column.ForEach(a =>
                                {
                                        _InitSetColumn(sql, a, main, join, param);
                                });
                        }
                        sql.Remove(sql.Length - 1, 1);
                }
                private static void _InitSetColumn(StringBuilder sql, ISqlSetColumn column, ISqlRunConfig main, ISqlRunConfig join, List<IDataParameter> param)
                {
                        if (column is ISqlJoinSetColumn i)
                        {
                                i.GenerateSql(sql, main, join);
                        }
                        else
                        {
                                column.GenerateSql(sql, main, param);
                        }
                }
                internal static void InitSetColumn(StringBuilder sql, ISqlSetColumn[] column, ISqlRunConfig config, List<IDataParameter> param)
                {
                        if (column.Length == 1)
                        {
                                column[0].GenerateSql(sql, config, param);
                        }
                        else
                        {
                                column.ForEach(a =>
                                {
                                        a.GenerateSql(sql, config, param);
                                });
                        }
                        sql.Remove(sql.Length - 1, 1);
                }

                private static void _InitGroupBy(StringBuilder sql, string[] group)
                {
                        if (group.IsNull())
                        {
                                return;
                        }
                        sql.Append(" group by ");
                        if (group.Length == 1)
                        {
                                sql.Append(group[0]);
                        }
                        else
                        {
                                group.ForEach(a =>
                                {
                                        sql.Append(a);
                                        sql.Append(",");
                                });
                                sql.Remove(sql.Length - 1, 1);
                        }
                }
                internal static void InitWhere(StringBuilder sql, ISqlWhere[] where, ISqlRunConfig main, ISqlRunConfig join, List<IDataParameter> param)
                {
                        sql.Append(" where ");
                        sql.Append(GetWhere(where, main, join, param));
                }
                public static string GetWhere(ISqlWhere[] where, ISqlRunConfig main, ISqlRunConfig join, List<IDataParameter> param)
                {
                        StringBuilder str = new StringBuilder(64);
                        where.ForEach(a =>
                        {
                                if (a is ISqlLinkWhere i)
                                {
                                        i.GenerateSql(str, main, join, param);
                                }
                                else
                                {
                                        a.GenerateSql(str, main, param);
                                }
                        });
                        str.Remove(0, 3);
                        return str.ToString();
                }
                internal static void InitWhere(StringBuilder sql, ISqlWhere[] where, ISqlRunConfig config, List<IDataParameter> param)
                {
                        string str = GetWhere(where, config, param);
                        if (str != string.Empty)
                        {
                                sql.Append(" where ");
                                sql.Append(str);
                        }
                }
                internal static void InitWhere(StringBuilder sql, ISqlWhere[] where, JoinTable main, JoinTable[] tables, List<IDataParameter> param)
                {
                        sql.Append(" where ");
                        sql.Append(GetWhere(where, main, tables, param));
                }
                public static string GetWhere(ISqlWhere[] where, JoinTable main, JoinTable[] tables, List<IDataParameter> param)
                {
                        StringBuilder str = new StringBuilder(64);
                        where.ForEach(a =>
                        {
                                if (a is ISqlJoinWhere i)
                                {
                                        JoinTable table = tables.Length == 1 ? tables[0] : tables.Find(c => c.TableName == i.Table);
                                        i.GenerateSql(str, main.Config, table.Config, param);
                                }
                                else if (a is ISqlTableWhere k && k.Table != null && k.Table != main.TableName)
                                {
                                        JoinTable table = tables.Find(c => c.TableName == k.Table);
                                        a.GenerateSql(str, table.Config, param);
                                }
                                else
                                {
                                        a.GenerateSql(str, main.Config, param);
                                }
                        });
                        str.Remove(0, 3);
                        return str.ToString();
                }
                private static void _InitHaving(StringBuilder sql, ISqlWhere[] having, ISqlRunConfig config, List<IDataParameter> param)
                {
                        if (having.IsNull())
                        {
                                return;
                        }
                        sql.Append(" having ");
                        StringBuilder str = new StringBuilder();
                        if (having.Length == 1)
                        {
                                having[0].GenerateSql(str, config, param);
                        }
                        else
                        {
                                having.ForEach(a =>
                                {
                                        a.GenerateSql(sql, config, param);
                                });
                        }
                        str.Remove(0, 3);
                        sql.Append(str.ToString());
                }


                public static void AppendWhere(StringBuilder sql, ISqlRunConfig config, ISqlWhere[] where, List<IDataParameter> param)
                {
                        sql.Append(" and ");
                        sql.Append(GetWhere(where, config, param));
                }
                public static void InitPaging(StringBuilder sql, string orderBy, int skip, int size, string column, ISqlRunConfig config, string body, List<IDataParameter> param)
                {
                        InitOrderBy(sql, orderBy, config);
                        ISqlExtendParameter count = new Param.WhereSqlParameter("DataCount", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                        count.InitParam(config);
                        ISqlExtendParameter skipParam = new Param.WhereSqlParameter("Skip", SqlDbType.Int) { Value = skip };
                        skipParam.InitParam(config);
                        ISqlExtendParameter sizeParam = new Param.WhereSqlParameter("Size", SqlDbType.Int) { Value = size };
                        sizeParam.InitParam(config);
                        sql.AppendFormat(" offset {0} row fetch next {1} rows only;select {2}=count({3})", skipParam.ParamName, sizeParam.ParamName, count.ParamName, column);
                        sql.Append(body);
                        param.Add(skipParam.GetParameter());
                        param.Add(sizeParam.GetParameter());
                        param.Add(count.GetParameter());
                }
                public static string InitBatchPaging(StringBuilder sql, string orderBy, int skip, int size, string column, ISqlRunConfig config, string body, List<IDataParameter> param)
                {
                        sql.Append(",1 as t_is");
                        sql.Append(body);
                        InitOrderBy(sql, orderBy, config);
                        ISqlExtendParameter skipParam = new Param.WhereSqlParameter("Skip", SqlDbType.Int) { Value = skip };
                        skipParam.InitParam(config);
                        ISqlExtendParameter sizeParam = new Param.WhereSqlParameter("Size", SqlDbType.Int) { Value = size };
                        sizeParam.InitParam(config);
                        sql.AppendFormat(" offset {0} row fetch next {1} rows only", skipParam.ParamName, sizeParam.ParamName);
                        param.Add(skipParam.GetParameter());
                        param.Add(sizeParam.GetParameter());
                        return string.Format("select a.Counts,b.t_is,b.* from (select count({1}) as Counts{2}) as a left join ({0}) as b on 1=1", sql.ToString(), column, body);
                }
                internal static void InitPaging(StringBuilder sql, IOrderBy[] orderBy, JoinTable main, JoinTable[] tables, int skip, int size, List<IDataParameter> param)
                {
                        InitOrderBy(sql, orderBy, main, tables);
                        ISqlExtendParameter skipParam = new Param.WhereSqlParameter("Skip", SqlDbType.Int) { Value = skip };
                        skipParam.InitParam(main.Config);
                        ISqlExtendParameter sizeParam = new Param.WhereSqlParameter("Size", SqlDbType.Int) { Value = size };
                        sizeParam.InitParam(main.Config);
                        sql.AppendFormat(" offset {0} row fetch next {1} rows only;", skipParam.ParamName, sizeParam.ParamName);
                        param.Add(skipParam.GetParameter());
                        param.Add(sizeParam.GetParameter());
                }
                internal static void InitPaging(StringBuilder sql, IOrderBy[] orderBy, JoinTable main, JoinTable[] tables, string column, string body, int skip, int size, List<IDataParameter> param)
                {
                        InitOrderBy(sql, orderBy, main, tables);
                        ISqlExtendParameter count = new Param.WhereSqlParameter("DataCount", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                        count.InitParam(main.Config);
                        ISqlExtendParameter skipParam = new Param.WhereSqlParameter("Skip", SqlDbType.Int) { Value = skip };
                        skipParam.InitParam(main.Config);
                        ISqlExtendParameter sizeParam = new Param.WhereSqlParameter("Size", SqlDbType.Int) { Value = size };
                        sizeParam.InitParam(main.Config);
                        sql.AppendFormat(" offset {0} row fetch next {1} rows only;select {2}=count({3})", skipParam.ParamName, sizeParam.ParamName, count.ParamName, column);
                        sql.Append(body);
                        param.Add(skipParam.GetParameter());
                        param.Add(sizeParam.GetParameter());
                        param.Add(count.GetParameter());
                }
                public static void InitPaging(StringBuilder sql, string orderBy, int skip, int size, ISqlRunConfig config, List<IDataParameter> param)
                {
                        InitOrderBy(sql, orderBy, config);
                        ISqlExtendParameter skipParam = new Param.WhereSqlParameter("Skip", SqlDbType.Int) { Value = skip };
                        skipParam.InitParam(config);
                        ISqlExtendParameter sizeParam = new Param.WhereSqlParameter("Size", SqlDbType.Int) { Value = size };
                        sizeParam.InitParam(config);
                        sql.AppendFormat(" offset {0} row fetch next {1} rows only;", skipParam.ParamName, sizeParam.ParamName);
                        param.Add(skipParam.GetParameter());
                        param.Add(sizeParam.GetParameter());
                }
                public static string GetWhere(ISqlWhere[] where, ISqlRunConfig config, List<IDataParameter> param)
                {
                        StringBuilder str = new StringBuilder(64);
                        where.ForEach(a =>
                        {
                                if (a != null)
                                {
                                        a.GenerateSql(str, config, param);
                                }
                        });
                        if (str.Length > 0)
                        {
                                str.Remove(0, 3);
                        }
                        return str.ToString();
                }

                public static string FormatTableName(string tableName, string alias)
                {
                        return alias != null ? string.Format("{0} as {1}", tableName, alias) : tableName;
                }

                public static string GetTableName(string tableName, string alias, RunParam param)
                {
                        return string.Concat(FormatTableName(tableName, alias), param.ToString());
                }
                public static string GetTableName(string tableName, RunParam param)
                {
                        return string.Concat(tableName, param.ToString());
                }
        }
}
