using System;
using System.Collections.Generic;
using System.Net;
using WeDonekRpc.HttpService.Config;
using WeDonekRpc.HttpService.Execution;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.Collect
{
    internal class ServiceCollect
    {
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

        internal static void InitHttpExecution(LogConfig config)
        {
            ServiceCollect._Exec = config.IsEnable ? new LogExecution(config) : new BasicExecution();
        }
    }
}
