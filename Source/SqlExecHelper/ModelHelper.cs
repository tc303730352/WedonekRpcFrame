using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SqlExecHelper
{
        public class ModelHelper
        {
                public static T GetNum<T>(object res)
                {
                        return res != null && DBNull.Value != res ? (T)Convert.ChangeType(res, typeof(T)) : default;
                }
                public static T GetValue<T>(object res)
                {
                        if (res != null && DBNull.Value != res)
                        {
                                return (T)SqlToolsHelper.GetValue(typeof(T), res);
                        }
                        else
                        {
                                return default;
                        }
                }

                public static T GetModelByRow<T>(DataRow row, int colIndex)
                {
                        if (row == null || row.IsNull(colIndex))
                        {
                                return default;
                        }
                        Type type = typeof(T);
                        if (!type.IsClass || type.FullName == SqlToolsHelper.StrType.FullName)
                        {
                                if (type.IsEnum)
                                {
                                        return (T)Enum.ToObject(type, row[colIndex]);
                                }
                                else if (type.FullName == SqlToolsHelper.DateTimeType.FullName)
                                {
                                        object res = SqlToolsHelper.FormatTime(row[colIndex]);
                                        return (T)res;
                                }
                                return (T)row[colIndex];
                        }
                        else
                        {
                                object val = row[colIndex];
                                return SqlToolsHelper.Json<T>(Convert.ToString(val));
                        }
                }

                public static T GetModelByRow<T>(DataRow row)
                {
                        if (row == null)
                        {
                                return default;
                        }
                        Type type = typeof(T);
                        string name = type.FullName;
                        if (!type.IsClass && type.IsPrimitive)
                        {
                                return (T)SqlToolsHelper.GetValue(type, row[0]);
                        }
                        else if (name == SqlToolsHelper.StrType.FullName)
                        {
                                object obj = row[0].ToString();
                                return (T)obj;
                        }
                        else if (name == SqlToolsHelper.UriType.FullName)
                        {
                                object obj = new Uri(row[0].ToString());
                                return (T)obj;
                        }
                        else if (name == SqlToolsHelper.GuidType.FullName)
                        {
                                object obj = row[0];
                                if (obj == null || obj == DBNull.Value)
                                {
                                        obj = Guid.Empty;
                                        return (T)obj;
                                }
                                else
                                {
                                        obj = new Guid(obj.ToString());
                                }
                                return (T)obj;
                        }
                        else
                        {
                                object obj = Activator.CreateInstance(type);
                                foreach (PropertyInfo i in type.GetProperties())
                                {
                                        if (row.Table.Columns.Contains(i.Name) && !row.IsNull(i.Name))
                                        {
                                                object res = SqlToolsHelper.GetValue(i.PropertyType, row[i.Name]);
                                                if (res != null)
                                                {
                                                        i.SetValue(obj, res, null);
                                                }
                                        }
                                }
                                return (T)obj;
                        }
                }
                public static T GetModelByDataReader<T>(SqlDataReader ready)
                {
                        if (ready == null)
                        {
                                return default;
                        }
                        Type type = typeof(T);
                        if (!type.IsClass || type.FullName == SqlToolsHelper.StrType.FullName)
                        {
                                return (T)ready[0];
                        }
                        else
                        {
                                object obj = Activator.CreateInstance(type);
                                foreach (PropertyInfo i in type.GetProperties())
                                {
                                        int index = ready.GetOrdinal(i.Name);
                                        if (index != -1 && !ready.IsDBNull(index))
                                        {
                                                object res = SqlToolsHelper.GetValue(i.PropertyType, ready[index]);
                                                if (res != null)
                                                {
                                                        i.SetValue(obj, res, null);
                                                }
                                        }
                                }
                                return (T)obj;
                        }
                }


                public static T[] GetModelByTable<T>(DataTable table)
                {
                        if (table == null || table.Rows.Count == 0)
                        {
                                return new T[0];
                        }
                        T[] list = new T[table.Rows.Count];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                                DataRow row = table.Rows[i];
                                list[i] = GetModelByRow<T>(row);
                        }
                        return list;
                }
                public static T[] GetModelByTable<T>(DataTable table, int colIndex)
                {
                        if (table == null)
                        {
                                return null;
                        }
                        T[] list = new T[table.Rows.Count];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                                DataRow row = table.Rows[i];
                                list[i] = GetModelByRow<T>(row, colIndex);
                        }
                        return list;
                }

        }
}
