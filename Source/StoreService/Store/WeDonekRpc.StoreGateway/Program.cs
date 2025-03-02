using System;
using WeDonekRpc.ApiGateway;

namespace WeDonekRpc.StoreGateway
{
    internal class Program
    {
        private static void Main ( string[] args )
        {
            //全局
            GatewayServer.Global = new Global();
            //启动服务
            GatewayServer.InitApiService();
            _ = Console.ReadLine();
        }
    }
}
