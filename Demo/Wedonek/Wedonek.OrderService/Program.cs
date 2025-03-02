using System;
using Wedonek.Demo.Service;

namespace Wedonek.OrderService
{
    internal class Program
    {
        private static void Main ()
        {
            StartService.Start();
            Console.WriteLine("已启动");
            _ = Console.ReadLine();
        }
    }
}
