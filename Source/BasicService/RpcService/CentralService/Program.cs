using System;

namespace CentralService
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            RpcCentral.Service.CentralService.Start();
            Console.WriteLine("服务已启用!");
            _ = Console.ReadLine();
        }
    }
}
