using System.Collections.Concurrent;
using System.Reflection;
using GatewayBuildCli.Model;

namespace GatewayBuildCli
{
    internal class XmlShowHelper
    {
        private static readonly ConcurrentDictionary<string, XmlParamDic> _XmlParamList = new ConcurrentDictionary<string, XmlParamDic>();
        public static string BasicPath;
        public static string FindParamShow ( Type type )
        {
            string key = type.Module.ScopeName;
            if ( _XmlParamList.TryGetValue(key, out XmlParamDic dic) )
            {
                return dic.FindParamShow(type);
            }
            string path = Path.Combine(BasicPath, key.Replace(".dll", ".xml"));
            if ( !File.Exists(path) )
            {
                return string.Empty;
            }
            else
            {
                dic = new XmlParamDic();
                dic.LoadXml(path);
                _ = _XmlParamList.TryAdd(key, dic);
                return dic.FindParamShow(type);
            }
        }
        public static string FindParamShow ( Type type, MethodInfo method )
        {
            string key = type.Module.ScopeName;
            if ( _XmlParamList.TryGetValue(key, out XmlParamDic dic) )
            {
                return dic.FindParamShow(type, method);
            }
            string path = Path.Combine(BasicPath, key.Replace(".dll", ".xml"));
            if ( !File.Exists(path) )
            {
                return string.Empty;
            }
            else
            {
                dic = new XmlParamDic();
                dic.LoadXml(path);
                _ = _XmlParamList.TryAdd(key, dic);
                return dic.FindParamShow(type, method);
            }
        }
        public static string FindParamShow ( Type type, MethodInfo method, string name )
        {
            string key = type.Module.ScopeName;
            if ( _XmlParamList.TryGetValue(key, out XmlParamDic dic) )
            {
                return dic.FindParamShow(type, method, name);
            }
            string path = Path.Combine(BasicPath, key.Replace(".dll", ".xml"));
            if ( !File.Exists(path) )
            {
                return string.Empty;
            }
            else
            {
                dic = new XmlParamDic();
                dic.LoadXml(path);
                _ = _XmlParamList.TryAdd(key, dic);
                return dic.FindParamShow(type, method, name);
            }
        }

        public static string FindParamShow ( Type type, string name )
        {
            string key = type.Module.ScopeName;
            if ( _XmlParamList.TryGetValue(key, out XmlParamDic dic) )
            {
                return dic.FindParamShow(type, name);
            }
            string path = Path.Combine(BasicPath, key.Replace(".dll", ".xml"));
            if ( !File.Exists(path) )
            {
                return string.Empty;
            }
            else
            {
                dic = new XmlParamDic();
                dic.LoadXml(path);
                _ = _XmlParamList.TryAdd(key, dic);
                return dic.FindParamShow(type, name);
            }
        }

    }
}
