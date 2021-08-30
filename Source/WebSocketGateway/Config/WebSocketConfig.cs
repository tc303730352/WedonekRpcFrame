
using RpcClient.Interface;

namespace WebSocketGateway.Config
{
        internal class WebSocketConfig
        {
                private static DefSocketConfig _DefConfig = null;
                static WebSocketConfig()
                {
                        RpcClient.RpcClient.Config.AddRefreshEvent(_Refresh);
                }

                private static void _Refresh(IConfigServer config, string name)
                {
                        if (name.StartsWith("gateway:socket") || name == string.Empty)
                        {
                                _DefConfig = RpcClient.RpcClient.Config.GetConfigVal<DefSocketConfig>("gateway:socket", new DefSocketConfig());
                        }
                }


                /// <summary>
                ///  Api 接口路径生成格式
                /// </summary>
                public static string ApiRouteFormat => _DefConfig.ApiRouteFormat;
                public static string RequestEncoding => _DefConfig.RequestEncoding;

        }
}
