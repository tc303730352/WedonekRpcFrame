using System;

using ApiGateway;
using ApiGateway.Interface;
using ApiGateway.Model;

using HttpApiGateway.Interface;
using HttpApiGateway.Model;

namespace HttpApiGateway
{
        public class ApiGatewayService
        {
                private static IApiDocModular _ApiDoc => RpcClient.RpcClient.Unity.Resolve<IApiDocModular>();
                private static readonly IHttpConfig _Config = new Config.HttpConfig();
                public static IHttpConfig Config => _Config;

                static ApiGatewayService()
                {
                        GatewayServer.Starting += GatewayServer_Starting;
                        GatewayServer.Closeing += GatewayServer_Closeing;
                }

                private static void GatewayServer_Closeing()
                {
                        HttpService.HttpService.StopService();
                }

                private static void GatewayServer_Starting()
                {
                        HttpService.HttpService.RegService(Config.Url);
                }

                internal static string GetApiShow(Uri uri)
                {
                        if (_ApiDoc == null)
                        {
                                return string.Empty;
                        }
                        return _ApiDoc.GetApiShow(uri);
                }

                internal static bool ReceiveRequest(IApiService service)
                {
                        if (_Config.MaxRequstLength != 0 && service.Request.ContentLength > _Config.MaxRequstLength)
                        {
                                service.Response.SetHttpStatus(System.Net.HttpStatusCode.RequestEntityTooLarge);
                                return false;
                        }
                        CrossDomainConfig cross = _Config.CrossDomain;
                        Uri uri = service.UrlReferrer;
                        if (uri != null && cross.CheckUrlReferrer(uri))
                        {
                                string header = service.Request.Headers["Access-Control-Request-Headers"];
                                if (string.IsNullOrEmpty(header))
                                {
                                        header = cross.AllowHead;
                                }
                                service.Response.SetHead("Access-Control-Allow-Credentials", "true");
                                service.Response.SetHead("Access-Control-Allow-Headers", header);
                                service.Response.SetHead("Access-Control-Allow-Origin", string.Format("{1}://{0}", uri.Authority, uri.Scheme));
                                service.Response.SetHead("Access-Control-Max-Age", cross.MaxAge);
                                service.Response.SetHead("Access-Control-Request-Method", cross.Method);
                                return service.Request.HttpMethod != "OPTIONS";
                        }
                        else
                        {
                                return cross.AllowCredentials;
                        }
                }




                internal static void RegModular(string name, Type source)
                {
                        if (_ApiDoc != null)
                        {
                                _ApiDoc.RegModular(name, source, _Config.Url);
                        }
                }
                internal static void RegApi(ApiFuncBody api)
                {
                        if (_ApiDoc != null)
                        {
                                _ApiDoc.RegApi(api);
                        }
                }
        }
}
