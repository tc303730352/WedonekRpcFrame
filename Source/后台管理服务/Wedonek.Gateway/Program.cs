using System;

using ApiGateway;

using RpcHelper;

namespace Wedonek.Gateway
{
        internal class Program
        {
                private static void Main(string[] args)
                {
                        LogSystem.IsConsole = true;
                        //全局
                        GatewayServer.Global = new Global();
                        //启动服务
                        GatewayServer.InitApiService();
                        Console.ReadLine();
                }
        }
}
