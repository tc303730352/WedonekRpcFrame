using System;
using WeDonekRpc.Helper.Log;

namespace AutoTaskService
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            LogSystem.IsConsole = true;
            AutoTask.Service.AutoTaskService.InitService();
            Console.WriteLine("服务已启动");
            _ = Console.ReadLine();
        }
    }
}
