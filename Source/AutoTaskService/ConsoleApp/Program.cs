using System;

namespace ConsoleApp
{
        internal class Program
        {
                private static void Main(string[] args)
                {
                        RpcHelper.LogSystem.IsConsole = true;
                        AutoTaskService.AutoTaskService.InitService();
                        Console.ReadLine();
                }
        }
}
