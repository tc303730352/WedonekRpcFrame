using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RpcHelper.Validate
{
        internal class DataValidate
        {
                private const string DateTimeName = "DateTime";
                private const string GuidName = "Guid";
                public const string StrTypeName = "String";

                public static bool CheckIsNull(Type type, object data)
                {
                        if (type.IsValueType)
                        {
                                if (data == null)
                                {
                                        return true;
                                }
                                else if (type.Name == DateTimeName)
                                {
                                        return ((DateTime)data) == DateTime.MinValue;
                                }
                                else if (type.Name == GuidName)
                                {
                                        return ((Guid)data) == Guid.Empty;
                                }
                                else
                                {
                                        object num = Convert.ChangeType(data, TypeCode.Decimal);
                                        return num.Equals(decimal.Zero);
                                }
                        }
                        else if (data == null)
                        {
                                return true;
                        }
                        else if (type.Name == DataValidate.StrTypeName && string.IsNullOrEmpty((string)data))
                        {
                                return true;
                        }
                        else if (type.IsArray && ((Array)data).Length == 0)
                        {
                                return true;
                        }
                        return false;
                }
                public static bool CheckTime(Type type, object data, TimeValidate validate)
                {
                        DateTime time;
                        if (type.Name != DataValidate.DateTimeName)
                        {
                                if (type.Name == PublicDataDic.StringTypeName)
                                {
                                        string str = (string)data;
                                        if (str == string.Empty)
                                        {
                                                return true;
                                        }
                                        else if (!ValidateHelper.CheckIsTime(str))
                                        {
                                                return true;
                                        }
                                        else
                                        {
                                                time = DateTime.Parse(str);
                                        }
                                }
                                else if (type.Name == PublicDataDic.LongTypeName)
                                {
                                        time = Tools.GetTimeStamp((long)data);
                                }
                                else
                                {
                                        return true;
                                }
                        }
                        else
                        {
                                time = (DateTime)data;
                        }
                        return time < validate.MinTime || time > validate.MaxTime;
                }
                public static bool CheckLen(Type type, object data, int minlen, int maxLen, bool isArray, bool isAllowEmpty)
                {
                        int len;
                        if (type.Name == DataValidate.StrTypeName)
                        {
                                string str = (string)data;
                                if (str == string.Empty && isAllowEmpty)
                                {
                                        return false;
                                }
                                len = str.Length;
                        }
                        else if ((type.IsArray || type.IsGenericType) && isArray)
                        {
                                return false;
                        }
                        else if (type.IsArray)
                        {
                                len = ((Array)data).Length;
                        }
                        else if (type.IsGenericType)
                        {
                                PropertyInfo property = type.GetProperty("Count");
                                if (property != null)
                                {
                                        len = (int)property.GetValue(data);
                                }
                                else
                                {
                                        return false;
                                }
                        }
                        else
                        {
                                return false;
                        }
                        return len < minlen || len > maxLen;
                }
                public static bool CheckSize(Type type, object data, NumValidate validate)
                {
                        if (!type.IsValueType)
                        {
                                return false;
                        }
                        long num;
                        if (type.IsEnum)
                        {
                                TypeCode codeType = Type.GetTypeCode(type);
                                if (codeType == TypeCode.Int64)
                                {
                                        num = (long)data;
                                }
                                else if (codeType == TypeCode.Int32)
                                {
                                        num = (int)data;
                                }
                                else
                                {
                                        num = (short)data;
                                }
                        }
                        else
                        {
                                num = (long)Convert.ChangeType(data, TypeCode.Int64);
                        }
                        if (num < validate.MinNum || num > validate.MaxNum)
                        {
                                return true;
                        }
                        else if (num == 0)
                        {
                                return false;
                        }
                        else if (validate.Format == NumFormat.偶数)
                        {
                                return !ValidateHelper.CheckIsEvenNumber(num);
                        }
                        else if (validate.Format == NumFormat.奇数)
                        {
                                return !ValidateHelper.CheckIsOddNumber(num);
                        }
                        else if (validate.Format == NumFormat.对数 && Math.Log(num, validate.BaseNum) % 1 != 0)
                        {
                                return true;
                        }
                        else if (validate.Format == NumFormat.取余 && num % validate.BaseNum != 0)
                        {
                                return true;
                        }
                        return false;
                }
                public static bool CheckString(string str, ValidateFormat format, string containStr)
                {
                        return !ValidateHelper.CheckData(str, format, containStr);
                }
                public static bool CheckString(Type type, object data, Regex regex)
                {
                        if (type.Name != DataValidate.StrTypeName)
                        {
                                return false;
                        }
                        string str = (string)data;
                        return !regex.IsMatch(str);
                }
        }
}
