using System;

using HttpApiDocHelper.Helper;
using HttpApiDocHelper.Interface;

using ApiGateway.Model;

using RpcHelper;

namespace HttpApiDocHelper.Model
{
        internal class ClassFormat
        {
                public ClassFormat(string id)
                {
                        this.ClassId = id;
                }
                public string ClassId
                {
                        get;
                }
                public string ClassName
                {
                        get;
                        private set;
                }
                public string ClassShow
                {
                        get;
                        private set;
                }
                public IApiDataFormat[] ProList
                {
                        get;
                        private set;
                }
                public ApiPostFormat ToDataFormat(ApiAttrShow attr)
                {
                        return new ApiPostFormat
                        {
                                DataType = ApiDataType.类,
                                DefValue = "Null",
                                ElementType = new ElementClass[]
                                {
                                        new ElementClass
                                        {
                                                ElementName=this.ClassName,
                                                ElementType= ElementType.对象,
                                                Id=this.ClassId
                                        }
                                },
                                ParamName = attr.AttrName,
                                ParamShow = attr.AttrShow,
                                ParamType = "object"
                        };
                }
                public void InitReturn(IApiTemplate template, Type type, string show)
                {
                        this.ProList = new IApiDataFormat[]
                       {
                                ApiHelper.GetApiDataFormat(template.ErrorCode),
                                ApiHelper.GetApiDataFormat(template.ErrorMsg)
                       };
                        if (type != null)
                        {
                                this.ProList = this.ProList.Add(ApiHelper.GetApiDataFormat(template.Data, type, show));
                        }
                }
                public void InitReturn(IApiTemplate template, ApiFuncBody body)
                {
                        this.ProList = new IApiDataFormat[]
                       {
                                ApiHelper.GetApiDataFormat(template.ErrorCode),
                                ApiHelper.GetApiDataFormat(template.ErrorMsg)
                       };
                        if (!body.Results.IsNull())
                        {
                                this.ProList = this.ProList.Add(body.Results.ConvertAll(a =>
                                {
                                        string show = XmlShowHelper.FindParamShow(body.Source, body.Method, a.ParamName);
                                        return ApiHelper.GetApiDataFormat(a, show);
                                }));
                        }
                }
                public void InitTemplate(IApiTemplate template, ClassFormat result)
                {
                        this.ProList = new IApiDataFormat[]
                       {
                                ApiHelper.GetApiDataFormat(template.ErrorCode),
                                ApiHelper.GetApiDataFormat(template.ErrorMsg)
                       };
                        if (result != null)
                        {
                                this.ProList = this.ProList.Add(result.ToDataFormat(template.Data));
                        }
                }
                public void LoadPaging(ApiFuncBody body)
                {
                        this.ClassName = "PagingResult";
                        this.ClassShow = "分页数据";
                        this.ProList = ApiHelper.GetPagingClass(body, ApiDocModular.Template);
                }
                public void LoadPaging(Type type, string show)
                {
                        Type mType = ApiHelper.GetSourceType(type);
                        this.ClassName = string.Format("PagingResult<{0}>", mType.Name);
                        this.ClassShow = "分页数据";
                        this.ProList = ApiHelper.GetPagingClass(type, ApiDocModular.Template, show);
                }
                public void LoadFormat(Type type, string name)
                {
                        this.ClassName = name ?? type.Name;
                        this.ClassShow = XmlShowHelper.FindParamShow(type);
                        this.ProList = ApiHelper.GetApiPostFormat(type);
                }
        }
}
