using System;
using Wedonek.Demo.User.Service;
using RpcHelper;
namespace Wedonek.UserService
{
        class Program
        {
                static void Main(string[] args)
                {
                        LogSystem.IsConsole = true;
                        StartService.Start();
                        Console.ReadLine();
                }
        }
}
