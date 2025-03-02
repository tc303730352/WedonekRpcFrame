using System;
using RpcExtend.Service;
using WeDonekRpc.Helper.Log;

namespace RpcExtendApp
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            LogSystem.IsConsole = true;
            ExtendService.InitService();
            Console.WriteLine("服务已启动");
            _ = Console.ReadLine();
        }
    }
}
