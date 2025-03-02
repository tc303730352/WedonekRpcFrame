using System;
using System.Collections.Generic;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.HttpApiDoc.Interface;
using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;

namespace WeDonekRpc.HttpApiDoc.Collect
{
    internal class ClassCollect
    {
        private static readonly Dictionary<string, ClassFormat> _XmlParamList = [];
        private static readonly LockHelper _Lock = new LockHelper();
        public static IApiDataFormat[] GetClassProList (string id)
        {
            if (id == null)
            {
                return new IApiDataFormat[0];
            }
            return _XmlParamList.TryGetValue(id, out ClassFormat format) ? format.ProList : null;
        }
        public static ClassFormat GetClass (string id)
        {
            return _XmlParamList[id];
        }

        internal static string RegResultClass (ApiFuncBody body, string apiId)
        {
            string id = string.Concat("Result_", apiId).GetMd5().ToLower();
            if (_XmlParamList.ContainsKey(id))
            {
                return id;
            }
            else
            {
                ClassFormat format = new ClassFormat(id);
                if (_Add(id, format))
                {
                    format.InitReturn(ApiDocModular.Template, body);
                }
                return id;
            }
        }
        private static bool _Add (string id, ClassFormat format)
        {
            if (_Lock.GetLock())
            {
                if (!_XmlParamList.ContainsKey(id))
                {
                    _XmlParamList.Add(id, format);
                    _Lock.Exit();
                    return true;
                }
                _Lock.Exit();
            }
            return false;
        }
        internal static string RegResultClass (Type type, string apiId, string show)
        {
            string id = string.Concat("Result_", apiId).GetMd5().ToLower();
            if (_XmlParamList.ContainsKey(id))
            {
                return id;
            }
            else
            {
                ClassFormat format = new ClassFormat(id);
                if (_Add(id, format))
                {
                    format.InitReturn(ApiDocModular.Template, type, show);
                }
                return id;
            }
        }
        private static string _AddResultClass (string apiId, ClassFormat data)
        {
            string id = string.Concat("Result_", apiId).GetMd5().ToLower();
            if (_XmlParamList.ContainsKey(id))
            {
                return id;
            }
            else
            {
                ClassFormat format = new ClassFormat(id);
                if (_Add(id, format))
                {
                    format.InitTemplate(ApiDocModular.Template, data);
                }
                return id;
            }
        }



        internal static string LoadClass (Type type, string name = null)
        {
            if (( type.IsClass || type.IsGenericType ) && type != PublicDataDic.StrType)
            {
                string id = type.FullName.GetMd5().ToLower();
                if (_XmlParamList.ContainsKey(id))
                {
                    return id;
                }
                else
                {
                    ClassFormat format = new ClassFormat(id);
                    if (_Add(id, format))
                    {
                        format.LoadFormat(type, name);
                    }
                    return id;
                }
            }
            return null;
        }

        internal static ClassData GetClassData (string id)
        {
            return _XmlParamList.TryGetValue(id, out ClassFormat format)
                    ? new ClassData
                    {
                        ClassName = format.ClassName,
                        ProList = format.ProList,
                        Show = format.ClassShow
                    }
                    : null;
        }

    }
}
