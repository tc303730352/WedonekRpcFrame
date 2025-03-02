using WeDonekRpc.Helper.Log;

namespace RpcStoreApp
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            LogSystem.IsConsole = true;
            RpcStore.Service.RpcStoreService.Start();
            Console.WriteLine("服务已启动");
            _ = Console.ReadLine();
        }
    }
}