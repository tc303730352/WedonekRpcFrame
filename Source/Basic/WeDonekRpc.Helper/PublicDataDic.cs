using System;
using System.Net;
using System.Threading.Tasks;

namespace WeDonekRpc.Helper
{
    public class PublicDataDic
    {
        public static readonly string TrueValue = "true";
        public static readonly string FalseValue = "false";
        public static readonly string[] ImgFormat = new string[] { ".jpg", ".png", ".jpeg", ".bmp", ".gif", ".tiff", ".psd", ".cdr", ".svg", ".raw", ".ico", ".hdr", ".tif", ".ai", ".eps", ".dxf", ".pcd", ".fpx", ".tga", ".att", ".dib", ".rle", ".bw", ".col", ".dwg", ".dxb", ".wmf", ".emf", ".iff", ".lbm", ".mag", ".mac", ".mpt", ".msk", ".opt", ".ply", ".pcx", ".pic", ".pict", ".pict2", "pnt", ".pdd", ".pxr", ".ras", ".win", ".xbm" };
        public static readonly string[] MainImgFormat = new string[] { ".jpg", ".png", ".jpeg", ".bmp", ".gif" };
        public static readonly string[] ExcelFormat = new string[] { ".xls", ".xlsx" };
        public static readonly string[] WordFormat = new string[] { ".docx", ".doc" };

        public static readonly string[] AudioFormat = new string[] { "" };

        /// <summary>
        /// 小写字母列表
        /// </summary>
        public static readonly string[] Letter = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        /// <summary>
        /// 大写字母列表
        /// </summary>
        public static readonly string[] UpperLetter = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// <summary>
        /// 中文数字列表
        /// </summary>
        public static readonly char[] ChinaNum = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };

        /// <summary>
        /// 中文数字单位列表
        /// </summary>
        public static readonly char[] ChinaNumUnit = new char[] { ' ', '十', '百', '千', '万', '十', '百', '千', '亿', '十', '佰', '千', '万', '兆', '十', '佰', '千', '万', '亿' };
        /// <summary>
        /// 数字0的Ascii码
        /// </summary>
        public const int ZeroAscii = 48;
        /// <summary>
        /// 字符串类型
        /// </summary>
        public const string StringTypeName = "String";
        /// <summary>
        /// URI类型名
        /// </summary>
        public const string UriTypeName = "Uri";

        public const string LongTypeName = "Int64";
        public const string DecimalTypeName = "Decimal";
        public static string Nullable = "Nullable`1";
        public const string IntTypeName = "Int32";
        public static readonly Type GuidType = typeof(Guid);
        public static readonly Type IntType = typeof(int);
        public static readonly Type StrType = typeof(string);
        public static readonly Type BoolType = typeof(bool);
        public static readonly Type UriType = typeof(Uri);
        public static readonly Type IPAddressType = typeof(IPAddress);
        public static readonly Type IPEndPointType = typeof(IPEndPoint);
        public static readonly Type DateTimeType = typeof(DateTime);
        public static readonly Type LongType = typeof(long);
        public static readonly Type ShortType = typeof(short);
        public static readonly Type UShortType = typeof(ushort);
        public static readonly Type UIntType = typeof(uint);
        public static readonly Type ULongType = typeof(ulong);
        public static readonly Type DecimalType = typeof(decimal);
        public static readonly Type ByteType = typeof(byte);
        public static readonly Type CharType = typeof(char);
        public static readonly Type EnumType = typeof(Enum);
        public static readonly Type ObjectType = typeof(object);
        public static readonly Type DelegateType = typeof(Delegate);
        public static readonly Type AttributeType = typeof(Attribute);
        public static readonly Type ExceptionType = typeof(Exception);
        public static readonly Type MulticastDelegateType = typeof(MulticastDelegate);
        public static readonly Type VoidType = typeof(void);
        public static readonly Type TaskType = typeof(Task);
        public static readonly string TaskTName = "Task`1";
        public const string ByteTypeName = "Byte";
    }
}
