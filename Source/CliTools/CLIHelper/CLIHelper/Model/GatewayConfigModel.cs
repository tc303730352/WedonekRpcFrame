using System.Text;
using WeDonekRpc.Helper;

namespace GatewayBuildCli.Model
{
    internal class GatewayConfigModel
    {
        public static void Save (string name, DirectoryInfo dir, string rootName)
        {
            StringBuilder str = new StringBuilder();
            _ = str.AppendLine("using WeDonekRpc.HttpApiGateway;");
            _ = str.AppendLine();
            _ = str.AppendFormat("namespace {0}\r\n", name);
            _ = str.AppendLine("{");
            _ = str.AppendFormat("\tinternal class {0}_ApiModular : BasicApiModular\r\n", rootName);
            _ = str.AppendLine("\t{");
            _ = str.AppendFormat("\t\tpublic {0}_ApiModular():base(\"{0}_Modular\")\r\n", rootName);
            _ = str.AppendLine("\t\t{\r\n\t\t}");
            _ = str.AppendLine("\t\tprotected override void Init()\r\n\t\t{\r\n\t\t\tthis.Config.ApiRouteFormat=\"/api/{controller}/{name}\";\r\n\t\t}");
            _ = str.AppendLine("\t}");
            _ = str.AppendLine("}");
            string path = Path.Combine(dir.FullName, string.Concat(rootName, "_ApiModular.cs"));
            Tools.WriteText(path, str.ToString(), Encoding.UTF8);
        }
    }
}
