using System;
using System.Collections.Generic;

using HttpApiDocHelper.Interface;
using HttpApiDocHelper.Model;

using ApiGateway.Model;

using RpcHelper;

namespace HttpApiDocHelper.Collect
{
        internal class ClassCollect
        {
                private static readonly Dictionary<string, ClassFormat> _XmlParamList = new Dictionary<string, ClassFormat>();

                public static IApiDataFormat[] GetClassProList(string id)
                {
                        if (id == null)
                        {
                                return new IApiDataFormat[0];
                        }
                        return _XmlParamList.TryGetValue(id, out ClassFormat format) ? format.ProList : null;
                }
                public static ClassFormat GetClass(string id)
                {
                        return _XmlParamList[id];
                }
                private static ClassFormat _AddPagingClass(Type type, string apiId, string show)
                {
                        string id = string.Concat("PagingModel_", apiId).GetMd5().ToLower();
                        if (_XmlParamList.TryGetValue(id, out ClassFormat format))
                        {
                                return format;
                        }
                        else
                        {
                                format = new ClassFormat(id);
                                _XmlParamList.Add(id, format);
                                format.LoadPaging(type, show);
                                return format;
                        }
                }
                internal static string RegResultClass(ApiFuncBody body, string apiId)
                {
                        string id = string.Concat("Result_", apiId).GetMd5().ToLower();
                        if (_XmlParamList.ContainsKey(id))
                        {
                                return id;
                        }
                        else
                        {
                                ClassFormat format = new ClassFormat(id);
                                _XmlParamList.Add(id, format);
                                format.InitReturn(ApiDocModular.Template, body);
                                return id;
                        }
                }
                internal static string RegResultClass(Type type, string apiId, string show)
                {
                        string id = string.Concat("Result_", apiId).GetMd5().ToLower();
                        if (_XmlParamList.ContainsKey(id))
                        {
                                return id;
                        }
                        else
                        {
                                ClassFormat format = new ClassFormat(id);
                                _XmlParamList.Add(id, format);
                                format.InitReturn(ApiDocModular.Template, type, show);
                                return id;
                        }
                }
                private static string _AddResultClass(string apiId, ClassFormat data)
                {
                        string id = string.Concat("Result_", apiId).GetMd5().ToLower();
                        if (_XmlParamList.ContainsKey(id))
                        {
                                return id;
                        }
                        else
                        {
                                ClassFormat format = new ClassFormat(id);
                                _XmlParamList.Add(id, format);
                                format.InitTemplate(ApiDocModular.Template, data);
                                return id;
                        }
                }
                private static ClassFormat _AddPagingClass(ApiFuncBody body, string apiId)
                {
                        string id = string.Concat("PagingModel_", apiId).GetMd5().ToLower();
                        if (_XmlParamList.TryGetValue(id, out ClassFormat format))
                        {
                                return format;
                        }
                        else
                        {
                                format = new ClassFormat(id);
                                _XmlParamList.Add(id, format);
                                format.LoadPaging(body);
                                return format;
                        }
                }
                internal static string RegPagingClass(ApiFuncBody body, string apiId)
                {
                        ClassFormat cla = _AddPagingClass(body, apiId);
                        return _AddResultClass(apiId, cla);
                }
                internal static string RegPagingClass(Type type, string apiId, string show)
                {
                        ClassFormat cla = _AddPagingClass(type, apiId, show);
                        return _AddResultClass(apiId, cla);
                }
                internal static string LoadClass(Type type, string name = null)
                {
                        if (type.IsClass && type != PublicDataDic.StrType)
                        {
                                string id = type.FullName.GetMd5().ToLower();
                                if (_XmlParamList.ContainsKey(id))
                                {
                                        return id;
                                }
                                else
                                {
                                        ClassFormat format = new ClassFormat(id);
                                        _XmlParamList.Add(id, format);
                                        format.LoadFormat(type, name);
                                        return id;
                                }
                        }
                        return null;
                }

                internal static ClassData GetClassData(string id)
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
