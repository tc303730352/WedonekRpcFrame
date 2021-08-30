using System;
using Wedonek.Demo.Service;

namespace Wedonek.OrderService
{
        class Program
        {
                static void Main()
                {
                        RpcHelper.LogSystem.IsConsole = true;
                        StartService.Start();
                        Console.ReadLine();
                }
        }
}
