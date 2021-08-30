using System.Data;

using SqlExecHelper.Cache;
using SqlExecHelper.Config;
using SqlExecHelper.Drop;
using SqlExecHelper.Insert;
using SqlExecHelper.Interface;
using SqlExecHelper.Join;
using SqlExecHelper.Merge;
using SqlExecHelper.Query;
using SqlExecHelper.SqlValue;
using SqlExecHelper.TempTable;
using SqlExecHelper.Update;

using RpcHelper;

namespace SqlExecHelper
{
        public class SqlExecTool
        {
                public static IBatchMerge BatchMerge(string table, IDAL dal, int row, int col)
                {
                        if (row > SqlConfig.TempTableLimitNum || (row * col) > SqlConfig.MxColumnNum)
                        {
                                return new TempTableMerge(table, dal);
                        }
                        else
                        {
                                return new BatchMerge(table, dal);
                        }
                }

                #region 连表查询
                public static bool JoinScalar<T>(string one, string two, IDAL dal, out T value, string column, SqlFuncType funcType, params ISqlWhere[] where)
                {
                        JoinTable[] tables = new JoinTable[]
                        {
                                new JoinTable(two,"b",SqlConfig.QueryLockType)
                        };
                        ISqlBasic query = new SqlJoinQuery(one, SqlConfig.QueryLockType, tables, new JoinColumn
                        {
                                Column = column,
                                Table = one,
                                FuncType = funcType,
                        }, where);
                        return SqlHelper.ExecuteScalar<T>(query, dal, out value);
                }
                public static bool JoinQuery<T>(string one, string two, IDAL dal, out T[] datas, params ISqlWhere[] where)
                {
                        JoinTable[] tables = new JoinTable[]
                        {
                                new JoinTable(two,"b",SqlConfig.QueryLockType)
                        };
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlJoinQuery(one, SqlConfig.QueryLockType, tables, obj, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                public static bool JoinQuery<T>(string one, string two, IDAL dal, JoinColumn column, out T[] datas, params ISqlWhere[] where)
                {
                        JoinTable[] tables = new JoinTable[]
                        {
                                new JoinTable(two,"b",SqlConfig.QueryLockType)
                        };
                        ISqlBasic query = new SqlJoinQuery(one, SqlConfig.QueryLockType, tables, column, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                public static bool JoinQuery<T>(string one, string two, IDAL dal, IOrderBy[] orderby, out T[] datas, params ISqlWhere[] where)
                {
                        JoinTable[] tables = new JoinTable[]
                        {
                                new JoinTable(two,"b",SqlConfig.QueryLockType)
                        };
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlJoinQuery(one, SqlConfig.QueryLockType, tables, obj, orderby, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                public static bool JoinQuery<T>(string one, string two, IDAL dal, IOrderBy[] orderBy, int index, int size, out long count, out T[] array, params ISqlWhere[] where)
                {
                        JoinTable[] tables = new JoinTable[]
                        {
                                new JoinTable(two,"b",SqlConfig.QueryLockType)
                        };
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlJoinPagingQuery(one, SqlConfig.QueryLockType, index, size, tables, obj, orderBy, where);
                        return SqlHelper.Query(query, dal, out array, out count);
                }

                public static bool JoinQuery<T>(string one, string two, IDAL dal, IOrderBy[] orderBy, int index, int size, JoinColumn column, out T[] array, params ISqlWhere[] where)
                {
                        JoinTable[] tables = new JoinTable[]
                        {
                                new JoinTable(two,"b",SqlConfig.QueryLockType)
                        };
                        ISqlBasic query = new SqlJoinPagingQuery(one, SqlConfig.QueryLockType, index, size, true, tables, column, orderBy, where);
                        return SqlHelper.GetTable(query, dal, out array);
                }
                public static bool JoinQuery<T>(string one, string two, IDAL dal, IOrderBy[] orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        JoinTable[] tables = new JoinTable[]
                        {
                                new JoinTable(two,"b",SqlConfig.QueryLockType)
                        };
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlJoinPagingQuery(one, SqlConfig.QueryLockType, index, size, true, tables, obj, orderBy, where);
                        return SqlHelper.GetTable(query, dal, out array);
                }
                #endregion

                #region "插入"

                public static IInsertTable InsertTable(string tableName, IDAL dal, int row, int col)
                {
                        if (row > SqlConfig.TempTableLimitNum || (row * col) > SqlConfig.MxColumnNum)
                        {
                                return new InsertTable(tableName, dal);
                        }
                        else
                        {
                                return new BatchInsert(tableName, dal);
                        }
                }
                public static IBatchInsert BatchInsert(string tableName, IDAL dal)
                {
                        return new BatchInsert(tableName, dal);
                }
                public static IInsertTable InsertTable(string tableName, IDAL dal)
                {
                        return new InsertTable(tableName, dal);
                }
                public static bool Insert<T>(string tableName, IDAL dal, T[] adds) where T : class
                {
                        if (adds.Length == 1)
                        {
                                return Insert(tableName, dal, adds[0]);
                        }
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        TableColumn[] cols = obj.GetTableColumn();
                        IInsertTable table = InsertTable(tableName, dal, adds.Length, cols.Length);
                        table.Column = cols;
                        adds.ForEach(a => table.AddRow(obj.GetRow(a)));
                        return table.Save();
                }
                public static bool Insert<T, Result>(string tableName, IDAL dal, T[] adds, out Result[] results) where T : class
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        IBatchInsert table = BatchInsert(tableName, dal);
                        table.Column = obj.GetTableColumn();
                        adds.ForEach(a => table.AddRow(obj.GetRow(a)));
                        return table.Save(out results);
                }
                public static bool Insert<T, Result>(string tableName, IDAL dal, T[] adds, string column, out Result[] results) where T : class
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        IBatchInsert table = BatchInsert(tableName, dal);
                        table.Column = obj.GetTableColumn();
                        adds.ForEach(a => table.AddRow(obj.GetRow(a)));
                        return table.Save(column, out results);
                }
                public static bool Insert<T>(string tableName, IDAL dal, T data) where T : class
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic insert = new SqlInsert(obj, tableName, data);
                        return SqlHelper.ExecuteNonQuery(insert, dal) != -2;
                }
                public static bool Insert<T, Result>(string tableName, IDAL dal, T data, out Result result) where T : class
                {
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic insert = new SqlInsertOut(obj, tableName, data, res);
                        return SqlHelper.GetRow(insert, dal, out result);
                }
                public static bool Insert<T, Result>(string tableName, IDAL dal, T data, string column, out Result result) where T : class
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic insert = new SqlInsertOut(obj, tableName, data, column);
                        return SqlHelper.ExecuteScalar(insert, dal, out result);
                }
                public static bool Insert<T>(string tableName, IDAL dal, T data, out long id) where T : class
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic insert = new SqlInsertById(obj, tableName, data, SqlDbType.BigInt);
                        return SqlHelper.Insert(insert, dal, out id);
                }
                public static bool Insert(string tableName, IDAL dal, IInsertSqlValue[] column, out long id)
                {
                        ISqlBasic insert = new SqlInsertById(column, tableName, SqlDbType.BigInt);
                        return SqlHelper.Insert(insert, dal, out id);
                }
                public static bool Insert(string tableName, IDAL dal, IInsertSqlValue[] column, out int id)
                {
                        ISqlBasic insert = new SqlInsertById(column, tableName, SqlDbType.BigInt);
                        return SqlHelper.Insert(insert, dal, out id);
                }
                public static bool Insert<T>(string tableName, IDAL dal, T data, out int id) where T : class
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic insert = new SqlInsertById(obj, tableName, data, SqlDbType.BigInt);
                        return SqlHelper.Insert(insert, dal, out id);
                }
                #endregion

