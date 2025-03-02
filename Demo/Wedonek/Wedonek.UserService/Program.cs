using System;
using Wedonek.Demo.User.Service;
namespace Wedonek.UserService
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            StartService.Start();
            Console.WriteLine("已启动");
            _ = Console.ReadLine();
        }
    }
}
