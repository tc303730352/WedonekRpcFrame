using System;
using System.Data;
using System.Text;

namespace SqlExecHelper
{
        internal class SqlHelper
        {
                [ThreadStatic]
                public static int RowNum = 0;
                public static bool GetTable<T>(ISqlBasic sql, IDAL myDAL, out T[] datas)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (!myDAL.GetDataTable(str.ToString(), out DataTable table, param))
                        {
                                datas = null;
                                return false;
                        }
                        datas = ModelHelper.GetModelByTable<T>(table);
                        return true;
                }
                public static int ExecuteNonQuery(ISqlBasic sql, IDAL myDAL)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        SqlHelper.RowNum = myDAL.ExecuteNonQuery(str.ToString(), param);
                        return SqlHelper.RowNum;
                }
                public static int ExecuteNonQuery(string sql, IDAL myDAL, IDataParameter[] param)
                {
                        SqlHelper.RowNum = myDAL.ExecuteNonQuery(sql, param);
                        return SqlHelper.RowNum;
                }

                public static bool GetRow<T>(ISqlBasic sql, IDAL myDAL, out T data)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (!myDAL.GetDataRow(str.ToString(), out DataRow row, param))
                        {
                                data = default;
                                return false;
                        }
                        data = ModelHelper.GetModelByRow<T>(row);
                        return true;
                }

                #region 单条插入
                public static bool Insert(ISqlBasic sql, IDAL myDAL, out long id)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (myDAL.ExecuteNonQuery(str.ToString(), param) > 0)
                        {
                                id = Convert.ToInt64(param[param.Length - 1].Value);
                                return true;
                        }
                        id = 0;
                        return false;
                }

                public static bool Insert(ISqlBasic sql, IDAL myDAL, out int id)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (myDAL.ExecuteNonQuery(str.ToString(), param) > 0)
                        {
                                id = Convert.ToInt32(param[param.Length - 1].Value);
                                return true;
                        }
                        id = 0;
                        return false;
                }
                #endregion

                #region 批量插入

                public static bool BatchInsert(ISqlBasic sql, IDAL myDAL, out int[] ids)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (!myDAL.GetDataTable(str.ToString(), out DataTable table, param))
                        {
                                ids = null;
                                return false;
                        }
                        ids = new int[table.Rows.Count];
                        for (int i = 0; i < ids.Length; i++)
                        {
                                ids[i] = Convert.ToInt32(table.Rows[i]);
                        }
                        return true;
                }
                public static bool BatchInsert(ISqlBasic sql, IDAL myDAL, out long[] ids)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (!myDAL.GetDataTable(str.ToString(), out DataTable table, param))
                        {
                                ids = null;
                                return false;
                        }
                        ids = new long[table.Rows.Count];
                        for (int i = 0; i < ids.Length; i++)
                        {
                                ids[i] = Convert.ToInt64(table.Rows[i]);
                        }
                        return true;
                }
                #endregion
                public static bool Query<T>(ISqlBasic sql, IDAL myDAL, out T[] array, out long count)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (!myDAL.GetDataTable(str.ToString(), out DataTable table, param))
                        {
                                count = 0;
                                array = null;
                                return false;
                        }
                        array = ModelHelper.GetModelByTable<T>(table);
                        object val = param[param.Length - 1].Value;
                        count = val == DBNull.Value ? 0 : Convert.ToInt64(val);
                        return true;
                }
                public static bool ExecuteScalar(ISqlBasic sql, IDAL myDAL, out object data)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        return myDAL.ExecuteScalar(str.ToString(), out data, param);
                }
                public static bool ExecuteScalar<T>(ISqlBasic sql, IDAL myDAL, out T data)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (!myDAL.ExecuteScalar(str.ToString(), out object val, param))
                        {
                                data = default;
                                return false;
                        }
                        else
                        {
                                data = ModelHelper.GetValue<T>(val);
                                return true;
                        }
                }
                public static bool ExecuteScalar<T>(ISqlBasic sql, IDAL myDAL, out T data, T def)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (!myDAL.ExecuteScalar(str.ToString(), out object val, param))
                        {
                                data = default;
                                return false;
                        }
                        else if (val == null)
                        {
                                data = def;
                                return true;
                        }
                        else
                        {
                                data = ModelHelper.GetValue<T>(val);
                                return true;
                        }
                }

                internal static bool BatchQuery<T>(ISqlBasic sql, IDAL myDAL, out T[] array, out long count)
                {
                        StringBuilder str = sql.GenerateSql(out IDataParameter[] param);
                        if (!myDAL.GetDataTable(str.ToString(), out DataTable table, param))
                        {
                                count = 0;
                                array = null;
                                return false;
                        }
                        count = Convert.ToInt64(table.Rows[0][0]);
                        if (count == 0 || !Convert.ToBoolean(table.Rows[0][1]))
                        {
                                array = new T[0];
                                return true;
                        }
                        array = ModelHelper.GetModelByTable<T>(table);
                        return true;
                }
        }
}
