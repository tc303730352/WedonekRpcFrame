using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.Config;

namespace SqlExecHelper.Query
{
        internal class BatchQuery : ISqlBasic, IBatchQuery
        {
                private readonly IDAL _MyDAL = null;

                private readonly Batch.BatchSql _BatchExcel = null;
                public BatchQuery(string table, RunParam param, IDAL myDAL) : this(table, myDAL)
                {
                        this._Name = SqlTools.GetTableName(table, "t", param);
                }
                private BatchQuery(string table, IDAL myDAL)
                {
                        this._MyDAL = myDAL;
                        this._BatchExcel = new Batch.BatchSql();
                        this.TableName = table;
                        this.Config = new SqlBatchConfig(table, "t");
                }
                private string _OrderBy = null;

                private string[] _Group = null;

                private ISqlWhere[] _Having = null;

                private int _TopNum = -1;
                protected readonly string _Name = "";
                public ISqlRunConfig Config { get; }

                public string TableName { get; }

                public SqlTableColumn[] Column
                {
                        get => this._BatchExcel.Column;
                        set => this._BatchExcel.Column = value;
                }

                public int RowCount => this._BatchExcel.RowCount;
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
                        else if (this._Group != null)
                        {
                                SqlTools.InitGroup(sql, this._OrderBy, this._Group, this._Having, this.Config, param);
                        }
                }
                private string _GetSqlBody(List<IDataParameter> param)
                {
                        StringBuilder main = new StringBuilder(128);
                        main.AppendFormat(" from {0},{1} where ", this._Name, this._BatchExcel.TableName);
                        SqlTools.InitWhereColumn(this._BatchExcel, main, this.Config);
                        if (!this._Where.IsNull())
                        {
                                SqlTools.AppendWhere(main, this.Config, this._Where, param);
                        }
                        return main.ToString();
                }
                private string _GeneratePagingSql(List<IDataParameter> param)
                {
                        string main = this._GetSqlBody(param);
                        StringBuilder sql = new StringBuilder("select ", main.Length + 256);
                        SqlTools.InitColumn(sql, this._Column, this.Config);
                        if (this._IsCount)
                        {
                                return SqlTools.InitBatchPaging(sql, this._OrderBy, this._Skip, this._Size, this._Column[0].Name, this.Config, main, param);
                        }
                        else
                        {
                                sql.Append(main);
                                SqlTools.InitPaging(sql, this._OrderBy, this._Skip, this._Size, this.Config, param);
                        }
                        return sql.ToString();
                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = this._BatchExcel.GenerateSql(out param);
                        List<IDataParameter> list = new List<IDataParameter>();
                        if (this._IsPaging)
                        {
                                sql.Append(this._GeneratePagingSql(list));
                        }
                        else
                        {
                                sql.Append("select ");
                                SqlTools.InitHead(sql, this._TopNum);
                                SqlTools.InitColumn(sql, this._Column, this.Config);
                                sql.Append(this._GetSqlBody(list));
                                this._InitFoot(sql, list);
                        }
                        param = param.Add(list);
                        return sql;
                }

                public void AddRow(object[] datas)
                {
                        this._BatchExcel.AddRow(datas);
                }
                #region 分组查询
                public bool GroupByOne<T>(string group, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = new SqlColumn[] {
                                new SqlColumn(group),
                        };
                        this._Group = new string[] { group };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Group<T>(string group, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Group<T>(string group, out T data, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return SqlHelper.GetRow(this, this._MyDAL, out data);
                }
                public bool Group<T>(string group, string sortBy, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._OrderBy = sortBy;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Group<T>(string[] group, out T data, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = group;
                        return SqlHelper.GetRow(this, this._MyDAL, out data);
                }
                public bool Group<T>(string[] group, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = group;
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Group<T>(string[] group, string sortBy, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._OrderBy = sortBy;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = group;
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                #endregion

                #region 分组查询(Group by having)

                public bool GroupByOne<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._Column = new SqlColumn[]
                        {
                                new SqlColumn(group)
                        };
                        this._Group = new string[] { group };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Group<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Group<T>(string group, ISqlWhere[] having, out T data, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return SqlHelper.GetRow(this, this._MyDAL, out data);
                }
                public bool Group<T>(string group, string orderBy, ISqlWhere[] having, out T[] data, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._OrderBy = orderBy;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = new string[] { group };
                        return SqlHelper.GetTable(this, this._MyDAL, out data);
                }


                public bool Group<T>(string[] groups, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = groups;
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                public bool Group<T>(string[] groups, string orderBy, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Having = having;
                        this._OrderBy = orderBy;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._Group = groups;
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
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
                        return SqlHelper.ExecuteScalar(this, this._MyDAL, out value);
                }
                public bool ExecuteScalar<T>(string column, string funcType, out T value, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._Column = new SqlColumn[]
                        {
                                new SqlColumn(column, null,funcType)
                        };
                        return SqlHelper.ExecuteScalar(this, this._MyDAL, out value);
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
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
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
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
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
                        return SqlHelper.BatchQuery(this, this._MyDAL, out datas, out count);
                }

                public bool QueryByPaging<T>(string orderBy, int index, int size, out T[] datas, params ISqlWhere[] where)
                {
                        this._Where = where;
                        this._IsPaging = true;
                        this._IsCount = false;
                        this._Column = ClassStructureCache.GetQueryColumn(typeof(T));
                        this._OrderBy = orderBy;
                        this._Skip = (index - 1) * size;
                        this._Size = size;
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
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
                        return SqlHelper.GetTable(this, this._MyDAL, out datas);
                }
                #endregion
                public void Dispose()
                {

                }
        }
}
