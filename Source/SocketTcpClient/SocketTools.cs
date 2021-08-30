using System;
using System.Text;

using SocketBuffer;

using SocketTcpClient.Enum;
using SocketTcpClient.Model;

using RpcHelper;

namespace SocketTcpClient
{
        internal class SocketTools
        {
                private const byte _PageVer = 253;
                public const int HeadLen = 9;
                public static void ReadPage(object data)
                {
                        Model.GetDataPage arg = (Model.GetDataPage)data;
                        if (arg.IsCompression)
                        {
                                arg.PageContent = Tools.DecompressionData(arg.PageContent, arg.OriginalSize);
                        }
                        arg.Allot.InitInfo(arg);
                        string type = arg.Type;
                        object resData = arg.Allot.Action(ref type);
                        if (resData != null && (PageType.单向 & arg.PageType) != PageType.单向)
                        {
                                Page page = Page.GetReplyPage(arg, type, resData);
                                if (!arg.Client.SendPage(page, out string error))
                                {
                                        new LogInfo(error, LogGrade.ERROR, "Socket_Client")
                                        {
                                                LogTitle = "回复事件包错误!",
                                                LogContent = page.ToJson()
                                        }.Save();
                                }
                        }
                }


                /// <summary>
                /// 序列化数据
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                internal static byte[] SerializationString(string data)
                {
                        return Encoding.UTF8.GetBytes(data);
                }

                /// <summary>
                /// 字符串数据
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                internal static string DeserializeStringData(byte[] data)
                {
                        return data == null || data.Length == 0 ? string.Empty : Encoding.UTF8.GetString(data);
                }
                private static readonly Type _StrType = typeof(string);
                /// <summary>
                /// 反序列化数据
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                internal static T DeserializeData<T>(byte[] data)
                {
                        if (data == null || data.Length == 0)
                        {
                                return default;
                        }
                        string json = Encoding.UTF8.GetString(data);
                        if (string.IsNullOrEmpty(json))
                        {
                                return default;
                        }
                        Type type = typeof(T);
                        return type.IsClass && type != _StrType ? Tools.Json<T>(json) : (T)Convert.ChangeType(json, type);
                }

                /// <summary>
                /// 组建数据包
                /// </summary>
                /// <param name="data"></param>
                /// <param name="num"></param>
                /// <param name="objPage"></param>
                internal static bool SplitPage(byte[] data, int len, ref Model.DataPageInfo objPage, Client.SocketTcpClient client)
                {
                        int index = 0;
                        return SocketTools.SplitPage(data, len, ref index, ref objPage, client);
                }
                internal static bool SplitPage(byte[] data, int len, ref int index, ref Model.DataPageInfo objPage, Client.SocketTcpClient client)
                {
                        if (objPage == null)
                        {
                                if (data[index] != _PageVer)
                                {
                                        return false;
                                }
                                objPage = new Model.DataPageInfo();
                        }
                        if (objPage.LoadData(data, len, ref index))
                        {
                                client.ReceiveComplate();
                                if (index < len)
                                {
                                        return SocketTools.SplitPage(data, len, ref index, ref objPage, client);
                                }
                                return true;
                        }
                        return objPage.LoadProgress != PageLoadProgress.包校验错误;
                }

                internal static SendType GetSendType(object data)
                {
                        Type type = data.GetType();
                        if (type.Name == RpcHelper.PublicDataDic.StringTypeName)
                        {
                                return SendType.字符串;
                        }
                        else if (type.IsArray && type.GetElementType().Name == RpcHelper.PublicDataDic.ByteTypeName)
                        {
                                return SendType.字节流;
                        }
                        else if (type.IsClass)
                        {
                                return SendType.对象;
                        }
                        return SendType.字符串;
                }
                /// <summary>
                /// 序列化对象
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                internal static byte[] SerializationClass(object data)
                {
                        if (data == null)
                        {
                                return new byte[0];
                        }
                        string json = Tools.Json(data);
                        return SerializationString(json);
                }
                internal static byte[] SerializationData(Type type, object data)
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
                                long span = ((DateTime)data).ToLong();
                                return BitConverter.GetBytes(span);
                        }
                        else if (type == PublicDataDic.GuidType)
                        {
                                return ((Guid)data).ToByteArray();
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
                internal static object GetValue(Type type, byte[] data)
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
                                string val = DeserializeStringData(data);
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


                /// <summary>
                /// 获取发送的字节流
                /// </summary>
                /// <param name="pageInfo"></param>
                /// <returns></returns>
                internal static ISocketBuffer GetSendBuffer(DataPage page)
                {
                        ISocketBuffer buffer = BufferCollect.ApplySendBuffer(page.GetPageSize(), page.DataId);
                        _InitPageHead(buffer, page);
                        return buffer;
                }
                private static int _InitHeadBody(ISocketBuffer buffer, DataPage page)
                {
                        int index = 9;
                        if (page.OriginalSize <= short.MaxValue)
                        {
                                index += buffer.WriteShort((short)page.DataLen, index);
                                if (page.IsCompression)
                                {
                                        index += buffer.WriteShort((short)page.OriginalSize, index);
                                }
                        }
                        else
                        {
                                index += buffer.WriteInt(page.DataLen, index);
                                if (page.IsCompression)
                                {
                                        index += buffer.WriteInt(page.OriginalSize, index);
                                }
                        }
                        index += buffer.WriteByte(Tools.CS(buffer.Stream, index), index);
                        index += buffer.WriteChar(page.Type, index);
                        return index;
                }

                /// <summary>
                /// 获取包头信息
                /// </summary>
                /// <param name="pageId"></param>
                /// <param name="page"></param>
                /// <returns></returns>
                private static void _InitPageHead(ISocketBuffer buffer, DataPage page)
                {
                        buffer.WriteByte(_PageVer, 0);
                        buffer.WriteByte((byte)page.PageType, 1);
                        buffer.WriteByte((byte)page.DataType, 2);
                        buffer.WriteByte(page.HeadType, 3);
                        buffer.WriteInt(page.DataId, 4);
                        buffer.WriteByte(page.TypeLen, 8);
                        if (page.DataLen != 0)
                        {
                                int len = _InitHeadBody(buffer, page);
                                buffer.Write(page.Content, len);
                        }
                        else
                        {
                                buffer.WriteByte(Tools.CS(buffer.Stream, 9), 9);
                                buffer.WriteChar(page.Type, 10);
                        }
                }
        }
}
