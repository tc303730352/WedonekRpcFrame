using SqlExecHelper.Interface;
using SqlExecHelper.Join;
using SqlExecHelper.SqlValue;

namespace SqlExecHelper
{
        public class SqlBasicClass : SqlBasicDAL, System.IDisposable
        {
                private readonly string _TableName = null;
                public SqlBasicClass(string table)
                {
                        this._TableName = table;
                }
                public SqlBasicClass(string table, string conName) : base(conName)
                {
                        this._TableName = table;
                }

                protected IBatchMerge BatchMerge(int row, int col)
                {
                        return SqlExecTool.BatchMerge(this._TableName, this._MyDAL, row, col);
                }
                protected IBatchMerge BatchMerge(string table, int row, int col)
                {
                        return SqlExecTool.BatchMerge(table, this._MyDAL, row, col);
                }

                #region 连表查询

                protected bool JoinScalar<T>(string table, out T value, string column, SqlFuncType funcType, params ISqlWhere[] where) where T : struct
                {
                        return SqlExecTool.JoinScalar(this._TableName, table, this._MyDAL, out value, column, funcType, where);
                }
                protected bool JoinQuery<T>(string table, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(this._TableName, table, this._MyDAL, out datas, where);
                }
                protected bool JoinQuery<T>(string table, string column, out T[] datas, params ISqlWhere[] where)
                {
                        JoinColumn col;
                        if (column.IndexOf(".") != -1)
                        {
                                string[] str = column.Split('.');
                                col = new JoinColumn
                                {
                                        Table = str[0],
                                        Column = str[1]
                                };
                        }
                        else
                        {
                                col = new JoinColumn
                                {
                                        Column = column
                                };
                        }
                        return SqlExecTool.JoinQuery(this._TableName, table, this._MyDAL, col, out datas, where);
                }

                protected bool JoinQuery<T>(string table, IOrderBy[] orderby, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(this._TableName, table, this._MyDAL, orderby, out datas, where);
                }
                protected bool JoinQuery<T>(string table, IOrderBy[] orderBy, int index, int size, out long count, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(this._TableName, table, this._MyDAL, orderBy, index, size, out count, out array, where);
                }
                protected bool JoinQuery<T>(string table, string orderBy, int index, int size, out long count, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(this._TableName, table, this._MyDAL, new IOrderBy[] {
                                new SqlOrderBy(this._TableName,orderBy,true)
                        }, index, size, out count, out array, where);
                }
                protected bool JoinQuery<T>(string table, string orderBy, int index, int size, string column, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(this._TableName, table, this._MyDAL, new IOrderBy[] {
                                new SqlOrderBy(this._TableName,orderBy,true)
                        }, index, size, new JoinColumn
                        {
                                Column = column,
                                Table = this._TableName,
                        }, out array, where);
                }
                protected bool JoinQuery<T>(string table, IOrderBy[] orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(this._TableName, table, this._MyDAL, orderBy, index, size, out array, where);
                }
                #endregion

                #region "插入"

                protected IInsertTable InsertTable(int row, int col)
                {
                        return SqlExecTool.InsertTable(this._TableName, this._MyDAL, row, col);
                }
                protected IInsertTable InsertTable(string tableName, int row, int col)
                {
                        return SqlExecTool.InsertTable(tableName, this._MyDAL, row, col);
                }

