using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WeDonekRpc.Helper;

namespace GatewayBuildCli.Model
{
    internal class ApiGatewayBody
    {
        private readonly ApiServiceBody _Services;
        private readonly GatewayApiBody[] _ApiList;
        public ApiGatewayBody (ApiServiceBody service)
        {
            this._Services = service;
            this._ApiList = service.ApiList.ConvertAll(a => new GatewayApiBody(a, service));
        }

        public void Save (DirectoryInfo apiDir, DirectoryInfo modelDir)
        {
            this._ApiList.ForEach(a => a.SaveModel(modelDir));
            string text = this._CreateApi();
            string path = Path.Combine(apiDir.FullName, string.Concat(this._Services.RootName, "Api.cs"));
            Tools.WriteText(path, text, Encoding.UTF8);
        }
        private string _CreateApi ()
        {
            StringBuilder str = new StringBuilder();
            List<string> use =
                        [
                                "WeDonekRpc.ApiGateway.Attr",
                                "WeDonekRpc.HttpApiGateway",
                                "WeDonekRpc.Helper.Validate",
                                "WeDonekRpc.Client",
                               string.Concat(this._Services.Namespace,".Interface")
                        ];
            this._ApiList.ForEach(a =>
            {
                if (a.Use.Count > 0)
                {
                    use.AddRange(a.Use);
                }
                use.Add(a._body.Return.Type.Namespace);
            });
            string[] us = use.Distinct().ToArray();
            us.ForEach(a =>
            {
                _ = str.AppendFormat("using {0};\r\n", a);
            });
            _ = str.AppendLine();

            _ = str.AppendFormat("namespace {0}.Api\r\n", this._Services.Namespace);
            _ = str.AppendLine("{");
            _ = str.AppendFormat("\tinternal class {0}Api : ApiController\r\n", this._Services.RootName);
            _ = str.AppendLine("\t{");
            _ = str.AppendFormat("\t\tprivate I{0} _Service;\r\n\t\tpublic {1}Api(I{0} service)\r\n", this._Services.ServiceName, this._Services.RootName);
            _ = str.AppendLine("\t\t{\r\n\t\t\tthis._Service=service;\r\n\t\t}");
            this._ApiList.ForEach(a =>
            {
                a.WriteMethod(str);
                _ = str.AppendLine();
            });
            _ = str.AppendLine("\t}");
            _ = str.AppendLine("}");
            return str.ToString();
        }

    }
}
