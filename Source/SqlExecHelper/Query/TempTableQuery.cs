using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.TempTable;

namespace SqlExecHelper.Query
{
        internal class TempTableQuery : BatchTempTable, ISqlBasic, IBatchQuery
        {
                public TempTableQuery(string table, RunParam param, IDAL myDAL) : base(table, param, "t", myDAL)
                {
                }
                private string _OrderBy = null;

                private string[] _Group = null;

                private ISqlWhere[] _Having = null;

                private int _TopNum = -1;

                private ISqlWhere[] _Where = null;

                private SqlColumn[] _Column = null;
                private int _Skip = 0;
                private int _Size = 0;
                private bool _IsPaging = false;
                private bool _IsCount = true;

                private void _InitFoot(StringBuilder sql, List<IDataParameter> param)
                {
                        if (this._Skip != 0)
                        {
                                SqlTools.InitPaging(sql, this._OrderBy, this._Skip, this._Size, this.Config, param);
                        }
                        else
                        {
                                SqlTools.InitGroup(sql, this._OrderBy, this._Group, this._Having, this.Config, param);
                        }
                }
                private string _GetSqlBody(List<IDataParameter> param)
                {
                        StringBuilder main = new StringBuilder(256);
                        main.AppendFormat(" from {0},{1} where ", this.FullTableName, this._TempTable.TableName);
                        SqlTools.InitColumn(this._TempTable, main, this.Config);
                        if (!this._Where.IsNull())
                        {
                                SqlTools.AppendWhere(main, this.Config, this._Where, param);
                        }
                        return main.ToString();
                }
                private StringBuilder _GeneratePagingSql(out IDataParameter[] param)
                {
                        List<IDataParameter> list = new List<IDataParameter>();
                        string main = this._GetSqlBody(list);
                        StringBuilder sql = new StringBuilder("select ", (main.Length * 2) + 256);
                        SqlTools.InitColumn(sql, this._Column, this.Config);
                        sql.Append(main);
                        if (this._IsCount)
                        {
                                SqlTools.InitPaging(sql, this._OrderBy, this._Skip, this._Size, this._Column[0].Name, this.Config, main, list);
                        }
                        else
                        {
                                SqlTools.InitPaging(sql, this._OrderBy, this._Skip, this._Size, this.Config, list);
                        }
                        param = list.ToArray();
                        return sql;
                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        if (this._IsPaging)
                        {
                                return this._GeneratePagingSql(out param);
                        }
                        else
                        {
                                StringBuilder sql = new StringBuilder("select ", 512);
                                SqlTools.InitHead(sql, this._TopNum);
                                SqlTools.InitColumn(sql, this._Column, this.Config);
                                List<IDataParameter> list = new List<IDataParameter>();
                                sql.Append(this._GetSqlBody(list));
                                this._InitFoot(sql, list);
                                param = list.ToArray();
                                return sql;
                        }
                }
                private bool _Query<T>(out T[] datas, out long count)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                count = 0;
                                return false;
                        }
                        return SqlHelper.Query(this, this._MyDAL, out datas, out count);
                }
                private bool _ExecuteScalar<T>(out T value)
                {
                        if (!this._TempTable.Save())
                        {
                                value = default;
                                return false;
                        }
                        return SqlHelper.ExecuteScalar(this, this._MyDAL, out value);
                }
                private bool _GetTable<T>(out T[] datas)
                {
                        if (!this._TempTable.Save())
                        {
                                datas = null;
                                return false;
                        }
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                private bool _GetRow<T>(out T data)
                {
                        if (!this._TempTable.Save())
                        {
                                data = default;
                                return false;
                        }
                        return SqlHelper.GetRow(this, this._MyDAL, out data);
                }
                #region 分组查询

                public bool GroupByOne<T>(string group, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = new SqlColumn[] {
                                new SqlColumn(group),
                        };
                        this._Group = new string[] { group };
                        return this._GetTable(out datas);
                }
                public bool Group<T>(string group, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return this._GetTable(out datas);
                }
                public bool Group<T>(string group, out T data, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return this._GetRow(out data);
                }
                public bool Group<T>(string group, string sortBy, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._OrderBy = sortBy;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return this._GetTable(out datas);
                }
                public bool Group<T>(string[] group, out T data, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = group;
                        return this._GetRow(out data);
                }
                public bool Group<T>(string[] group, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = group;
                        return this._GetTable(out datas);
                }
                public bool Group<T>(string[] group, string sortBy, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._OrderBy = sortBy;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = group;
                        return this._GetTable(out datas);
                }
                #endregion

                #region 分组查询(Group by having)
                public bool GroupByOne<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Having = having;
                        this._Where = where;
                        this._Column = new SqlColumn[] {
                                new SqlColumn(group),
                        };
                        this._Group = new string[] { group };
                        return this._GetTable(out datas);
                }
                public bool Group<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return this._GetTable(out datas);
                }
                public bool Group<T>(string group, ISqlWhere[] having, out T data, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return this._GetRow(out data);
                }
                public bool Group<T>(string group, string orderBy, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._OrderBy = orderBy;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return this._GetTable(out datas);
                }


                public bool Group<T>(string[] groups, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = groups;
                        return this._GetTable(out datas);
                }
                public bool Group<T>(string[] groups, string orderBy, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._OrderBy = orderBy;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = groups;
                        return this._GetTable(out datas);
                }
                #endregion

                #region 单列函数查询(count,sum等)
                public bool ExecuteScalar<T>(string column, SqlFuncType funcType, out T value, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = new SqlColumn[]
                        {
                                new SqlColumn(column, funcType)
                        };
                        return this._ExecuteScalar(out value);
                }
                public bool ExecuteScalar<T>(string column, string funcType, out T value, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = new SqlColumn[]
                        {
                                new SqlColumn(column, null,funcType)
                        };
                        return this._ExecuteScalar(out value);
                }
                #endregion

                #region 查询
                public bool Query<T>(string column, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = new SqlColumn[]
                        {
                                new SqlColumn(column)
                        };
                        return this._GetTable(out datas);
                }
                public bool Query<T>(out T[] datas, params ISqlWhere[] where)
                {
                        return this.QueryBySort(null, out datas, where);
                }
                #endregion

                #region 排序查询
                public bool QueryBySort<T>(string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._OrderBy = orderBy;
                        return this._GetTable(out datas);
                }
                #endregion

                #region 分页查询
                public bool QueryByPaging<T>(string orderBy, int index, int size, out T[] datas, out long count, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._OrderBy = orderBy;
                        this._IsPaging = true;
                        this._Skip = (index - 1) * size;
                        this._Size = size;
                        return this._Query(out datas, out count);
                }

                public bool QueryByPaging<T>(string orderBy, int index, int size, out T[] datas, params ISqlWhere[] where)
                {
                        this._IsPaging = true;
                        this._IsCount = false;
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._OrderBy = orderBy;
                        this._Skip = (index - 1) * size;
                        this._Size = size;
                        return this._GetTable(out datas);
                }
                #endregion

                #region Top查询
                public bool QueryByTop<T>(int topNum, out T[] datas, params ISqlWhere[] where)
                {
                        return this.QueryByTop(topNum, null, out datas, where);
                }
                public bool QueryByTop<T>(int topNum, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        this._TopNum = topNum;
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._OrderBy = orderBy;
                        return this._GetTable(out datas);
                }
                #endregion
        }
}
