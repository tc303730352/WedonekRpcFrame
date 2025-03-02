using DataBasicCli;

namespace DataImportTools
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            do
            {
                Console.WriteLine("功能列表：");
                Console.WriteLine("1，创建表并导入初始数据。");
                Console.WriteLine("2，创建表。");
                Console.WriteLine("3，导入初始数据。");
                Console.WriteLine("4，生成导入数据摸版。");
                Console.WriteLine("输入功能序号：");
                string res = Console.ReadLine();
                if (res == "1")
                {
                    DbServer.InitDb();
                    DbServer.InitRpcService();
                    DbServer.InitRpcExtendService();
                }
                else if (res == "2")
                {
                    DbServer.InitDb();
                }
                else if (res == "3")
                {
                    DbServer.InitRpcService();
                    DbServer.InitRpcExtendService();
                }
                else if (res == "4")
                {
                    DbServer.CreateRpcServiceTemplate();
                    DbServer.CreateRpcExtendServiceTemplate();
                }
            } while (true);
            _ = Console.ReadLine();
        }

    }
}
