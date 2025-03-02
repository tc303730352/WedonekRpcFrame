using System;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiDoc.Collect;
using WeDonekRpc.HttpApiDoc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiDoc.Model
{
    internal class ApiFormat
    {
        private static readonly string _IResponseType = typeof(IResponse).FullName;
        private static readonly IHttpConfig _Config;

        static ApiFormat ()
        {
            _Config = RpcClient.Ioc.Resolve<IHttpConfig>();
        }

        public ApiFormat (string id, ApiFuncBody param, RequestHeader[] header)
        {
            this.UpConfig = param.UpConfig;
            this.ApiModel = new ApiModel
            {
                ApiType = param.ApiType,
                Header = header,
                Id = id,
                GatewayType = param.GatewayType,
                Uri = new Uri(_Config.RealRequestUri, param.ApiUri),
                GroupId = ApiGroupCollect.GetGroup(param.Source),
                Show = XmlShowHelper.FindParamShow(param.Source, param.Method),
                IsAccredit = param.IsAccredit,
                IsIdentity = GatewayServer.UserIdentity.IsEnableIdentity,
                Prower = param.Prower?.Join(',')
            };
            if (param.GatewayType == GatewayType.WebSocket)
            {
                this.ApiModel.RequestMethod = "WebSocket";
            }
            else
            {
                if (param.ApiType == ApiType.文件上传 || param.PostParam.IsExists(a => a.ReceiveMethod == "POST"))
                {
                    this.ApiModel.RequestMethod = "POST";
                }
                else
                {
                    this.ApiModel.RequestMethod = "GET";
                }
            }
            this._InitHead();
            this._InitGetParam(param);
            this._InitPostParam(param);
            this._InitReturnParam(param);
        }
        public ApiUpSet UpConfig { get; }
        public ApiPostBody Get { get; private set; }
        public ApiPostBody Post { get; private set; }

        public ApiModel ApiModel
        {
            get;
        }
        public ApiReturnBody ReturnBody
        {
            get;
            private set;
        } = new ApiReturnBody();

        private void _InitPostParam (ApiPostParam param)
        {
            if (this.Post.DataType == ApiRequestType.数组)
            {
                Type type = ApiHelper.GetSourceType(param.PostType);
                this.Post.IsValidate = DataValidateHepler.CheckIsValidate(type);
                this.Post.ClassId = ClassCollect.LoadClass(type);
                if (this.Post.ClassId == null)
                {
                    this.Post.ProList = new ApiPostFormat[]
                    {
                                              ApiHelper.GetApiPostFormat(type,null,"数组成员类型")
                    };
                }
                else
                {
                    this.Post.IsClass = true;
                }
            }
            else if (this.Post.DataType == ApiRequestType.对象)
            {
                this.Post.IsClass = true;
                this.Post.IsValidate = DataValidateHepler.CheckIsValidate(param.PostType);
                this.Post.ClassId = ClassCollect.LoadClass(param.PostType);
            }
        }
        private void _InitHead ()
        {
            RequestHeader[] headers = null;
            if (this.ApiModel.IsAccredit)
            {
                headers = new RequestHeader[]
                {
                                        new RequestHeader
                                        {
                                                Format="32位数字或字母组成",
                                                IsNull=false,
                                                key="accreditId",
                                                LenShow="32位",
                                                show="授权码"
                                        }
                };
            }
            if (this.ApiModel.ApiType == ApiType.API接口 && this.ApiModel.RequestMethod == "POST")
            {
                headers = headers.Add(new RequestHeader
                {
                    IsNull = false,
                    key = "content-type",
                    show = "类型头",
                    value = "application/json"
                });
            }
            this.ApiModel.Header = headers;
        }
        private bool _InitGetParam (ApiPostFormat param, int index, ApiFuncBody body, Type type)
        {
            if (MethodValidateHelper.CheckIsValidate(body.Method, index, out IValidateAttr[] attrs))
            {
                param.Init(attrs, type);
                return true;
            }
            return false;
        }
        private void _InitGetParam (ApiFuncBody body)
        {
            if (body.PostParam.IsNull())
            {
                return;
            }
            ApiPostParam[] param = body.PostParam.FindAll(a => a.ReceiveMethod != "POST");
            if (param.Length == 0)
            {
                return;
            }
            this.Get = new ApiPostBody
            {
                DataType = ApiRequestType.字典
            };
            bool isValidate = false;
            this.Get.ProList = param.ConvertAll(a =>
            {
                string show = XmlShowHelper.FindParamShow(body.Source, body.Method, a.Name);
                ApiPostFormat get = ApiHelper.GetApiPostFormat(a.PostType, a.Name, show);
                if (this._InitGetParam(get, a.ParamIndex, body, a.PostType))
                {
                    isValidate = true;
                }
                return get;
            });
            this.Get.IsValidate = isValidate;
        }
        private void _InitPostParam (ApiFuncBody body)
        {
            if (body.PostParam.IsNull())
            {
                return;
            }
            ApiPostParam param = body.PostParam.Find(a => a.ReceiveMethod == "POST");
            if (param != null)
            {
                this.Post = new ApiPostBody
                {
                    DataType = ApiHelper.GetRequestType(param.PostType),
                    Show = XmlShowHelper.FindParamShow(body.Source, body.Method, param.Name)
                };
                this._InitPostParam(param);
            }
        }
        private void _InitReturnParam (ResultBody result, ApiFuncBody body)
        {
            if (result.ResultType.FullName == _IResponseType || result.ResultType.GetInterface(_IResponseType) != null)
            {
                this.ReturnBody.ReturnType = ApiResultHelper.GetReturnType(result.ResultType);
                this.ReturnBody.Show = XmlShowHelper.FindParamShow(body.Source, body.Method, result.ParamName);
                return;
            }
            else
            {
                string show = XmlShowHelper.FindParamShow(body.Source, body.Method, result.ParamName);
                this.ReturnBody.ClassId = ClassCollect.RegResultClass(result.ResultType, this.ApiModel.Id, show);
            }
        }
        private void _InitReturnParam (ApiFuncBody body)
        {
            if (!body.Results.IsNull() && body.Results.Length == 1)
            {
                this._InitReturnParam(body.Results[0], body);
            }
            else
            {
                this.ReturnBody.ClassId = ClassCollect.RegResultClass(body, this.ApiModel.Id);
            }
        }

    }
}
