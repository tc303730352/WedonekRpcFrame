using System;
using System.Data;

using Newtonsoft.Json;

namespace SqlExecHelper
{
        internal class SqlToolsHelper
        {
                private static readonly JsonSerializerSettings _Settling = new JsonSerializerSettings
                {
                        DateParseHandling = DateParseHandling.DateTime,
                        DateTimeZoneHandling = DateTimeZoneHandling.Local,
                        NullValueHandling = NullValueHandling.Ignore,

                };
                private static readonly DateTime _DtStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                private static readonly DateTime _SqlMinTime = new DateTime(1900, 1, 1);
                public static readonly Type StrType = typeof(string);
                private static readonly Type _ByteArrayType = typeof(byte[]);
                private static readonly Type _LongType = typeof(long);
                private static readonly Type _IntType = typeof(int);
                private static readonly Type _ShortType = typeof(short);
                private static readonly Type _ObjectType = typeof(object);
                private static readonly Type _CharType = typeof(char);
                private static readonly Type _ByteType = typeof(byte);
                public static readonly Type UriType = typeof(Uri);
                private static readonly Type _boolType = typeof(bool);
                public static readonly Type GuidType = typeof(Guid);
                public static readonly Type DateTimeType = typeof(DateTime);

                public static object Json(string str, Type type)
                {
                        return JsonConvert.DeserializeObject(str, type, _Settling);
                }

                /// <summary>
                /// 链接符号
                /// </summary>
                private static readonly string[] _SymbolStr = new string[]
                {
                       "=",
                       ">",
                       "<",
                       ">=",
                       "<=",
                       "!="
                };


                /// <summary>
                /// 将对象JSON
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public static string Json(object obj)
                {
                        return JsonConvert.SerializeObject(obj, _Settling);
                }
                public static DateTime GetTimeStamp(long timeStamp)
                {
                        return _DtStart.AddSeconds(timeStamp).ToLocalTime();
                }
                /// <summary>
                /// 获取指定时间的时间戳
                /// </summary>
                /// <param name="time"></param>
                /// <returns></returns>
                public static long GetTimeStamp(DateTime time)
                {
                        if (time == DateTime.MinValue)
                        {
                                return 0;
                        }
                        TimeSpan ts = time.ToUniversalTime() - _DtStart;
                        return (long)ts.TotalSeconds;
                }
                public static object FormatValue(SqlDbType dbType, object value, int size)
                {
                        value = FormatValue(dbType, value);
                        if (size != 0 && (dbType == SqlDbType.NVarChar || dbType == SqlDbType.VarChar))
                        {
                                string str = (string)value;
                                if (str.Length > size)
                                {
                                        return str.Substring(0, size);
                                }
                        }
                        return value;
                }
                public static object FormatValue(SqlDbType dbType, object value)
                {
                        if (value == null)
                        {
                                return _GetDefValue(dbType);
                        }
                        else
                        {
                                Type type = value.GetType();
                                if (type.IsGenericType && type.Name == "Nullable`1")
                                {
                                        type = type.GenericTypeArguments[0];
                                }
                                if (type.IsEnum)
                                {
                                        return (int)value;
                                }
                                else if (type.IsArray || type.IsGenericType)
                                {
                                        return Json(value);
                                }
                                else if (dbType == SqlDbType.BigInt && type.FullName == DateTimeType.FullName)
                                {
                                        return GetTimeStamp((DateTime)value);
                                }
                                else if (type.FullName == _LongType.FullName && (dbType == SqlDbType.DateTime || dbType == SqlDbType.Date || dbType == SqlDbType.SmallDateTime))
                                {
                                        return GetTimeStamp((long)value);
                                }
                                else if (type.FullName == DateTimeType.FullName && (dbType == SqlDbType.DateTime || dbType == SqlDbType.Date || dbType == SqlDbType.SmallDateTime))
                                {
                                        DateTime time = (DateTime)value;
                                        return time == DateTime.MinValue ? _SqlMinTime : time;
                                }
                                else if (type.FullName == _boolType.FullName)
                                {
                                        return (bool)value ? 1 : 0;
                                }
                                else if (type.FullName == UriType.FullName)
                                {
                                        return value.ToString();
                                }
                                else if (type.IsClass && type.FullName != StrType.FullName)
                                {
                                        return Json(value);
                                }
                                else
                                {
                                        return value;
                                }
                        }
                }
                private static object _GetDefValue(SqlDbType dbType)
                {
                        if (dbType == SqlDbType.UniqueIdentifier)
                        {
                                return Guid.Empty;
                        }
                        else if (dbType == SqlDbType.Binary)
                        {
                                return new byte[0];
                        }
                        else if (dbType == SqlDbType.Char || dbType == SqlDbType.NVarChar || dbType == SqlDbType.VarChar || dbType == SqlDbType.NText || dbType == SqlDbType.Text)
                        {
                                return string.Empty;
                        }
                        else if (dbType == SqlDbType.Date || dbType == SqlDbType.DateTime || dbType == SqlDbType.DateTime2 || dbType == SqlDbType.SmallDateTime)
                        {
                                return _SqlMinTime;
                        }
                        else
                        {
                                return dbType == SqlDbType.Xml ? string.Empty : 0;
                        }
                }