                protected IBatchInsert BatchInsert()
                {
                        return SqlExecTool.BatchInsert(this._TableName, this._MyDAL);
                }
                protected IInsertTable InsertTable()
                {
                        return SqlExecTool.InsertTable(this._TableName, this._MyDAL);
                }
                protected bool Insert<T>(T[] adds) where T : class
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, adds);
                }
                protected bool Insert<T>(string tableName, T[] adds) where T : class
                {
                        return SqlExecTool.Insert(tableName, this._MyDAL, adds);
                }
                protected bool Insert<T, Result>(T[] adds, out Result[] results) where T : class
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, adds, out results);
                }
                protected bool Insert<T, Result>(T[] adds, string column, out Result[] results) where T : class
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, adds, column, out results);
                }
                protected bool Insert<T>(T data) where T : class
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, data);
                }
                protected bool Insert<T, Result>(T data, out Result result) where T : class
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, data, out result);
                }
                protected bool Insert<T, Result>(T data, string column, out Result result) where T : class
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, data, column, out result);
                }
                protected bool Insert<T>(T data, out long id) where T : class
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, data, out id);
                }
                protected bool Insert(IInsertSqlValue[] column, out long id)
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, column, out id);
                }
                protected bool Insert(IInsertSqlValue[] column, out int id)
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, column, out id);
                }
                protected bool Insert<T>(T data, out int id) where T : class
                {
                        return SqlExecTool.Insert(this._TableName, this._MyDAL, data, out id);
                }
                #endregion

                #region 删除数据

                protected IBatchDrop BatchDrop(int row, int col)
                {
                        return SqlExecTool.BatchDrop(this._TableName, this._MyDAL, row, col);
                }
                protected int BatchDrop<T>(string column, T[] datas, params ISqlWhere[] param)
                {
                        return SqlExecTool.BatchDrop(this._TableName, this._MyDAL, column, datas, param);
                }
                protected bool BatchDrop<T, Result>(string column, T[] datas, out Result[] results, params ISqlWhere[] param)
                {
                        return SqlExecTool.BatchDrop(this._TableName, this._MyDAL, column, datas, out results, param);
                }
                protected bool BatchDrop<T, Result>(string column, T[] datas, string returnCol, out Result[] results, params ISqlWhere[] param)
                {
                        return SqlExecTool.BatchDrop(this._TableName, this._MyDAL, column, datas, returnCol, out results, param);
                }
                protected int Drop(ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, where);
                }
                protected int Drop<T>(string column, T data)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, column, data);
                }
                protected bool Drop<T, Result>(string column, T data, out Result result)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, column, data, out result);
                }
                protected bool Drop<T, Result>(string column, T data, string returnColumn, out Result result)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, column, data, returnColumn, out result);
                }
                protected bool Drop<T, Result>(string column, T data, string returnColumn, out Result[] result)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, column, data, returnColumn, out result);
                }
                protected bool Drop<T>(out T result, ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, out result, where);
                }
                protected bool Drop<T>(out T[] result, ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, out result, where);
                }
                protected bool Drop<T>(string column, out T result, ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, column, out result, where);
                }
                protected bool Drop<T>(string column, out T[] result, ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this._TableName, this._MyDAL, column, out result, where);
                }
                #endregion

                #region 修改


                protected IBatchUpdate BatchUpdate(int row, int col)
                {
                        return SqlExecTool.BatchUpdate(this._TableName, this._MyDAL, row, col);
                }

                protected bool Update(ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, where);
                }
                protected bool Update(string table, ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, table, this._MyDAL, columns, where);
                }
                protected bool Update(ISqlSetColumn[] columns, out int rowNum, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, out rowNum, where);
                }
                protected bool Update<T>(ISqlSetColumn[] columns, string column, T[] vals, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, column, vals, where);
                }

                protected bool Update<T>(ISqlSetColumn[] columns, string column, T val)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, column, val);
                }
                protected bool Update<Result>(ISqlSetColumn[] columns, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, out result, where);
                }
                protected bool Update<Result>(string table, ISqlSetColumn[] columns, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(table, this._MyDAL, columns, out result, where);
                }
                protected bool Update<Result>(string table, ISqlSetColumn[] columns, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(table, this._MyDAL, columns, prefix, out result, where);
                }
                protected bool Update<Result>(ISqlSetColumn[] columns, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, prefix, out result, where);
                }
                protected bool Update<Result>(ISqlSetColumn[] columns, string column, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, column, out result, where);
                }
                protected bool Update<Result>(string table, ISqlSetColumn[] columns, string column, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(table, this._MyDAL, columns, column, out result, where);
                }
                protected bool Update<Result>(string table, ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(table, this._MyDAL, columns, column, prefix, out result, where);
                }
                protected bool Update<Result>(ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, column, prefix, out result, where);
                }

                protected bool Update<T>(T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, where);
                }
                protected bool Update<T, T1>(T data, string column, T1 val)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, column, val);
                }
                protected bool Update<T>(T data, out int rowNum, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, out rowNum, where);
                }

                protected bool Update<T, Result>(T data, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, out result, where);
                }
                protected bool Update<T, Result>(T data, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, prefix, out result, where);
                }

                protected bool Update<T, Result>(T data, string column, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, column, out result, where);
                }
                protected bool Update<T, Result>(T data, string column, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, column, prefix, out result, where);
                }

                protected bool Update<Result>(ISqlSetColumn[] columns, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, prefix, out result, where);
                }
                protected bool Update<Result>(ISqlSetColumn[] columns, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, out result, where);
                }
                protected bool Update<Result>(ISqlSetColumn[] columns, string column, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update<Result>(this._TableName, this._MyDAL, columns, column, SqlEventPrefix.deleted, out result, where);
                }
                protected bool Update<Result>(ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, columns, column, prefix, out result, where);
                }

                protected bool Update<T, Result>(T data, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, prefix, out result, where);
                }
                protected bool Update<T, Result>(T data, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, out result, where);
                }

                protected bool Update<T, Result>(T data, string column, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, column, out result, where);
                }
                protected bool Update<T, Result>(T data, string column, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this._TableName, this._MyDAL, data, column, prefix, out result, where);
                }
                #endregion

                #region 批量查询
                protected IBatchQuery BatchQuery(int row, int col)
                {
                        return SqlExecTool.BatchQuery(this._TableName, this._MyDAL, row, col);
                }
                #endregion

                #region 表分页查询
                protected bool QueryTable<T>(string table, string orderBy, int index, int size, out T[] array, out long count, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(table, this._MyDAL, orderBy, index, size, out array, out count, where);
                }
                #endregion

                #region 分页查询
                protected bool Query<T>(string orderBy, int index, int size, out T[] array, out long count, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(this._TableName, this._MyDAL, orderBy, index, size, out array, out count, where);
                }
                protected bool Query<T>(string column, string orderBy, int index, int size, out T[] array, out long count, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(this._TableName, this._MyDAL, column, orderBy, index, size, out array, out count, where);
                }

                protected bool Query<T>(string column, string orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(this._TableName, this._MyDAL, column, orderBy, index, size, out array, where);
                }
                protected bool Query<T>(string orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(this._TableName, this._MyDAL, orderBy, index, size, out array, where);
                }
                #endregion

                #region 查询列表

                protected bool Get<T, Result>(string column, T[] ids, string showCol, out Result[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this._TableName, this._MyDAL, column, ids, showCol, out datas, where);
                }
                protected bool Get<T, Result>(string column, T[] ids, out Result[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this._TableName, this._MyDAL, column, ids, out datas, where);
                }


                protected bool Get<T, Result>(string column, T data, out Result[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this._TableName, this._MyDAL, column, data, out datas, where);
                }
                protected bool Get<T>(out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this._TableName, this._MyDAL, out datas, where);
                }
                protected bool Get<T>(out T[] datas, string orderBy, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this._TableName, this._MyDAL, out datas, orderBy, where);
                }
                protected bool Get<T>(string column, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this._TableName, this._MyDAL, column, out datas, where);
                }
                #endregion

                #region TOP查询(select top)
                protected bool GetTop<T>(int top, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetTop(this._TableName, this._MyDAL, top, out datas, where);
                }
                protected bool GetTop<T>(int top, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetTop(this._TableName, this._MyDAL, top, orderBy, out datas, where);
                }
                protected bool GetTop<T>(string column, int top, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetTop(this._TableName, this._MyDAL, column, top, out datas, where);
                }
                protected bool GetTop<T>(int top, string column, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetTop(this._TableName, this._MyDAL, top, column, orderBy, out datas, where);
                }
                #endregion

                #region 查询单行(GetRow)

                protected bool GetRow<T>(out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetRow(this._TableName, this._MyDAL, out data, where);
                }
                protected bool GetRow<T, Result>(string column, T value, out Result data)
                {
                        return SqlExecTool.GetRow(this._TableName, this._MyDAL, column, value, out data);
                }
                protected bool GetRow<T>(out T data, string orderBy, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetRow(this._TableName, this._MyDAL, out data, orderBy, where);
                }
                #endregion

                #region 单列查询(ExecuteScalar)

                protected bool ExecuteScalarTable<T>(string table, string column, out T data, T def, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalarDef(table, this._MyDAL, column, out data, def, where);
                }
                protected bool ExecuteScalarTable<T>(string table, string column, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(table, this._MyDAL, column, out data, where);
                }
                protected bool ExecuteScalar<T>(string column, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this._TableName, this._MyDAL, column, out data, where);
                }
                protected bool ExecuteScalar<T, T1>(string column, out T data, string where, T1 value)
                {
                        return SqlExecTool.ExecuteScalar<T, T1>(this._TableName, this._MyDAL, column, out data, where, value);
                }
                protected bool ExecuteScalar<T>(string column, out T data, string orderBy, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this._TableName, this._MyDAL, column, out data, orderBy, where);
                }
                protected bool ExecuteScalar<T>(string column, SqlFuncType func, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this._TableName, this._MyDAL, column, func, out data, where);
                }
                protected bool ExecuteScalar<T>(string column, string func, out T val, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this._TableName, this._MyDAL, column, func, out val, where);
                }
                protected bool ExecuteCount<T>(out T count, params ISqlWhere[] where) where T : struct
                {
                        return SqlExecTool.ExecuteScalar<T>(this._TableName, this._MyDAL, "*", SqlFuncType.count, out count, where);
                }

                protected bool ExecuteSum<T>(string column, out T sum, params ISqlWhere[] where) where T : struct
                {
                        return SqlExecTool.ExecuteScalar<T>(this._TableName, this._MyDAL, column, SqlFuncType.sum, out sum, where);
                }
                protected bool CheckIsExists(out bool isExists, params ISqlWhere[] where)
                {
                        return SqlExecTool.CheckIsExists(this._TableName, this._MyDAL, out isExists, where);
                }
                #endregion

                #region 分组(Group by)
                protected bool GroupTable<T>(string table, string group, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GroupByOne(table, this._MyDAL, group, out datas, where);
                }
                protected bool GroupByOne<T>(string group, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GroupByOne(this._TableName, this._MyDAL, group, out datas, where);
                }
                protected bool Group<T>(string group, out T[] datas, params ISqlWhere[] where)
                {
                        return this.Group(new string[] { group }, out datas, where);
                }
                protected bool Group<T, Result>(string group, string column, T[] ids, out Result[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, new string[] { group }, column, ids, out datas, where);
                }
                protected bool Group<T>(string group, out T data, params ISqlWhere[] where)
                {
                        return this.Group(new string[] { group }, out data, where);
                }
                protected bool Group<T>(string group, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        return this.Group(new string[] { group }, orderBy, out datas, where);
                }
                protected bool Group<T>(string[] groups, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, groups, out datas, where);
                }
                protected bool Group<T>(string[] groups, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, groups, out data, where);
                }
                protected bool Group<T>(string[] groups, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, groups, orderBy, out datas, where);
                }
                protected bool Group<T>(string[] groups, int index, int size, string orderBy, out T[] datas, out long count, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, groups, index, size, orderBy, out datas, out count, where);
                }
                protected bool Group<T>(string group, int index, int size, string orderBy, out T[] datas, out long count, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, new string[] { group }, index, size, orderBy, out datas, out count, where);
                }
                protected bool GroupByColumn<T>(string group, int index, int size, string orderBy, out T[] datas, out long count, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, group, index, size, orderBy, out datas, out count, where);
                }
                #endregion

                #region 分组查询(Group by having)

                protected bool GroupByOne<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GroupByOne(this._TableName, this._MyDAL, group, having, out datas, where);
                }
                protected bool Group<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, group, having, out datas, where);
                }
                protected bool Group<T>(string group, ISqlWhere[] having, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, group, having, out data, where);
                }
                protected bool Group<T>(string group, string orderBy, ISqlWhere[] having, out T[] data, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, group, orderBy, having, out data, where);
                }
                protected bool Group<T>(string[] groups, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, groups, having, out datas, where);
                }
                protected bool Group<T>(string[] groups, string orderBy, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this._TableName, this._MyDAL, groups, orderBy, having, out datas, where);
                }
                #endregion


                #region 链接查询
                public bool UnionQuery<T>(string column, out T[] datas, UnionQueryParam[] unions, params ISqlWhere[] where)
                {
                        return SqlExecTool.UnionQuery(this._MyDAL, column, out datas, unions.Add(new UnionQueryParam
                        {
                                Table = _TableName,
                                Where = where
                        }));
                }
                public bool UnionQuery<T>(out T[] datas, ISqlWhere[] one, ISqlWhere[] two) where T : class
                {
                        return SqlExecTool.UnionQuery(this._MyDAL, out datas, new UnionQueryParam[] {
                                new UnionQueryParam
                                {
                                        Table = _TableName,
                                        Where = one
                                },new UnionQueryParam
                                {
                                        Table = _TableName,
                                        Where = two
                                }
                        });
                }
                public bool UnionQuery<T>(string column, out T[] datas, ISqlWhere[] one, ISqlWhere[] two)
                {
                        return SqlExecTool.UnionQuery(this._MyDAL, column, out datas, new UnionQueryParam[] {
                                new UnionQueryParam
                                {
                                        Table = _TableName,
                                        Where = one
                                },new UnionQueryParam
                                {
                                        Table = _TableName,
                                        Where = two
                                }
                        });
                }
                #endregion

                public void Dispose()
                {
                        this._MyDAL.Dispose();
                }
        }
}