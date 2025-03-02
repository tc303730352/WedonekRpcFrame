using System.Reflection;
using GatewayBuildCli.Model;

namespace GatewayBuildCli
{
    public class CLIService
    {
        public static void CreateApiGateway ( GenerateConfig config )
        {
            Assembly assembly = config.GetAssemblie();
            if ( assembly == null )
            {
                return;
            }
            FileInfo file = new FileInfo(config.FilePath);
            XmlShowHelper.BasicPath = file.Directory.FullName;
            AssemblyBody body = new AssemblyBody(assembly);
            body.Init();
            body.Save(config.Output, file);
        }
    }
}
