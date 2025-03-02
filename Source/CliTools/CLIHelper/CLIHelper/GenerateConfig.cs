using System.Reflection;
using WeDonekRpc.Helper;

namespace GatewayBuildCli
{
    public class GenerateConfig
    {
        /// <summary>
        /// DLL文件路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }
        /// <summary>
        /// 输出目录
        /// </summary>
        public DirectoryInfo Output
        {
            get;
            set;
        }

        internal Assembly GetAssemblie ()
        {
            FileInfo file = new FileInfo(this.FilePath);
            if (!file.Exists)
            {
                return null;
            }
            return Tools.LoadAssembly(file);
        }
    }
}
