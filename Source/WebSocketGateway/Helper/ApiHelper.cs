using System;
using System.Reflection;
using System.Text;

using ApiGateway.Interface;

using RpcHelper;

using WebSocketGateway.Collect;
using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace WebSocketGateway.Helper
{
        internal class ApiHelper
        {
                /// <summary>
                /// 获取API接口路径
                /// </summary>
                /// <param name="config"></param>
                /// <param name="name"></param>
                /// <param name="modular"></param>
                /// <returns></returns>
                internal static string GetApiPath(IApiConfig config, string name, IApiModular modular)
                {
                        StringBuilder str = new StringBuilder(modular.Config.ApiRouteFormat);
                        str.Replace("{service}", modular.ServiceName);
                        str.Replace("{controller}", _FormatApiName(config.Name));
                        str.Replace("{name}", name);
                        str.Replace("//", "/");
                        return str.ToString();
                }
                /// <summary>
                /// 加载模块
                /// </summary>
                /// <param name="modular"></param>
                public static void LoadModular(IApiModular modular)
                {
                        Type type = modular.GetType();
                        Assembly assembly = type.Assembly;
                        RpcClient.RpcClient.Unity.RegisterInstance<IApiModular>(modular, modular.ServiceName);
                        RpcClient.RpcClient.Unity.RegisterInstance<ICurrentModular>(new CurrentModular(modular), modular.ServiceName);
                        RpcClient.RpcClient.Load(assembly);
                        Type[] types = assembly.GetTypes();
                        using (IResourceCollect resource = ResourceCollect.Create(modular))
                        {
                                types.ForEach(a =>
                                {
                                        if (a.IsInterface)
                                        {
                                                return;
                                        }
                                        else if (UnityCollect.RegisterApi(a))
                                        {
                                                IApiController api = UnityCollect.GetApi(a);
                                                RouteCollect.RegApi(a, api, modular, resource.RegRoute);
                                        }
                                        else if (UnityCollect.RegisterGateway(a))
                                        {
                                                RouteCollect.RegApi(a, modular, resource.RegRoute);
                                        }
                                });
                        }
                }
                private static string _FormatApiName(string name)
                {
                        name = name.ToLower();
                        if (name.EndsWith("api") && name != "api")
                        {
                                name = name.Remove(name.Length - 3);
                        }
                        else if (name.EndsWith("controller"))
                        {
                                name = name.Remove(name.Length - 10);
                        }
                        else if (name.EndsWith("service"))
                        {
                                name = name.Remove(name.Length - 7);
                        }
                        return name;
                }

                public static IUserPage GetPage(byte[] content, out byte[] value)
                {
                        if (content.Length == 0)
                        {
                                value = null;
                                return null;
                        }
                        int index = content.FindIndex(a => a == 10);
                        if (index == -1)
                        {
                                value = null;
                                return null;
                        }
                        int begin = index + 1;
                        int len = 0;
                        string pageId = null;
                        if (content[begin] == 48)
                        {
                                begin += 2;
                                len = content.Length - begin;
                        }
                        else
                        {
                                int end = content.FindIndex(begin, a => a == 10);
                                if (end == -1)
                                {
                                        value = null;
                                        return null;
                                }
                                pageId = Encoding.UTF8.GetString(content, begin, end - begin);
                                begin = end + 1;
                                len = content.Length - begin;
                        }
                        string direct = Encoding.UTF8.GetString(content, 0, index);
                        value = new byte[len];
                        if (len > 0)
                        {
                                Buffer.BlockCopy(content, begin, value, 0, len);
                        }
                        return new UserPage
                        {
                                Direct = direct,
                                PageId = pageId
                        };
                }
        }
}
