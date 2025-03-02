using System;
using System.IO;
using GatewayBuildCli;

namespace GatewayBuildCliTools
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            Console.WriteLine("欢迎生成网关生成器");
            Console.WriteLine("输入交互DLL路径：");
            string path = Console.ReadLine();
            if (!File.Exists(path))
            {
                Console.WriteLine("文件不存在!");
                return;
            }

            GenerateConfig config = new GenerateConfig
            {
                FilePath = path,
                Output = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OutPut"))
            };
            CLIService.CreateApiGateway(config);
            Console.WriteLine("已完成!");
            Console.WriteLine("输出路径:" + config.Output.FullName);
            _ = Console.Read();
        }
    }
}
