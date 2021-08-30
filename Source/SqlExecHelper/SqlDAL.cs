using SqlExecHelper.Interface;
using SqlExecHelper.SqlValue;

namespace SqlExecHelper
{
        public class SqlDAL : SqlBasicDAL, ISqlDAL
        {
                public string TableName
                {
                        get;
                        set;
                }
                public int RowNum => SqlHelper.RowNum;
                public SqlDAL(string table)
                {
                        this.TableName = table;
                }
                public SqlDAL(string table, string conName) : base(conName)
                {
                        this.TableName = table;
                }

                public IBatchMerge BatchMerge(int row, int col)
                {
                        return SqlExecTool.BatchMerge(this.TableName, this._MyDAL, row, col);
                }

                #region 连表查询
                public bool JoinQuery<T>(string one, string two, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(one, two, this._MyDAL, out datas, where);
                }
                public bool JoinQuery<T>(string one, string two, IOrderBy[] orderby, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(one, two, this._MyDAL, orderby, out datas, where);
                }
                public bool JoinQuery<T>(string one, string two, IOrderBy[] orderBy, int index, int size, out long count, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(one, two, this._MyDAL, orderBy, index, size, out count, out array, where);
                }


                public bool JoinQuery<T>(string one, string two, IOrderBy[] orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.JoinQuery(one, two, this._MyDAL, orderBy, index, size, out array, where);
                }
                #endregion

                #region "插入"

                public IInsertTable InsertTable(int row, int col)
                {
                        return SqlExecTool.InsertTable(this.TableName, this._MyDAL, row, col);
                }


