
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using WeDonekRpc.Client.Model;

namespace WeDonekRpc.Client.Helper
{
    internal class XmlShowHelper
    {
        private static readonly ConcurrentDictionary<string, XmlParamDic> _XmlParamList = new ConcurrentDictionary<string, XmlParamDic>();
        public static string FindShow(Type type)
        {
            string key = type.Module.ScopeName;
            if (_XmlParamList.TryGetValue(key, out XmlParamDic dic))
            {
                return dic.FindParamShow(type);
            }
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, key.Replace(".dll", ".xml"));
            if (!System.IO.File.Exists(path))
            {
                return string.Empty;
            }
            else
            {
                dic = new XmlParamDic();
                dic.LoadXml(path);
                _XmlParamList.TryAdd(key, dic);
                return dic.FindParamShow(type);
            }
        }
        public static string FindParamShow(MethodInfo method)
        {
            string key = method.Module.ScopeName;
            if (_XmlParamList.TryGetValue(key, out XmlParamDic dic))
            {
                return dic.FindParamShow(method);
            }
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, key.Replace(".dll", ".xml"));
            if (!System.IO.File.Exists(path))
            {
                return string.Empty;
            }
            else
            {
                dic = new XmlParamDic();
                dic.LoadXml(path);
                _XmlParamList.TryAdd(key, dic);
                return dic.FindParamShow(method);
            }
        }
    }
}
