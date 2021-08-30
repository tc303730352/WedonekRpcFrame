using System;
using System.Collections.Generic;
using System.Net;

using HttpService.Config;
using HttpService.Execution;
using HttpService.Interface;

namespace HttpService.Collect
{
        internal class ServiceCollect
        {
                static ServiceCollect()
                {
                        InitHttpExecution(ServerConfig.IsEnableLog);
                }
                /// <summary>
                /// 服务器列表
                /// </summary>
                private static readonly Dictionary<Uri, IHttpServer> _ServerList = new Dictionary<Uri, IHttpServer>();

                /// <summary>
                /// 执行项
                /// </summary>
                private static IBasicExecution _Exec = null;

                public static void AddServer(IHttpServer server)
                {
                        _ServerList.Add(server.Uri, server);
                }

                public static void Execution(IHttpServer server, HttpListenerContext context)
                {
                        _Exec.Execution(server, context);
                }

                internal static void InitHttpExecution(bool isEnable)
                {
                        ServiceCollect._Exec = isEnable ? new LogExecution() : new BasicExecution();
                }
        }
}
