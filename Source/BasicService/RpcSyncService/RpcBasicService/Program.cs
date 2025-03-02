using System;

namespace RpcBasicService
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            RpcSync.Service.RpcSyncService.Start();
            Console.WriteLine("已启动");
            _ = Console.Read();
        }
    }
}
