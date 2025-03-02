using System;
using WeDonekRpc.ApiGateway;

namespace Wedonek.Gateway
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            //全局
            GatewayServer.Global = new Global();
            //启动服务
            GatewayServer.InitApiService();
            Console.WriteLine("已启动");
            _ = Console.ReadLine();
        }
    }
}
