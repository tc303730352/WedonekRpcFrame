using System;

using ApiGateway.Collect;
using ApiGateway.Interface;
using ApiGateway.Model;

using RpcHelper;

namespace ApiGateway
{
        public class GatewayServer
        {
                private static IApiDocModular _ApiDoc = null;
                public static event Action Starting;
                public static event Action Closeing;
                /// <summary>
                /// 配置信息
                /// </summary>
                public static IGatewayConfig Config
                {
                        get;
                } = new Config.GatewayConfig();

                public static IUserIdentityCollect UserIdentity
                {
                        get;
                } = new UserIdentityCollect();

                /// <summary>
                /// 全局事件
                /// </summary>
                public static IGlobal Global
                {
                        get;
                        set;
                } = new BasicGlobal();

                internal static string GetApiShow(Uri uri)
                {
                        if (_ApiDoc == null)
                        {
                                return string.Empty;
                        }
                        return _ApiDoc.GetApiShow(uri);
                }

                internal static void RegDoc(IApiDocModular doc)
                {
                        if (_ApiDoc == null)
                        {
                                _ApiDoc = doc;
                                RpcClient.RpcClient.Unity.RegisterInstance(doc);
                                RegModular(doc);
                        }
                }

                public static T GetModular<T>(string name) where T : IModular
                {
                        return ModularCollect.GetModular<T>(name);
                }
                /// <summary>
                /// 停止服务
                /// </summary>
                public static void StopApiService()
                {
                        ModularCollect.Close();
                        Closeing?.Invoke();
                        RpcClient.RpcClient.Close();
                        Global.ServiceClose();
                }
                /// <summary>
                /// 初始化Api服务
                /// </summary>
                public static void InitApiService()
                {
                        RpcClient.RpcClient.InitComplate += RpcClient_InitComplate;
                        RpcClient.RpcClient.Start();
                }

                private static void RpcClient_InitComplate()
                {
                        RpcClient.RpcClient.Unity.RegisterType(typeof(IClientIdentity), typeof(ClientIdentity));
                        IGatewayService service = new GatewayService();
                        Global.ServiceStarting(service);
                        Global.LoadModular(service);
                        ModularCollect.Start();
                        if (Starting != null)
                        {
                                Starting();
                        }
                        Global.ServiceStarted(service);
                }
                internal static void RegModular(IModular modular)
                {
                        try
                        {
                                ModularCollect.RegModular(modular);
                        }
                        catch (Exception e)
                        {
                                ErrorException error = ErrorException.FormatError(e);
                                GatewayLog.AddErrorLog(error, modular);
                        }
                }


        }
}
