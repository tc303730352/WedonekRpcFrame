using WeDonekRpc.HttpApiDoc.Model;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace WeDonekRpc.HttpApiDoc.Helper
{
    internal class XmlShowHelper
    {
        private static readonly ConcurrentDictionary<string, XmlParamDic> _XmlParamList = new ConcurrentDictionary<string, XmlParamDic>();
        public static string FindParamShow(Type type)
        {
            string key = type.Module.Name;
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
        public static string FindParamShow(Type type, MethodInfo method)
        {
            string key = type.Module.Name;
            if (_XmlParamList.TryGetValue(key, out XmlParamDic dic))
            {
                return dic.FindParamShow(type, method);
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
                return dic.FindParamShow(type, method);
            }
        }
        public static string FindParamShow(Type type, MethodInfo method, string name)
        {
            string key = type.Module.Name;
            if (_XmlParamList.TryGetValue(key, out XmlParamDic dic))
            {
                return dic.FindParamShow(type, method, name);
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
                return dic.FindParamShow(type, method, name);
            }
        }

        public static string FindParamShow(Type type, string name)
        {
            string key = type.Module.Name;
            if (_XmlParamList.TryGetValue(key, out XmlParamDic dic))
            {
                return dic.FindParamShow(type, name);
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
                return dic.FindParamShow(type, name);
            }
        }

    }
}