                public IBatchInsert BatchInsert()
                {
                        return SqlExecTool.BatchInsert(this.TableName, this._MyDAL);
                }
                public IInsertTable InsertTable()
                {
                        return SqlExecTool.InsertTable(this.TableName, this._MyDAL);
                }
                public bool Insert<T>(T[] adds) where T : class
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, adds);
                }
                public bool Insert<T, Result>(T[] adds, out Result[] results) where T : class
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, adds, out results);
                }
                public bool Insert<T, Result>(T[] adds, string column, out Result[] results) where T : class
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, adds, column, out results);
                }
                public bool Insert<T>(T data) where T : class
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, data);
                }
                public bool Insert<T, Result>(T data, out Result result) where T : class
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, data, out result);
                }
                public bool Insert<T, Result>(T data, string column, out Result result) where T : class
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, data, column, out result);
                }
                public bool Insert<T>(T data, out long id) where T : class
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, data, out id);
                }
                public bool Insert(IInsertSqlValue[] column, out long id)
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, column, out id);
                }
                public bool Insert(IInsertSqlValue[] column, out int id)
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, column, out id);
                }
                public bool Insert<T>(T data, out int id) where T : class
                {
                        return SqlExecTool.Insert(this.TableName, this._MyDAL, data, out id);
                }
                #endregion

                #region 删除数据

                public IBatchDrop BatchDrop(int row, int col)
                {
                        return SqlExecTool.BatchDrop(this.TableName, this._MyDAL, row, col);
                }
                public int BatchDrop<T>(string column, T[] datas, params ISqlWhere[] param)
                {
                        return SqlExecTool.BatchDrop(this.TableName, this._MyDAL, column, datas, param);
                }
                public bool BatchDrop<T, Result>(string column, T[] datas, out Result[] results, params ISqlWhere[] param)
                {
                        return SqlExecTool.BatchDrop(this.TableName, this._MyDAL, column, datas, out results, param);
                }
                public bool BatchDrop<T, Result>(string column, T[] datas, string returnCol, out Result[] results, params ISqlWhere[] param)
                {
                        return SqlExecTool.BatchDrop(this.TableName, this._MyDAL, column, datas, returnCol, out results, param);
                }
                public int Drop(ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, where);
                }
                public int Drop<T>(string column, T data)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, column, data);
                }
                public bool Drop<T, Result>(string column, T data, out Result result)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, column, data, out result);
                }
                public bool Drop<T, Result>(string column, T data, string returnColumn, out Result result)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, column, data, returnColumn, out result);
                }
                public bool Drop<T, Result>(string column, T data, string returnColumn, out Result[] result)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, column, data, returnColumn, out result);
                }
                public bool Drop<T>(out T result, ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, out result, where);
                }
                public bool Drop<T>(out T[] result, ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, out result, where);
                }
                public bool Drop<T>(string column, out T result, ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, column, out result, where);
                }
                public bool Drop<T>(string column, out T[] result, ISqlWhere[] where)
                {
                        return SqlExecTool.Drop(this.TableName, this._MyDAL, column, out result, where);
                }
                #endregion

                #region 修改


                public IBatchUpdate BatchUpdate(int row, int col)
                {
                        return SqlExecTool.BatchUpdate(this.TableName, this._MyDAL, row, col);
                }

                public bool Update(ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, where);
                }

                public bool Update(ISqlSetColumn[] columns, out int rowNum, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, out rowNum, where);
                }
                public bool Update<T>(ISqlSetColumn[] columns, string column, T[] vals, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, column, vals, where);
                }
                public bool Update<T, Result>(ISqlSetColumn[] columns, string column, T[] vals, string retCol, out Result[] results, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, column, vals, retCol, SqlEventPrefix.deleted, out results, where);
                }
                public bool Update<T, Result>(ISqlSetColumn[] columns, string column, T[] vals, string retCol, SqlEventPrefix prefix, out Result[] results, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, column, vals, retCol, prefix, out results, where);
                }

                public bool Update<T>(ISqlSetColumn[] columns, string column, T val)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, column, val);
                }
                public bool Update<Result>(ISqlSetColumn[] columns, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, out result, where);
                }

                public bool Update<Result>(ISqlSetColumn[] columns, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, prefix, out result, where);
                }
                public bool Update<Result>(ISqlSetColumn[] columns, string column, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, column, out result, where);
                }


                public bool Update<Result>(ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, column, prefix, out result, where);
                }

                public bool Update<T>(T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, where);
                }
                public bool Update<T, T1>(T data, string column, T1 val)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, column, val);
                }
                public bool Update<T>(T data, out int rowNum, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, out rowNum, where);
                }

                public bool Update<T, Result>(T data, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, out result, where);
                }
                public bool Update<T, Result>(T data, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, prefix, out result, where);
                }

                public bool Update<T, Result>(T data, string column, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, column, out result, where);
                }
                public bool Update<T, Result>(T data, string column, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, column, prefix, out result, where);
                }

                public bool Update<Result>(ISqlSetColumn[] columns, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, prefix, out result, where);
                }
                public bool Update<Result>(ISqlSetColumn[] columns, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, out result, where);
                }
                public bool Update<Result>(ISqlSetColumn[] columns, string column, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update<Result>(this.TableName, this._MyDAL, columns, column, SqlEventPrefix.deleted, out result, where);
                }
                public bool Update<Result>(ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, columns, column, prefix, out result, where);
                }

                public bool Update<T, Result>(T data, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, prefix, out result, where);
                }
                public bool Update<T, Result>(T data, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, out result, where);
                }

                public bool Update<T, Result>(T data, string column, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, column, out result, where);
                }
                public bool Update<T, Result>(T data, string column, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        return SqlExecTool.Update(this.TableName, this._MyDAL, data, column, prefix, out result, where);
                }
                #endregion

                #region 批量查询
                public IBatchQuery BatchQuery(int row, int col)
                {
                        return SqlExecTool.BatchQuery(this.TableName, this._MyDAL, row, col);
                }
                #endregion



                #region 分页查询
                public bool Query<T>(string orderBy, int index, int size, out T[] array, out long count, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(this.TableName, this._MyDAL, orderBy, index, size, out array, out count, where);
                }
                public bool Query<T>(string column, string orderBy, int index, int size, out T[] array, out long count, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(this.TableName, this._MyDAL, column, orderBy, index, size, out array, out count, where);
                }

                public bool Query<T>(string column, string orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(this.TableName, this._MyDAL, column, orderBy, index, size, out array, where);
                }
                public bool Query<T>(string orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        return SqlExecTool.Query(this.TableName, this._MyDAL, orderBy, index, size, out array, where);
                }
                #endregion

                #region 查询列表

                public bool Get<T, Result>(string column, T[] ids, string showCol, out Result[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this.TableName, this._MyDAL, column, ids, showCol, out datas, where);
                }
                public bool Get<T, Result>(string column, T[] ids, out Result[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this.TableName, this._MyDAL, column, ids, out datas, where);
                }


                public bool Get<T, Result>(string column, T data, out Result[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this.TableName, this._MyDAL, column, data, out datas, where);
                }
                public bool Get<T>(out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this.TableName, this._MyDAL, out datas, where);
                }
                public bool Get<T>(out T[] datas, string orderBy, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this.TableName, this._MyDAL, out datas, orderBy, where);
                }
                public bool Get<T>(string column, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Get(this.TableName, this._MyDAL, column, out datas, where);
                }
                #endregion

                #region TOP查询(select top)
                public bool GetTop<T>(int top, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetTop(this.TableName, this._MyDAL, top, out datas, where);
                }
                public bool GetTop<T>(int top, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetTop(this.TableName, this._MyDAL, top, orderBy, out datas, where);
                }
                public bool GetTop<T>(string column, int top, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetTop(this.TableName, this._MyDAL, column, top, out datas, where);
                }
                public bool GetTop<T>(int top, string column, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetTop(this.TableName, this._MyDAL, top, column, orderBy, out datas, where);
                }
                #endregion

                #region 查询单行(GetRow)

                public bool GetRow<T>(out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetRow(this.TableName, this._MyDAL, out data, where);
                }
                public bool GetRow<T, Result>(string column, T value, out Result data)
                {
                        return SqlExecTool.GetRow(this.TableName, this._MyDAL, column, value, out data);
                }
                public bool GetRow<T>(out T data, string orderBy, params ISqlWhere[] where)
                {
                        return SqlExecTool.GetRow(this.TableName, this._MyDAL, out data, orderBy, where);
                }
                #endregion

                #region 单列查询(ExecuteScalar)

                public bool ExecuteScalarTable<T>(string column, out T data, T def, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalarDef(this.TableName, this._MyDAL, column, out data, def, where);
                }
                public bool ExecuteScalarTable<T>(string column, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this.TableName, this._MyDAL, column, out data, where);
                }
                public bool ExecuteScalar<T>(string column, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this.TableName, this._MyDAL, column, out data, where);
                }
                public bool ExecuteScalar<T, T1>(string column, out T data, string where, T1 value)
                {
                        return SqlExecTool.ExecuteScalar<T, T1>(this.TableName, this._MyDAL, column, out data, where, value);
                }
                public bool ExecuteScalar<T>(string column, out T data, string orderBy, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this.TableName, this._MyDAL, column, out data, orderBy, where);
                }
                public bool ExecuteScalar<T>(string column, SqlFuncType func, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this.TableName, this._MyDAL, column, func, out data, where);
                }
                public bool ExecuteScalar<T>(string column, string func, out T val, params ISqlWhere[] where)
                {
                        return SqlExecTool.ExecuteScalar<T>(this.TableName, this._MyDAL, column, func, out val, where);
                }
                public bool ExecuteCount<T>(out T count, params ISqlWhere[] where) where T : struct
                {
                        return SqlExecTool.ExecuteScalar<T>(this.TableName, this._MyDAL, "*", SqlFuncType.count, out count, where);
                }

                public bool ExecuteSum<T>(string column, out T sum, params ISqlWhere[] where) where T : struct
                {
                        return SqlExecTool.ExecuteScalar<T>(this.TableName, this._MyDAL, column, SqlFuncType.sum, out sum, where);
                }
                public bool CheckIsExists(out bool isExists, params ISqlWhere[] where)
                {
                        return SqlExecTool.CheckIsExists(this.TableName, this._MyDAL, out isExists, where);
                }
                #endregion

                #region 分组(Group by)

                public bool GroupByOne<T>(string group, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GroupByOne(this.TableName, this._MyDAL, group, out datas, where);
                }
                public bool Group<T>(string group, out T[] datas, params ISqlWhere[] where)
                {
                        return this.Group(new string[] { group }, out datas, where);
                }
                public bool Group<T>(string group, out T data, params ISqlWhere[] where)
                {
                        return this.Group(new string[] { group }, out data, where);
                }
                public bool Group<T>(string group, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        return this.Group(new string[] { group }, orderBy, out datas, where);
                }
                public bool Group<T>(string[] groups, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this.TableName, this._MyDAL, groups, out datas, where);
                }
                public bool Group<T>(string[] groups, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this.TableName, this._MyDAL, groups, out data, where);
                }
                public bool Group<T>(string[] groups, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this.TableName, this._MyDAL, groups, orderBy, out datas, where);
                }

                #endregion

                #region 分组查询(Group by having)

                public bool GroupByOne<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.GroupByOne(this.TableName, this._MyDAL, group, having, out datas, where);
                }
                public bool Group<T>(string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this.TableName, this._MyDAL, group, having, out datas, where);
                }
                public bool Group<T>(string group, ISqlWhere[] having, out T data, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this.TableName, this._MyDAL, group, having, out data, where);
                }
                public bool Group<T>(string group, string orderBy, ISqlWhere[] having, out T[] data, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this.TableName, this._MyDAL, group, orderBy, having, out data, where);
                }
                public bool Group<T>(string[] groups, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this.TableName, this._MyDAL, groups, having, out datas, where);
                }
                public bool Group<T>(string[] groups, string orderBy, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        return SqlExecTool.Group(this.TableName, this._MyDAL, groups, orderBy, having, out datas, where);
                }
                #endregion


                public void Dispose()
                {
                        this._MyDAL.Dispose();
                }
        }
}
