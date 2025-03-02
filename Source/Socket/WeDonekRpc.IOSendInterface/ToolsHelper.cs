using System;
using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.IOSendInterface
{
    public class ToolsHelper
    {


        public static byte GetSendType (Type type)
        {
            if (type.Name == PublicDataDic.StringTypeName)
            {
                return ConstDicConfig.StringSendType;
            }
            else if (type.IsArray && type.GetElementType().Name == PublicDataDic.ByteTypeName)
            {
                return ConstDicConfig.StreamSendType;
            }
            else if (type.IsClass)
            {
                return ConstDicConfig.ObjectSendType;
            }
            return ConstDicConfig.StringSendType;
        }
        /// <summary>
        /// 序列化数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SerializationString (string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
        public static string DeserializeStringData (ReadOnlySpan<byte> data)
        {
            return Encoding.UTF8.GetString(data);
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeData<T> (ReadOnlySpan<byte> data)
        {
            string json = Encoding.UTF8.GetString(data);
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            Type type = typeof(T);
            return type.IsClass && type != PublicDataDic.StrType ? JsonTools.Json<T>(json) : (T)Convert.ChangeType(json, type);
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SerializationClass (object data)
        {
            if (data == null)
            {
                return Array.Empty<byte>();
            }
            string json = JsonTools.Json(data, data.GetType());
            return SerializationString(json);
        }
        public static byte[] SerializationData (Type type, object data)
        {
            if (type.Name == PublicDataDic.StringTypeName)
            {
                return SerializationString((string)data);
            }
            else if (type.IsClass)
            {
                return SerializationClass(data);
            }
            else if (type == PublicDataDic.DateTimeType)
            {
                long span = ( (DateTime)data ).ToLong();
                return BitConverter.GetBytes(span);
            }
            else if (type == PublicDataDic.GuidType)
            {
                return ( (Guid)data ).ToByteArray();
            }
            else if (type.Name == PublicDataDic.DecimalTypeName)
            {
                return SerializationString(data.ToString());
            }
            else
            {
                TypeCode code = Type.GetTypeCode(type);
                switch (code)
                {
                    case TypeCode.Byte:
                        return new byte[] {
                                     (byte)data
                                  };
                    case TypeCode.Boolean:
                        return BitConverter.GetBytes((bool)data);
                    case TypeCode.Double:
                        return BitConverter.GetBytes((double)data);
                    case TypeCode.Single:
                        return BitConverter.GetBytes((float)data);
                    case TypeCode.Int16:
                        return BitConverter.GetBytes((short)data);
                    case TypeCode.UInt16:
                        return BitConverter.GetBytes((ushort)data);
                    case TypeCode.Int32:
                        return BitConverter.GetBytes((int)data);
                    case TypeCode.UInt32:
                        return BitConverter.GetBytes((uint)data);
                    case TypeCode.UInt64:
                        return BitConverter.GetBytes((ulong)data);
                    case TypeCode.Int64:
                        return BitConverter.GetBytes((long)data);
                    default:
                        return SerializationClass(data);
                }

            }
        }
        public static object GetValue (Type type, byte[] data)
        {
            if (type == PublicDataDic.DateTimeType)
            {
                long span = BitConverter.ToInt64(data, 0);
                return Tools.GetTimeStamp(span);
            }
            else if (type == PublicDataDic.GuidType)
            {
                return new Guid(data);
            }
            else if (type.Name == PublicDataDic.DecimalTypeName)
            {
                string val = ToolsHelper.DeserializeStringData(data);
                return decimal.Parse(val);
            }
            else
            {
                TypeCode code = Type.GetTypeCode(type);
                switch (code)
                {
                    case TypeCode.Byte:
                        return data[0];
                    case TypeCode.Boolean:
                        return data[0];
                    case TypeCode.Double:
                        return data[0];
                    case TypeCode.Single:
                        return BitConverter.ToSingle(data, 0);
                    case TypeCode.Int16:
                        return BitConverter.ToInt16(data, 0);
                    case TypeCode.UInt16:
                        return BitConverter.ToUInt16(data, 0);
                    case TypeCode.Int32:
                        return BitConverter.ToInt32(data, 0);
                    case TypeCode.UInt32:
                        return BitConverter.ToUInt32(data, 0);
                    case TypeCode.UInt64:
                        return BitConverter.ToUInt64(data, 0);
                    case TypeCode.Int64:
                        return BitConverter.ToInt64(data, 0);
                    default:
                        return null;
                }

            }
        }
    }
}