                #region 删除数据

                public static IBatchDrop BatchDrop(string tableName, IDAL dal, int row, int col)
                {
                        if (row > SqlConfig.TempTableLimitNum || (row * col) > SqlConfig.MxColumnNum)
                        {
                                return new TempTableDrop(tableName, SqlConfig.DropLockType, dal);
                        }
                        return new BatchDrop(tableName, SqlConfig.DropLockType, dal);
                }
                public static int BatchDrop<T>(string tableName, IDAL dal, string column, T[] datas, params ISqlWhere[] param)
                {
                        using (IBatchDrop drop = BatchDrop(tableName, dal, datas.Length, 1))
                        {
                                drop.Column = new SqlTableColumn[]
                                {
                                new SqlTableColumn(column,SqlToolsHelper.GetSqlDbType(typeof(T)))
                                };
                                datas.ForEach(a => drop.AddRow(new object[] { a }));
                                return drop.Drop(param);
                        }
                }
                public static bool BatchDrop<T, Result>(string tableName, IDAL dal, string column, T[] datas, out Result[] results, params ISqlWhere[] param)
                {
                        using (IBatchDrop drop = BatchDrop(tableName, dal, datas.Length, 1))
                        {
                                drop.Column = new SqlTableColumn[]
                                {
                                new SqlTableColumn(column,SqlToolsHelper.GetSqlDbType(typeof(T)))
                                };
                                datas.ForEach(a => drop.AddRow(new object[] { a }));
                                return drop.Drop(out results, param);
                        }
                }
                public static bool BatchDrop<T, Result>(string tableName, IDAL dal, string column, T[] datas, string returnCol, out Result[] results, params ISqlWhere[] param)
                {
                        using (IBatchDrop drop = BatchDrop(tableName, dal, datas.Length, 1))
                        {
                                drop.Column = new SqlTableColumn[]
                                {
                                new SqlTableColumn(column,SqlToolsHelper.GetSqlDbType(typeof(T)))
                                };
                                datas.ForEach(a => drop.AddRow(new object[] { a }));
                                return drop.Drop(returnCol, out results, param);
                        }
                }
                public static int Drop(string tableName, IDAL dal, ISqlWhere[] where)
                {
                        ISqlBasic drop = new SqlDrop(tableName, SqlConfig.DropLockType, where);
                        return SqlHelper.ExecuteNonQuery(drop, dal);
                }
                public static int Drop<T>(string tableName, IDAL dal, string column, T data)
                {
                        ISqlWhere where = new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = data };
                        ISqlBasic drop = new SqlDrop(tableName, SqlConfig.DropLockType, where);
                        return SqlHelper.ExecuteNonQuery(drop, dal);
                }
                public static bool Drop<T, Result>(string tableName, IDAL dal, string column, T data, out Result result)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlWhere where = new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = data };
                        ISqlBasic drop = new SqlDropOut(tableName, SqlConfig.DropLockType, obj, where);
                        return SqlHelper.GetRow(drop, dal, out result);
                }
                public static bool Drop<T, Result>(string tableName, IDAL dal, string column, T data, string returnColumn, out Result result)
                {
                        ISqlWhere where = new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = data };
                        ISqlBasic drop = new SqlDropOut(tableName, SqlConfig.DropLockType, returnColumn, where);
                        return SqlHelper.ExecuteScalar(drop, dal, out result);
                }
                public static bool Drop<T, Result>(string tableName, IDAL dal, string column, T data, string returnColumn, out Result[] result)
                {
                        ISqlWhere where = new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = data };
                        ISqlBasic drop = new SqlDropOut(tableName, SqlConfig.DropLockType, returnColumn, where);
                        return SqlHelper.GetTable(drop, dal, out result);
                }
                public static bool Drop<T>(string tableName, IDAL dal, out T result, ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic drop = new SqlDropOut(tableName, SqlConfig.DropLockType, obj, where);
                        return SqlHelper.GetRow(drop, dal, out result);
                }
                public static bool Drop<T>(string tableName, IDAL dal, out T[] result, ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic drop = new SqlDropOut(tableName, SqlConfig.DropLockType, obj, where);
                        return SqlHelper.GetTable(drop, dal, out result);
                }
                public static bool Drop<T>(string tableName, IDAL dal, string column, out T result, ISqlWhere[] where)
                {
                        ISqlBasic drop = new SqlDropOut(tableName, SqlConfig.DropLockType, column, where);
                        return SqlHelper.GetRow(drop, dal, out result);
                }
                public static bool Drop<T>(string tableName, IDAL dal, string column, out T[] result, ISqlWhere[] where)
                {
                        ISqlBasic drop = new SqlDropOut(tableName, SqlConfig.DropLockType, column, where);
                        return SqlHelper.GetTable(drop, dal, out result);
                }
                #endregion

                #region 修改


                public static IBatchUpdate BatchUpdate(string tableName, IDAL dal, int row, int col)
                {
                        if (row > SqlConfig.TempTableLimitNum || (row * col) > SqlConfig.MxColumnNum)
                        {
                                return new TempTableUpdate(tableName, SqlConfig.SetLockType, dal);
                        }
                        return new BatchUpdate(tableName, SqlConfig.DropLockType, dal);
                }

                public static bool Update(string tableName, IDAL dal, ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        ISqlBasic update = new SqlUpdate(tableName, SqlConfig.SetLockType, columns, where);
                        return SqlHelper.ExecuteNonQuery(update, dal) != -2;
                }
                public static bool Update(string one, string two, IDAL dal, ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        ISqlBasic sql = new SqlJoinUpdate(one, two, SqlConfig.SetLockType, SqlConfig.QueryLockType, columns, where);
                        return SqlHelper.ExecuteNonQuery(sql, dal) != -2;
                }

                public static bool Update<T>(string tableName, IDAL dal, ISqlSetColumn[] columns, string column, T[] vals, params ISqlWhere[] where)
                {
                        if (vals.Length == 1)
                        {
                                where = where.TopInsert(new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = vals[0] });
                                return Update(tableName, dal, columns, where);
                        }
                        else if (vals.Length <= SqlConfig.InMaxNum)
                        {
                                where = where.TopInsert(new InSqlWhere<T>(column, SqlToolsHelper.GetSqlDbType(typeof(T)), vals));
                                return Update(tableName, dal, columns, where);
                        }
                        else
                        {
                                using (IBatchUpdate batch = BatchUpdate(tableName, dal, vals.Length, 1))
                                {
                                        batch.Column = new SqlTableColumn[]
                                        {
                                     new SqlTableColumn(column, SqlToolsHelper.GetSqlDbType(typeof(T))){ ColType= SqlColType.查询}
                                        };
                                        vals.ForEach(a => batch.AddRow(a));
                                        return batch.Update(columns, where);
                                }
                        }
                }

                public static bool Update<T, Result>(string tableName, IDAL dal, ISqlSetColumn[] columns, string column, T[] vals, string retCol, SqlEventPrefix prefix, out Result[] results, params ISqlWhere[] where)
                {
                        if (vals.Length == 1)
                        {
                                where = where.TopInsert(new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = vals[0] });
                                return Update(tableName, dal, columns, retCol, prefix, out results, where);
                        }
                        else if (vals.Length <= SqlConfig.InMaxNum)
                        {
                                where = where.TopInsert(new InSqlWhere<T>(column, SqlToolsHelper.GetSqlDbType(typeof(T)), vals));
                                return Update(tableName, dal, columns, retCol, prefix, out results, where);
                        }
                        else
                        {
                                using (IBatchUpdate batch = BatchUpdate(tableName, dal, vals.Length, 1))
                                {
                                        batch.Column = new SqlTableColumn[]
                                        {
                                    new SqlTableColumn(column, SqlToolsHelper.GetSqlDbType(typeof(T))){ ColType= SqlColType.查询}
                                        };
                                        vals.ForEach(a => batch.AddRow(a));
                                        return batch.Update(retCol, prefix, out results, columns, where);
                                }
                        }
                }

                public static bool Update<T>(string tableName, IDAL dal, ISqlSetColumn[] columns, string column, T val)
                {
                        ISqlWhere where = new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = val };
                        ISqlBasic update = new SqlUpdate(tableName, SqlConfig.SetLockType, columns, where);
                        return SqlHelper.ExecuteNonQuery(update, dal) != -2;
                }
                public static bool Update<Result>(string tableName, IDAL dal, ISqlSetColumn[] columns, out Result result, params ISqlWhere[] where)
                {
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, columns, res, where);
                        return SqlHelper.GetRow(update, dal, out result);
                }
                public static bool Update<Result>(string one, string two, IDAL dal, ISqlSetColumn[] columns, out Result result, params ISqlWhere[] where)
                {
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic sql = new SqlJoinUpdateOut(one, two, Config.SqlConfig.SetLockType, SqlConfig.QueryLockType, res, SqlEventPrefix.deleted, columns, where);
                        return SqlHelper.GetRow(sql, dal, out result);
                }
                public static bool Update<Result>(string one, string two, IDAL dal, ISqlSetColumn[] columns, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic sql = new SqlJoinUpdateOut(one, two, Config.SqlConfig.SetLockType, SqlConfig.QueryLockType, res, prefix, columns, where);
                        return SqlHelper.GetRow(sql, dal, out result);
                }
                public static bool Update<Result>(string tableName, IDAL dal, ISqlSetColumn[] columns, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, columns, res, prefix, where);
                        return SqlHelper.GetRow(update, dal, out result);
                }
                public static bool Update<Result>(string tableName, IDAL dal, ISqlSetColumn[] columns, string column, out Result result, params ISqlWhere[] where)
                {
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, columns, column, SqlEventPrefix.deleted, where);
                        return SqlHelper.ExecuteScalar(update, dal, out result);
                }
                public static bool Update<Result>(string one, string two, IDAL dal, ISqlSetColumn[] columns, string column, out Result result, params ISqlWhere[] where)
                {
                        ISqlBasic sql = new SqlJoinUpdateOut(one, two, Config.SqlConfig.SetLockType, SqlConfig.QueryLockType, column, SqlEventPrefix.deleted, columns, where);
                        return SqlHelper.GetRow(sql, dal, out result);
                }
                public static bool Update<Result>(string one, string two, IDAL dal, ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        ISqlBasic sql = new SqlJoinUpdateOut(one, two, Config.SqlConfig.SetLockType, SqlConfig.QueryLockType, column, prefix, columns, where);
                        return SqlHelper.GetRow(sql, dal, out result);
                }
                public static bool Update<Result>(string tableName, IDAL dal, ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, columns, column, prefix, where);
                        return SqlHelper.ExecuteScalar(update, dal, out result);
                }

                public static bool Update<T>(string tableName, IDAL dal, T data, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic update = new SqlUpdate(tableName, SqlConfig.SetLockType, obj, data, where);
                        return SqlHelper.ExecuteNonQuery(update, dal) != -2;
                }
                public static bool Update<T, T1>(string tableName, IDAL dal, T data, string column, T1 val)
                {
                        ISqlWhere where = new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T1))) { Value = val };
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic update = new SqlUpdate(tableName, SqlConfig.SetLockType, obj, data, where);
                        return SqlHelper.ExecuteNonQuery(update, dal) != -2;
                }


                public static bool Update<T, Result>(string tableName, IDAL dal, T data, out Result result, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, obj, data, res, where);
                        return SqlHelper.GetRow(update, dal, out result);
                }
                public static bool Update<T, Result>(string tableName, IDAL dal, T data, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, obj, data, res, prefix, where);
                        return SqlHelper.GetRow(update, dal, out result);
                }

                public static bool Update<T, Result>(string tableName, IDAL dal, T data, string column, out Result result, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, obj, data, column, SqlEventPrefix.deleted, where);
                        return SqlHelper.ExecuteScalar(update, dal, out result);
                }



                public static bool Update<T, Result>(string tableName, IDAL dal, T data, string column, SqlEventPrefix prefix, out Result result, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, obj, data, column, prefix, where);
                        return SqlHelper.ExecuteScalar(update, dal, out result);
                }

                public static bool Update<Result>(string tableName, IDAL dal, ISqlSetColumn[] columns, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, columns, res, prefix, where);
                        return SqlHelper.GetTable(update, dal, out result);
                }
                public static bool Update<Result>(string tableName, IDAL dal, ISqlSetColumn[] columns, out Result[] result, params ISqlWhere[] where)
                {
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, columns, res, where);
                        return SqlHelper.GetTable(update, dal, out result);
                }

                public static bool Update<Result>(string tableName, IDAL dal, ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, columns, column, prefix, where);
                        return SqlHelper.GetTable(update, dal, out result);
                }

                public static bool Update<T, Result>(string tableName, IDAL dal, T data, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, obj, data, res, prefix, where);
                        return SqlHelper.GetTable(update, dal, out result);
                }
                public static bool Update<T, Result>(string tableName, IDAL dal, T data, out Result[] result, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ClassStructure res = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, obj, data, res, where);
                        return SqlHelper.GetTable(update, dal, out result);
                }

                public static bool Update<T, Result>(string tableName, IDAL dal, T data, string column, out Result[] result, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, obj, data, column, SqlEventPrefix.deleted, where);
                        return SqlHelper.GetTable(update, dal, out result);
                }
                public static bool Update<T, Result>(string tableName, IDAL dal, T data, string column, SqlEventPrefix prefix, out Result[] result, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic update = new SqlUpdateOut(tableName, SqlConfig.SetLockType, obj, data, column, prefix, where);
                        return SqlHelper.GetTable(update, dal, out result);
                }
                #endregion

                #region 批量查询
                public static IBatchQuery BatchQuery(string tableName, IDAL dal, int row, int col)
                {
                        if (row > SqlConfig.TempTableLimitNum || (row * col) > SqlConfig.MxColumnNum)
                        {
                                return new TempTableQuery(tableName, SqlConfig.QueryLockType, dal);
                        }
                        return new BatchQuery(tableName, SqlConfig.QueryLockType, dal);
                }
                #endregion


                #region 分页查询
                public static bool Query<T>(string tableName, IDAL dal, string orderBy, int index, int size, out T[] array, out long count, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlPagingQuery(tableName, SqlConfig.QueryLockType, index, size, obj, orderBy, false, where);
                        return SqlHelper.Query(query, dal, out array, out count);
                }
                public static bool Query<T>(string tableName, IDAL dal, string column, string orderBy, int index, int size, out T[] array, out long count, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlPagingQuery(tableName, SqlConfig.QueryLockType, index, size, column, orderBy, false, where);
                        return SqlHelper.Query(query, dal, out array, out count);
                }

                public static bool Query<T>(string tableName, IDAL dal, string column, string orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlPagingQuery(tableName, SqlConfig.QueryLockType, index, size, column, orderBy, true, where);
                        return SqlHelper.GetTable(query, dal, out array);
                }
                public static bool Query<T>(string tableName, IDAL dal, string orderBy, int index, int size, out T[] array, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlPagingQuery(tableName, SqlConfig.QueryLockType, index, size, obj, orderBy, true, where);
                        return SqlHelper.GetTable(query, dal, out array);
                }
                #endregion

                #region 查询列表
                private static ISqlWhere[] _GetSqlWhere(ISqlWhere where, ISqlWhere[] param)
                {
                        if (!param.IsNull())
                        {
                                return param.Add(where);
                        }
                        else
                        {
                                return new ISqlWhere[]
                                {
                                     where
                                };
                        }
                }
                public static bool Get<T, Result>(string tableName, IDAL dal, string column, T[] ids, string showCol, out Result[] datas, params ISqlWhere[] param)
                {
                        if (ids.Length == 1)
                        {
                                param = _GetSqlWhere(new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = ids[0] }, param);
                                return Get(tableName, dal, showCol, out datas, param);
                        }
                        else if (ids.Length <= SqlConfig.InMaxNum)
                        {
                                param = _GetSqlWhere(new InSqlWhere<T>(column, SqlToolsHelper.GetSqlDbType(typeof(T)), ids), param);
                                return Get(tableName, dal, showCol, out datas, param);
                        }
                        else
                        {
                                using (IBatchQuery query = BatchQuery(tableName, dal, ids.Length, 1))
                                {
                                        query.Column = new SqlTableColumn[]
                                        {
                                        new SqlTableColumn(column,SqlToolsHelper.GetSqlDbType(typeof(T)))
                                        };
                                        ids.ForEach(a => query.AddRow(new object[] { a }));
                                        return query.Query(showCol, out datas, param);
                                }
                        }
                }
                public static bool Get<T, Result>(string tableName, IDAL dal, string column, T[] ids, out Result[] datas, params ISqlWhere[] param)
                {
                        if (ids.Length == 1)
                        {
                                param = _GetSqlWhere(new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = ids[0] }, param);
                                return Get(tableName, dal, out datas, param);
                        }
                        else if (ids.Length <= SqlConfig.InMaxNum)
                        {
                                param = _GetSqlWhere(new InSqlWhere<T>(column, SqlToolsHelper.GetSqlDbType(typeof(T)), ids), param);
                                return Get(tableName, dal, out datas, param);
                        }
                        else
                        {
                                using (IBatchQuery query = BatchQuery(tableName, dal, ids.Length, 1))
                                {
                                        query.Column = new SqlTableColumn[]
                                        {
                                        new SqlTableColumn(column,SqlToolsHelper.GetSqlDbType(typeof(T)))
                                        };
                                        ids.ForEach(a => query.AddRow(new object[] { a }));
                                        return query.Query(out datas, param);
                                }
                        }
                }


                public static bool Get<T, Result>(string tableName, IDAL dal, string column, T data, out Result[] datas, params ISqlWhere[] param)
                {
                        param = _GetSqlWhere(new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = data }, param);
                        return Get(tableName, dal, out datas, param);
                }

                public static bool Get<T>(string tableName, IDAL dal, out T[] datas, params ISqlWhere[] where)
                {
                        return Get(tableName, dal, out datas, null, where);
                }
                public static bool Get<T>(string tableName, IDAL dal, out T[] datas, string orderBy, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlQuery(tableName, SqlConfig.QueryLockType, obj, orderBy, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }

                public static bool Get<T>(string tableName, IDAL dal, string column, out T[] datas, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlQuery(tableName, SqlConfig.QueryLockType, column, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                #endregion
                #region 链接查询
                internal static bool UnionQuery<T>(IDAL dal, out T[] datas, UnionQueryParam[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic[] querys = where.ConvertAll((a,i) =>
                        {
                                return new SqlQuery(a.Table, SqlConfig.QueryLockType, PublicDataDic.Letter[i], obj, a.Where);
                        });
                        ISqlBasic query = new SqlUnionQuery(querys);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                internal static bool UnionQuery<T>(IDAL dal, string column, out T[] datas, UnionQueryParam[] where)
                {
                        ISqlBasic[] querys = where.ConvertAll(a =>
                        {
                                return new SqlQuery(a.Table, SqlConfig.QueryLockType, column, a.Where);
                        });
                        ISqlBasic query = new SqlUnionQuery(querys);
                        return SqlHelper.GetTable(query, dal, out datas);
                }

                #endregion
                #region TOP查询(select top)
                public static bool GetTop<T>(string tableName, IDAL dal, int top, out T[] datas, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlTopQuery(tableName, SqlConfig.QueryLockType, top, obj, null, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                public static bool GetTop<T>(string tableName, IDAL dal, int top, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlTopQuery(tableName, SqlConfig.QueryLockType, top, obj, orderBy, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                public static bool GetTop<T>(string tableName, IDAL dal, string column, int top, out T[] datas, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlTopQuery(tableName, SqlConfig.QueryLockType, top, column, null, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                public static bool GetTop<T>(string tableName, IDAL dal, int top, string column, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlTopQuery(tableName, SqlConfig.QueryLockType, top, column, orderBy, where);
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                #endregion

                #region 查询单行(GetRow)

                public static bool GetRow<T>(string tableName, IDAL dal, out T data, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlTopQuery(tableName, SqlConfig.QueryLockType, 1, obj, null, where);
                        return SqlHelper.GetRow(query, dal, out data);
                }
                public static bool GetRow<T, Result>(string tableName, IDAL dal, string column, T value, out Result data)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(Result));
                        ISqlWhere where = new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = value };
                        ISqlBasic query = new SqlTopQuery(tableName, SqlConfig.QueryLockType, 1, obj, null, where);
                        return SqlHelper.GetRow(query, dal, out data);
                }
                public static bool GetRow<T>(string tableName, IDAL dal, out T data, string orderBy, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlTopQuery(tableName, SqlConfig.QueryLockType, 1, obj, orderBy, where);
                        return SqlHelper.GetRow(query, dal, out data);
                }
                #endregion

                #region 单列查询(ExecuteScalar)

                public static bool ExecuteScalarDef<T>(string tableName, IDAL dal, string column, out T data, T def, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlScalarQuery(tableName, SqlConfig.QueryLockType, column, where);
                        return SqlHelper.ExecuteScalar(query, dal, out data, def);
                }
                public static bool ExecuteScalar<T>(string tableName, IDAL dal, string column, out T data, ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlScalarQuery(tableName, SqlConfig.QueryLockType, column, where);
                        return SqlHelper.ExecuteScalar(query, dal, out data);
                }

                public static bool ExecuteScalar<T, T1>(string tableName, IDAL dal, string column, out T data, string where, T1 value)
                {
                        ISqlBasic query = new SqlScalarQuery(tableName, SqlConfig.QueryLockType, column, new SqlWhere(where, SqlToolsHelper.GetSqlDbType(typeof(T1))) { Value = value });
                        return SqlHelper.ExecuteScalar(query, dal, out data);
                }
                public static bool ExecuteScalar<T>(string tableName, IDAL dal, string column, out T data, string orderBy, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlScalarQuery(tableName, SqlConfig.QueryLockType, column, orderBy, null, where);
                        return SqlHelper.ExecuteScalar(query, dal, out data);
                }
                public static bool ExecuteScalar<T>(string tableName, IDAL dal, string column, SqlFuncType func, out T data, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlScalarQuery(tableName, SqlConfig.QueryLockType, column, func.ToString(), where);
                        return SqlHelper.ExecuteScalar(query, dal, out data);
                }
                public static bool ExecuteScalar<T>(string tableName, IDAL dal, string column, string func, out T val, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlScalarQuery(tableName, SqlConfig.QueryLockType, column, func, where);
                        return SqlHelper.ExecuteScalar(query, dal, out val);
                }

                public static bool CheckIsExists(string tableName, IDAL dal, out bool isExists, params ISqlWhere[] where)
                {
                        ISqlBasic query = new SqlTopQuery(tableName, SqlConfig.QueryLockType, 1, where);
                        if (!SqlHelper.ExecuteScalar(query, dal, out object res))
                        {
                                isExists = false;
                                return false;
                        }
                        isExists = res != null;
                        return true;
                }
                #endregion

                #region 分组(Group by)

                public static bool GroupByOne<T>(string tableName, IDAL dal, string group, out T[] datas, params ISqlWhere[] where)
                {
                        ISqlBasic sql = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, group, where);
                        return SqlHelper.GetTable(sql, dal, out datas);
                }
                public static bool Group<T, Result>(string tableName, IDAL dal, string[] groups, string column, T[] ids, out Result[] datas, params ISqlWhere[] where)
                {
                        if (ids.Length == 1)
                        {
                                return Group<Result>(tableName, dal, groups, out datas, where.Add(new SqlWhere(column, SqlToolsHelper.GetSqlDbType(typeof(T))) { Value = ids[0] }));
                        }
                        else if (ids.Length <= SqlConfig.InMaxNum)
                        {
                                return Group<Result>(tableName, dal, groups, out datas, where.Add(new InSqlWhere<T>(column, ids)));
                        }
                        else
                        {
                                using (IBatchQuery batch = BatchQuery(tableName, dal, ids.Length, 1))
                                {
                                        batch.Column = new SqlTableColumn[]
                                        {
                                                new SqlTableColumn(column,SqlToolsHelper.GetSqlDbType(typeof(T)))
                                        };
                                        ids.ForEach(a => batch.AddRow(a));
                                        return batch.Group(groups, out datas, where);
                                }
                        }
                }
                public static bool Group<T>(string tableName, IDAL dal, string[] groups, out T[] datas, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic group = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, groups, obj, null, where);
                        return SqlHelper.GetTable(group, dal, out datas);
                }
                public static bool Group<T>(string tableName, IDAL dal, string[] groups, int index, int size, string orderBy, out T[] datas, out long count, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        SqlGroupQuery group = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, groups, obj, null, where);
                        ISqlBasic sql = new SqlGroupPagingQuery(group, index, size, orderBy, false);
                        return SqlHelper.Query(sql, dal, out datas, out count);
                }
                public static bool Group<T>(string tableName, IDAL dal, string group, int index, int size, string orderBy, out T[] datas, out long count, params ISqlWhere[] where)
                {
                        SqlGroupQuery gSql = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, group, where);
                        ISqlBasic sql = new SqlGroupPagingQuery(gSql, index, size, orderBy, false);
                        return SqlHelper.Query(sql, dal, out datas, out count);
                }
                public static bool Group<T>(string tableName, IDAL dal, string[] groups, out T data, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic group = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, groups, obj, null, where);
                        return SqlHelper.GetRow(group, dal, out data);
                }
                public static bool Group<T>(string tableName, IDAL dal, string[] groups, string orderBy, out T[] datas, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic group = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, groups, obj, orderBy, where);
                        return SqlHelper.GetTable(group, dal, out datas);
                }

                #endregion

                #region 分组查询(Group by having)

                public static bool GroupByOne<T>(string tableName, IDAL dal, string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        ISqlBasic sql = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, group, where)
                        {
                                Having = having
                        };
                        return SqlHelper.GetTable(sql, dal, out datas);
                }
                public static bool Group<T>(string tableName, IDAL dal, string group, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, new string[] { group }, obj, null, where)
                        {
                                Having = having
                        };
                        return SqlHelper.GetTable(query, dal, out datas);
                }
                public static bool Group<T>(string tableName, IDAL dal, string group, ISqlWhere[] having, out T data, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, new string[] { group }, obj, null, where)
                        {
                                Having = having
                        };
                        return SqlHelper.GetRow(query, dal, out data);
                }
                public static bool Group<T>(string tableName, IDAL dal, string group, string orderBy, ISqlWhere[] having, out T[] data, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic query = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, new string[] { group }, obj, orderBy, where)
                        {
                                Having = having
                        };
                        return SqlHelper.GetTable(query, dal, out data);
                }
                public static bool Group<T>(string tableName, IDAL dal, string[] groups, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic group = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, groups, obj, null, where)
                        {
                                Having = having
                        };
                        return SqlHelper.GetTable(group, dal, out datas);
                }
                public static bool Group<T>(string tableName, IDAL dal, string[] groups, string orderBy, ISqlWhere[] having, out T[] datas, params ISqlWhere[] where)
                {
                        ClassStructure obj = ClassStructureCache.GetStructure(typeof(T));
                        ISqlBasic group = new SqlGroupQuery(tableName, SqlConfig.QueryLockType, groups, obj, orderBy, where)
                        {
                                Having = having
                        };
                        return SqlHelper.GetTable(group, dal, out datas);
                }
                #endregion



                #region 执行SQL
                private static void _SyncParam(IDataParameter[] parameters, SqlBasicParameter[] param)
                {
                        if (param.IsNull())
                        {
                                return;
                        }
                        parameters.ForEach((a, i) =>
                        {
                                if (a.Direction == ParameterDirection.Output || a.Direction == ParameterDirection.InputOutput)
                                {
                                        param[i].Value = a.Value;
                                }
                        });
                }
                public static IBatchExecSql BatchSql(IDAL dal, int row, int col)
                {
                        if (row > SqlConfig.TempTableLimitNum || (row * col) > SqlConfig.MxColumnNum)
                        {
                                return new BatchTableExecSql(dal);
                        }
                        else
                        {
                                return new Batch.BatchExecSql(dal);
                        }
                }
                public static bool ExecuteNonQuery(string sql, IDAL dal, params SqlBasicParameter[] param)
                {
                        IDataParameter[] parameters = param.ConvertAll(a => a.GetParameter());
                        int rowNum = SqlHelper.ExecuteNonQuery(sql, dal, parameters);
                        _SyncParam(parameters, param);
                        return rowNum != -2;
                }

                public static bool ExecuteScalar<T>(string sql, IDAL dal, out T value, params SqlBasicParameter[] param)
                {
                        IDataParameter[] parameters = param.ConvertAll(a => a.GetParameter());
                        if (dal.ExecuteScalar(sql, out object res, parameters))
                        {
                                _SyncParam(parameters, param);
                                value = ModelHelper.GetValue<T>(res);
                                return true;
                        }
                        value = default;
                        return false;
                }
                public static bool GetRow<T>(string sql, IDAL dal, out T data, params SqlBasicParameter[] param)
                {
                        IDataParameter[] parameters = param.ConvertAll(a => a.GetParameter());
                        if (dal.GetDataRow(sql, out DataRow row, parameters))
                        {
                                _SyncParam(parameters, param);
                                data = ModelHelper.GetModelByRow<T>(row);
                                return true;
                        }
                        data = default;
                        return false;
                }
                public static bool GetTable<T>(string sql, IDAL dal, out T[] datas, params SqlBasicParameter[] param)
                {
                        IDataParameter[] parameters = param.ConvertAll(a => a.GetParameter());
                        if (dal.GetDataTable(sql, out DataTable table, parameters))
                        {
                                _SyncParam(parameters, param);
                                datas = ModelHelper.GetModelByTable<T>(table);
                                return true;
                        }
                        datas = null;
                        return false;
                }
                public static bool GetDataSet(string sql, IDAL dal, out DataSet dataSet, params SqlBasicParameter[] param)
                {
                        IDataParameter[] parameters = param.ConvertAll(a => a.GetParameter());
                        return dal.GetDataSet(sql, out dataSet, parameters);
                }
                #endregion
        }
}