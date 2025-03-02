using WeDonekRpc.HttpApiDoc.Collect;
using WeDonekRpc.HttpApiDoc.Interface;
using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.Helper;
using System.Text;

namespace WeDonekRpc.HttpApiDoc.Helper
{
    internal class ApiDataHelper
    {
        public static string GetGetFormat(IApiDataFormat[] datas)
        {
            if (datas.IsNull())
            {
                return string.Empty;
            }
            else
            {
                StringBuilder str = new StringBuilder();
                datas.ForEach(a =>
                {
                    str.AppendFormat("&{0}=", a.ParamName);
                });
                str.Remove(0, 1);
                return str.ToString();
            }
        }
        public static string GetPostFormat(ApiPostBody post)
        {
            if (post == null)
            {
                return string.Empty;
            }
            else if (post.DataType == ApiRequestType.字典)
            {
                if (post.ProList.IsNull())
                {
                    return string.Empty;
                }
                StringBuilder str = new StringBuilder();
                post.ProList.ForEach(a =>
                {
                    str.AppendFormat("&{0}=", a.ParamName);
                });
                str.Remove(0, 1);
                return str.ToString();
            }
            return _GetPostJson(post);
        }
        private static string _GetElementJson(ElementClass ele)
        {
            if (ele.ElementType == ElementType.基本)
            {
                return "null";
            }
            else if (ele.ElementType == ElementType.枚举)
            {
                return "0";
            }
            ClassData data = ClassCollect.GetClassData(ele.Id);
            StringBuilder json = new StringBuilder("{");
            data.ProList.ForEach(a =>
            {
                json.AppendFormat("\"{0}\":{1},", a.ParamName, _GetApiProValue(a));
            });
            json.Remove(json.Length - 1, 1);
            json.Append("}");
            return json.ToString();
        }
        private static string _GetDicJosn(ElementClass key, ElementClass val)
        {
            StringBuilder json = new StringBuilder("{");
            json.AppendFormat("\"{0}\":{1}", key.ElementName, _GetElementJson(val));
            json.Append("}");
            return json.ToString();
        }
        private static string _GetApiProValue(IApiDataFormat api)
        {
            ApiPostFormat obj = api as ApiPostFormat;
            if (obj == null)
            {
                return string.Empty;
            }
            else if (obj.NullCheck.IsAllowNull)
            {
                if (obj.DefValue == "Null")
                {
                    return "null";
                }
                else
                {
                    return obj.DataType == ApiDataType.数字 || obj.DataType == ApiDataType.布尔 ? obj.DefValue : string.Concat("\"", obj.DefValue, "\"");
                }
            }
            else if (obj.DataType == ApiDataType.字符串)
            {
                return "\"\"";
            }
            else if (obj.DataType == ApiDataType.数字)
            {
                return "0";
            }
            else if (obj.DataType == ApiDataType.数组)
            {
                return string.Concat("[", _GetElementJson(obj.ElementType[0]), "]");
            }
            else if (obj.DataType == ApiDataType.泛型)
            {
                return "null";
            }
            else if (obj.DataType == ApiDataType.类)
            {
                return _GetElementJson(obj.ElementType[0]);
            }
            else if (obj.DataType == ApiDataType.泛型字典)
            {
                return _GetDicJosn(obj.ElementType[0], obj.ElementType[1]);
            }
            return obj.DefValue == "Null" ? "null" : obj.DefValue;
        }
        private static string _GetPostJson(ApiPostBody post)
        {
            if (post.ProList.IsNull() || post.DataType == ApiRequestType.字典)
            {
                return null;
            }
            StringBuilder json = new StringBuilder();
            if (post.DataType == ApiRequestType.数组)
            {
                json.Append("[");
            }
            if (post.IsClass)
            {
                json.Append("{");
                post.ProList.ForEach(a =>
                {
                    json.AppendFormat("\"{0}\":{1},", a.ParamName, _GetApiProValue(a));
                });
                json.Remove(json.Length - 1, 1);
                json.Append("}");
            }
            if (post.DataType == ApiRequestType.数组)
            {
                json.Append("]");
            }
            return json.ToString();
        }
    }
}
