using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using ApiGateway;
using ApiGateway.Model;

using HttpApiDocHelper.Helper;
using HttpApiDocHelper.Interface;
using HttpApiDocHelper.Model;

using RpcHelper;
namespace HttpApiDocHelper.Collect
{

        internal class ApiCollect
        {
                private static readonly Dictionary<string, ApiFormat> _ApiDic = new Dictionary<string, ApiFormat>();
                private static readonly ConcurrentDictionary<string, string> _ApiNameDic = new ConcurrentDictionary<string, string>();


                private static ApiModel[] _ApiList = null;

                internal static ApiModel[] ApiList
                {
                        get
                        {
                                if (_ApiList == null || _ApiList.Length != _ApiDic.Count)
                                {
                                        _ApiList = _ApiDic.Select(a => a.Value.ApiModel).ToArray();
                                }
                                return _ApiList;
                        }
                }
                internal static string GetApiName(string path, GatewayType type)
                {
                        string id = _GetApiId(path, type);
                        if (_ApiNameDic.TryGetValue(id, out string name))
                        {
                                return name;
                        }
                        string val = string.Empty;
                        ApiInfo api = ApiCollect.GetApiInfo(path.GetMd5().ToLower());
                        if (api != null && !api.BasicSet.Show.IsNull())
                        {
                                val = api.BasicSet.Show;
                        }
                        _ApiNameDic.TryAdd(path, val);
                        return val;
                }

                internal static ApiInfo GetApiInfo(string id)
                {
                        if (!_ApiDic.TryGetValue(id, out ApiFormat api))
                        {
                                return null;
                        }
                        ApiPostBody post = api.Post != null ? new ApiPostBody
                        {
                                DataType = api.Post.DataType,
                                IsValidate = api.Post.IsValidate,
                                ProList = api.Post.ClassId != null ? ClassCollect.GetClassProList(api.Post.ClassId) : api.Post.ProList,
                                IsClass = api.Post.IsClass,
                                Show = api.Post.Show
                        } : null;
                        IApiDataFormat[] gets = api.Get?.ProList;
                        return new ApiInfo
                        {
                                BasicSet = api.ApiModel,
                                SubmitBody = post,
                                GetParam = gets,
                                GetStr = ApiDataHelper.GetGetFormat(gets),
                                UpConfig = api.UpConfig == null ? null : new
                                {
                                        Extension = api.UpConfig.Extension.Join(","),
                                        api.UpConfig.MaxSize,
                                        api.UpConfig.LimitFileNum
                                },
                                JsonStr = ApiDataHelper.GetPostFormat(post),
                                ReturnBody = new ReturnBody
                                {
                                        ReturnType = api.ReturnBody.ReturnType,
                                        Show = api.ReturnBody.Show,
                                        Pros = ClassCollect.GetClassProList(api.ReturnBody.ClassId)
                                }
                        };
                }

                private static string _GetApiId(string uri, GatewayType type)
                {
                        return string.Concat(uri.ToLower(), "_", (int)type).ToLower().GetMd5().ToLower();
                }

                public static void LoadApi(ApiFuncBody param)
                {
                        RequestHeader[] headers = null;
                        if (param.IsAccredit)
                        {
                                headers = new RequestHeader[]
                                {
                                        new RequestHeader
                                        {
                                                key="accreditId",
                                                Format="由32位数字和字母组成",
                                                IsNull=false,
                                                LenShow="32位",
                                                show="授权码"
                                        }
                                };
                        }
                        if (GatewayServer.UserIdentity.IsEnableIdentity)
                        {
                                headers = new RequestHeader[]
                                {
                                        new RequestHeader
                                        {
                                                key="identityId",
                                                Format="由32-36位GUID",
                                                IsNull=false,
                                                LenShow="32-36位",
                                                show="身份识别码"
                                        }
                                };
                        }
                        string id = _GetApiId(param.ApiUri, param.GatewayType);
                        if (_ApiDic.ContainsKey(id))
                        {
                                _ApiDic.Remove(id);
                        }
                        _ApiDic.Add(id, new ApiFormat(id, param, headers));
                }
        }
}
