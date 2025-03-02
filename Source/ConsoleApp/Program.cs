using System.Diagnostics;

namespace ConsoleApp
{

    internal class Program
    {
        private static readonly string[] _Paths = new string[]
        {
            @"BasicService\RpcService\CentralService\bin\{type}\net8.0\CentralService.exe",
            @"BasicService\RpcSyncService\RpcBasicService\bin\{type}\net8.0\RpcBasicService.exe",
            @"BasicService\ExtendService\RpcExtendApp\bin\{type}\net8.0\RpcExtendApp.exe",
            @"AttachService\TaskService\AutoTaskService\AutoTaskService\bin\{type}\net8.0\AutoTaskService.exe",
            @"StoreService\Store\RpcStoreApp\bin\{type}\net8.0\RpcStoreApp.exe",
            @"StoreService\Store\WeDonekRpc.StoreGateway\bin\{type}\net8.0\WeDonekRpc.StoreGateway.exe"
        };
        private static void Main ( string[] args )
        {
            //string path = @"D:\自研项目\Github基础系统\Source\BasicService\RpcService\CentralService\bin\Release\net8.0\CentralService.exe";
            DirectoryInfo dir = _GetRootDir();
            if ( dir == null )
            {
                Console.WriteLine("未找到源代码目录!");
                _ = Console.ReadLine();
                return;
            }
            Console.WriteLine("1，启动Release目录的程序!");
            Console.WriteLine("2，启动Debug目录的程序!");
            string res = Console.ReadLine();
            if ( res == "2" )
            {
                res = "Debug";
            }
            else
            {
                res = "Release";
            }
            foreach ( string c in _Paths )
            {
                string path = Path.Combine(dir.FullName, c.Replace("{type}", res));
                FileInfo fileInfo = new FileInfo(path);
                if ( !fileInfo.Exists )
                {
                    Console.WriteLine("未找到程序（" + Path.GetFileNameWithoutExtension(path) + "）请先进行编译!");
                    return;
                }
                ProcessStartInfo start = new ProcessStartInfo(fileInfo.FullName);
                start.WindowStyle = ProcessWindowStyle.Normal;
                start.RedirectStandardOutput = false;
                start.RedirectStandardError = false;
                start.RedirectStandardInput = false;
                start.UseShellExecute = true;
                start.CreateNoWindow = false;
                start.WorkingDirectory = fileInfo.Directory.FullName;
                using ( Process process = Process.Start(start) )
                {
                    Console.WriteLine("程序：" + Path.GetFileNameWithoutExtension(path) + " 启动成功!");
                }
                Thread.Sleep(1500);
            }
            Console.WriteLine("启动完成!");
            _ = Console.ReadLine();
        }
        private static DirectoryInfo _GetRootDir ()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            if ( dir.Name != "Source" )
            {
                DirectoryInfo[] dirs = dir.GetDirectories("Source", SearchOption.TopDirectoryOnly);
                if ( dirs != null && dirs.Length != 0 )
                {
                    return dirs[0];
                }
                do
                {
                    if ( dir.Parent == null )
                    {
                        return null;
                    }
                    dir = dir.Parent;
                    if ( dir.Name == "Source" )
                    {
                        return dir;
                    }
                } while ( true );
            }
            return dir;
        }
    }
}
