using System;

using HttpApiDocHelper.Collect;
using HttpApiDocHelper.Helper;
using HttpApiDocHelper.Interface;
using HttpApiDocHelper.Model;

using HttpApiGateway;
using ApiGateway.Interface;
using ApiGateway.Model;

using RpcHelper;
using ApiGateway;

namespace HttpApiDocHelper
{
        public class ApiDocModular : BasicApiModular, IApiDocModular
        {

                public ApiDocModular() : base("ApiDoc")
                {

                }

                public static IApiTemplate Template
                {
                        get;
                } = new ApiTemplate();

                public string GetApiShow(Uri uri)
                {
                        return this.GetApiShow(uri.AbsolutePath, GatewayType.Http);
                }
                public string GetApiShow(string path, GatewayType type)
                {
                        return ApiCollect.GetApiName(path, type);
                }

                public void RegApi(ApiFuncBody api)
                {
                        ApiCollect.LoadApi(api);
                }

                public void RegModular(string name, Type source, Uri root)
                {
                        string show = XmlShowHelper.FindParamShow(source);
                        if (show.IsNull())
                        {
                                show = string.Concat(name, "-", source.Name);
                        }
                        ApiGroupCollect.AddGroup(new ApiGroup(show, root, source.FullName));
                }
        }
}
