using HttpApiDocHelper;
using HttpApiGateway;
using RpcClient.Model;
using System;

namespace ConsoleApp
{
        class Program
        {
                static  void Main()
                {
                        ToolsHelper.LogSystem.IsConsole = true;
                        ApiGatewayService.Config.GatewayUri = new Uri("http://127.0.0.1:87/");
                        ApiGatewayService.RegDoc(new ApiDocModular());
                        ApiGatewayService.RegModular(new ApiDemoModular.DemoModular());
                        ApiGatewayService.InitApiService();
                        Console.ReadLine();
                }
        }
}
