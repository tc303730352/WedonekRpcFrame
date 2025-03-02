using System;
using System.Text;

using Enyim.Caching.Memcached;

using WeDonekRpc.Helper;
namespace WeDonekRpc.CacheClient.Memcached
{
    internal class MemcachedTranscoder : DefaultTranscoder
    {
        private readonly int _ByteMaxLen = 1024 * 5;
        public override T Deserialize<T> (CacheItem item)
        {
            return (T)this._Deserialize(item, typeof(T));
        }
        protected override object Deserialize (CacheItem item)
        {
            return this._Deserialize(item, null);
        }
        private ArraySegment<byte> _SerializeDecimal (decimal num)
        {
            return new ArraySegment<byte>(Encoding.UTF8.GetBytes(num.ToString()));
        }
        private static uint _GetFlags (int code)
        {
            return (uint)( code | 0x0100 );
        }
        private ArraySegment<byte> _SerializeString (string value)
        {
            return this._SerializeByte(Encoding.UTF8.GetBytes(value));
        }
        protected override ArraySegment<byte> SerializeObject (object value)
        {
            return this._SerializeString(value.ToJson());
        }
        private object _DeserializeObject (ArraySegment<byte> value, Type type)
        {
            if (type == null)
            {
                type = typeof(object);
            }
            string str = this.DeserializeString(value);
            return str.Json(type);
        }
        private decimal _DeserializeDecimal (ArraySegment<byte> value)
        {
            string val = Encoding.UTF8.GetString(value.Array, value.Offset, value.Count);
            if (val == null)
            {
                return 0;
            }
            return decimal.Parse(val);
        }
        protected override string DeserializeString (ArraySegment<byte> value)
        {
            byte[] myBytes = value.Array;
            if (myBytes[0] == 0)
            {
                return Encoding.UTF8.GetString(myBytes, 1, myBytes.Length - 1);
            }
            else
            {
                return ZipTools.DecompressionString(myBytes, 1, myBytes.Length - 1);
            }
        }
        private ArraySegment<byte> _SerializeByte (byte[] myByte)
        {
            if (myByte.Length > this._ByteMaxLen)
            {
                myByte = ZipTools.Compression(myByte).TopInsert<byte>(1);
            }
            else
            {
                myByte = myByte.TopInsert<byte>(0);
            }
            return new ArraySegment<byte>(myByte);
        }
        private byte[] _DeserializeByte (ArraySegment<byte> data)
        {
            byte[] mybyte = data.Array;
            if (mybyte[0] == 0)
            {
                return mybyte.Remove(0);
            }
            return ZipTools.Decompression(mybyte, 1, mybyte.Length - 1);
        }

        private object _Deserialize (CacheItem item, Type type)
        {
            if (item.Data.Array == null)
            {
                return null;
            }
            else if (item.Flags == RawDataFlag)
            {
                return this._DeserializeByte(item.Data);
            }
            uint val = item.Flags & 0xff;
            if (val == 2)
            {
                return null;
            }
            else if (val == 19)
            {
                string str = Encoding.UTF8.GetString(item.Data.Array);
                return new Uri(str);
            }
            else if (val == 20)
            {
                string str = Encoding.UTF8.GetString(item.Data.Array);
                return new Guid(str);
            }
            ArraySegment<byte> data = item.Data;
            switch ((TypeCode)val)
            {
                case TypeCode.Empty:
                    return data.Count == 0 ? string.Empty : Encoding.UTF8.GetString(data.Array);
                case TypeCode.String:
                    return this.DeserializeString(data);
                case TypeCode.Boolean:
                    return this.DeserializeBoolean(data);
                case TypeCode.Int16:
                    return this.DeserializeInt16(data);
                case TypeCode.Int32:
                    return this.DeserializeInt32(data);
                case TypeCode.Int64:
                    return this.DeserializeInt64(data);
                case TypeCode.UInt16:
                    return this.DeserializeUInt16(data);
                case TypeCode.UInt32:
                    return this.DeserializeUInt32(data);
                case TypeCode.UInt64:
                    return this.DeserializeUInt64(data);
                case TypeCode.Char:
                    return this.DeserializeChar(data);
                case TypeCode.DateTime:
                    return this.DeserializeDateTime(data);
                case TypeCode.Double:
                    return this.DeserializeDouble(data);
                case TypeCode.Single:
                    return this.DeserializeSingle(data);
                case TypeCode.Byte:
                    return this.DeserializeByte(data);
                case TypeCode.SByte:
                    return this.DeserializeSByte(data);
                case TypeCode.Decimal:
                    return this._DeserializeDecimal(data);
                case TypeCode.Object:
                    return this._DeserializeObject(data, type);
            }
            return null;
        }
        protected override CacheItem Serialize (object value)
        {
            if (value is ArraySegment<byte> i)
            {
                return new CacheItem(RawDataFlag, this._SerializeByte(i.Array));
            }
            else if (value is byte[] bytes)
            {
                return new CacheItem(RawDataFlag, this._SerializeByte(bytes));
            }
            else if (value == null)
            {
                return new CacheItem(_GetFlags(2), base.SerializeNull());
            }
            else
            {
                Type type = value.GetType();
                if (type.IsEnum)
                {
                    return new CacheItem(_GetFlags(9), base.SerializeInt32((int)value));
                }
                else if (type == PublicDataDic.UriType)
                {
                    return new CacheItem(_GetFlags(19), base.SerializeString(value.ToString()));
                }
                else if (type == PublicDataDic.GuidType)
                {
                    return new CacheItem(_GetFlags(20), base.SerializeString(value.ToString()));
                }
                else if (type == PublicDataDic.StrType)
                {
                    return new CacheItem(_GetFlags(18), this._SerializeString((string)value));
                }
                else
                {
                    ArraySegment<byte> data;
                    TypeCode code = Type.GetTypeCode(value.GetType());
                    switch (code)
                    {
                        case TypeCode.Boolean:
                            data = this.SerializeBoolean((bool)value);
                            break;
                        case TypeCode.SByte:
                            data = this.SerializeSByte((sbyte)value);
                            break;
                        case TypeCode.Byte:
                            data = this.SerializeByte((byte)value);
                            break;
                        case TypeCode.Int16:
                            data = this.SerializeInt16((short)value);
                            break;
                        case TypeCode.Int32:
                            data = this.SerializeInt32((int)value);
                            break;
                        case TypeCode.Int64:
                            data = this.SerializeInt64((long)value);
                            break;
                        case TypeCode.UInt16:
                            data = this.SerializeUInt16((ushort)value);
                            break;
                        case TypeCode.UInt32:
                            data = this.SerializeUInt32((uint)value);
                            break;
                        case TypeCode.UInt64:
                            data = this.SerializeUInt64((ulong)value);
                            break;
                        case TypeCode.Char:
                            data = this.SerializeChar((char)value);
                            break;
                        case TypeCode.Decimal:
                            data = this._SerializeDecimal((decimal)value);
                            break;
                        case TypeCode.DateTime:
                            data = this.SerializeDateTime((DateTime)value);
                            break;
                        case TypeCode.Double:
                            data = this.SerializeDouble((double)value);
                            break;
                        case TypeCode.Single:
                            data = this.SerializeSingle((float)value);
                            break;
                        default:
                            code = TypeCode.Object;
                            data = this.SerializeObject(value);
                            break;
                    }
                    return new CacheItem(TypeCodeToFlag(code), data);
                }
            }
        }
    }
}