                public static object GetValue(Type type, object res)
                {
                        if (UriType.FullName == type.FullName)
                        {
                                string uri = Convert.ToString(res);
                                return uri != string.Empty ? new Uri(uri) : null;
                        }
                        else if (type.IsClass && type.FullName != StrType.FullName)
                        {
                                string json = Convert.ToString(res);
                                return json != string.Empty ? Json(json, type) : null;
                        }
                        else if (type.IsGenericType && type.Name == "Nullable`1")
                        {
                                type = type.GenericTypeArguments[0];
                                return GetValue(type, res);
                        }
                        else if (type.IsEnum)
                        {
                                return Enum.ToObject(type, res);
                        }
                        else if (type.FullName == StrType.FullName)
                        {
                                return res.ToString();
                        }
                        else if (type.FullName == DateTimeType.FullName)
                        {
                                return FormatTime(res);
                        }
                        else
                        {
                                return Convert.ChangeType(res, type);

                        }
                }
                public static DateTime FormatTime(object res)
                {
                        Type type = res.GetType();
                        if (type.FullName == DateTimeType.FullName)
                        {
                                DateTime time = (DateTime)res;
                                if (time == _SqlMinTime)
                                {
                                        return DateTime.MinValue;
                                }
                                else
                                {
                                        return time;
                                }
                        }
                        else if (type.FullName == _LongType.FullName)
                        {
                                return GetTimeStamp((long)res);
                        }
                        else if (type.FullName == StrType.FullName)
                        {
                                return DateTime.Parse((string)res);
                        }
                        return DateTime.MinValue;
                }
                public static string GetSqlFunc(SqlFuncType type)
                {
                        return type.ToString();
                }
                public static string GetSymbol(QueryType type)
                {
                        return _SymbolStr[(int)type];
                }
                internal static SqlDbType GetSqlDbType(Type type)
                {
                        if (type.IsEnum)
                        {
                                return SqlDbType.Int;
                        }
                        else if (type.IsClass && type.FullName != StrType.FullName)
                        {
                                return SqlDbType.NVarChar;
                        }
                        else if (type.IsGenericType && type.Name == "Nullable`1")
                        {
                                type = type.GenericTypeArguments[0];
                                return GetSqlDbType(type);
                        }
                        else if (type.FullName == StrType.FullName)
                        {
                                return SqlDbType.NVarChar;
                        }
                        else if (type.IsArray && type.GetElementType().FullName == _ByteType.FullName)
                        {
                                return SqlDbType.Binary;
                        }
                        else
                        {
                                return type.FullName == GuidType.FullName ? SqlDbType.UniqueIdentifier : GetSqlDbType(Type.GetTypeCode(type));
                        }
                }
                public static T Json<T>(string str)
                {
                        return JsonConvert.DeserializeObject<T>(str, _Settling);
                }
                internal static SqlDbType GetSqlDbType(TypeCode type)
                {
                        switch (type)
                        {
                                case TypeCode.Int64:
                                        return SqlDbType.BigInt;
                                case TypeCode.Int32:
                                        return SqlDbType.Int;
                                case TypeCode.Int16:
                                        return SqlDbType.SmallInt;
                                case TypeCode.Decimal:
                                        return SqlDbType.Decimal;
                                case TypeCode.Double:
                                        return SqlDbType.Float;
                                case TypeCode.Boolean:
                                        return SqlDbType.Bit;
                                case TypeCode.Char:
                                        return SqlDbType.Char;
                                case TypeCode.DateTime:
                                        return SqlDbType.DateTime;
                                case TypeCode.UInt16:
                                        return SqlDbType.SmallInt;
                                case TypeCode.UInt32:
                                        return SqlDbType.Int;
                                case TypeCode.UInt64:
                                        return SqlDbType.BigInt;
                                case TypeCode.Byte:
                                        return SqlDbType.TinyInt;
                                case TypeCode.Object:
                                        return SqlDbType.NVarChar;
                                default:
                                        return SqlDbType.NVarChar;
                        }
                }
                internal static Type GetType(SqlDbType sqlType)
                {
                        switch (sqlType)
                        {
                                case SqlDbType.BigInt:
                                        return _LongType;
                                case SqlDbType.Binary:
                                        return _ByteArrayType;
                                case SqlDbType.Bit:
                                        return _boolType;
                                case SqlDbType.Char:
                                        return _CharType;
                                case SqlDbType.DateTime:
                                        return DateTimeType;
                                case SqlDbType.Decimal:
                                        return typeof(decimal);
                                case SqlDbType.Float:
                                        return typeof(double);
                                case SqlDbType.Image:
                                        return _ByteArrayType;
                                case SqlDbType.Int:
                                        return _IntType;
                                case SqlDbType.Money:
                                        return typeof(decimal);
                                case SqlDbType.NChar:
                                        return StrType;
                                case SqlDbType.NText:
                                        return StrType;
                                case SqlDbType.NVarChar:
                                        return StrType;
                                case SqlDbType.Real:
                                        return typeof(float);
                                case SqlDbType.SmallDateTime:
                                        return DateTimeType;
                                case SqlDbType.SmallInt:
                                        return _ShortType;
                                case SqlDbType.SmallMoney:
                                        return typeof(decimal);
                                case SqlDbType.Text:
                                        return StrType;
                                case SqlDbType.Timestamp:
                                        return _LongType;
                                case SqlDbType.TinyInt:
                                        return _ByteType;
                                case SqlDbType.Udt:
                                        return _ObjectType;
                                case SqlDbType.UniqueIdentifier:
                                        return GuidType;
                                case SqlDbType.VarBinary:
                                        return _ByteArrayType;
                                case SqlDbType.VarChar:
                                        return StrType;
                                case SqlDbType.Variant:
                                        return typeof(object);
                                case SqlDbType.Xml:
                                        return StrType;
                                case SqlDbType.Date:
                                        return DateTimeType;
                                case SqlDbType.DateTime2:
                                        return DateTimeType;
                                case SqlDbType.DateTimeOffset:
                                        return DateTimeType;
                                case SqlDbType.Time:
                                        return DateTimeType;
                                default:
                                        return null;
                        }

                }
        }
}
