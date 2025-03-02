using System.Text;
using WeDonekRpc.Helper;

namespace GatewayBuildCli.Model
{
    internal class GatewayApiBody
    {
        public ApiBody _body;
        private readonly ApiServiceBody _service;
        private readonly string _Prefix;
        public GatewayApiBody ( ApiBody body, ApiServiceBody service )
        {
            this._service = service;
            this._body = body;
            string root = this._service.Root.ToLower();
            if ( root.EndsWith("service") )
            {
                root = root.Substring(0, root.Length - 7);
            }
            this._Prefix = string.Concat(root, ".", this._service.RootName.ToLower());
        }
        private string _ModelName;

        public List<string> Use { get; } = [];

        public void WriteMethod ( StringBuilder str )
        {
            StringBuilder arg = new StringBuilder();
            int mode = 2;
            if ( !this._ModelName.IsNull() )
            {
                mode = 0;
                _ = arg.AppendFormat("[NullValidate(\"{1}.param.null\")]{0} param", this._ModelName, this._Prefix);
            }
            else if ( this._body.Params.Length > 0 )
            {
                this._body.Params.ForEach(c =>
               {
                   if ( c.ParamType == "basic" )
                   {
                       _ = arg.AppendFormat("{1}{0},", c.ToString(), c.GetValidate(this._Prefix));
                   }
               });
                if ( arg[^1] == ',' )
                {
                    mode = 1;
                    _ = arg.Remove(arg.Length - 1, 1);
                }
            }
            string name = this._body.ApiName.Replace(this._service.RootName, string.Empty);
            Helper.WriteApiShow(this._body, mode, str);
            _ = str.Append("\t\t");
            _ = this._body.IsPaging
                ? str.AppendFormat("public PagingResult<{0}> {1}({2})\r\n", Helper.FormatTypeName(this._body.Return.Type), name, arg.ToString())
                : str.AppendFormat("public {0} {1}({2})\r\n", this._body.Return.ToString(), name, arg.ToString());
            _ = str.AppendLine("\t\t{");
            _ = str.Append("\t\t\t");
            if ( this._body.IsPaging )
            {
                string rName = Helper.FormatTypeName(this._body.Return.Type);
                _ = str.AppendFormat(" {0}[] results = this._Service.{1}(", rName, this._body.ApiName);
                this._body.Params.ForEach(a =>
                {
                    if ( a.Name == "count" )
                    {
                        _ = str.Append("out int " + a.Name);
                        _ = str.Append(",");
                    }
                    else if ( a.ParamType == "paging" )
                    {
                        _ = str.Append("param,");
                    }
                    else if ( mode == 1 )
                    {
                        _ = str.Append(a.Name);
                        _ = str.Append(",");
                    }
                    else
                    {
                        _ = str.AppendFormat("param.{0},", a.SourceProName);
                    }
                });
                _ = str.Remove(str.Length - 1, 1);
                _ = str.AppendLine(");");
                _ = str.AppendFormat("\t\t return new PagingResult<{0}>(count, results);\r\n", rName);
            }
            else
            {
                if ( this._body.Return.Type != ConstDic._VoidType )
                {
                    _ = str.Append("return ");
                }
                _ = str.AppendFormat("this._Service.{0}", this._body.ApiName);
                if ( arg.Length == 0 )
                {
                    _ = str.AppendLine("();");
                }
                else
                {
                    _ = str.Append("(");
                    this._body.Params.ForEach(a =>
                    {
                        if ( mode == 1 )
                        {
                            _ = str.Append(a.Name);
                            _ = str.Append(",");
                        }
                        else
                        {
                            _ = str.AppendFormat("param.{0},", a.SourceProName);
                        }
                    });
                    _ = str.Remove(str.Length - 1, 1);
                    _ = str.AppendLine(");");
                }
            }
            _ = str.AppendLine("\t\t}");

        }
        private string CreateUi_Model ()
        {
            StringBuilder str = new StringBuilder();
            this._body.UseList.ForEach(a =>
            {
                _ = str.AppendFormat("using {0};\r\n", a);
            });
            _ = str.Append("using WeDonekRpc.Helper.Validate;");
            _ = str.AppendLine();
            string use = string.Concat(this._service.Namespace, ".Model.", this._service.RootName);
            this.Use.Add(use);
            _ = str.AppendFormat("namespace {0}\r\n", use);
            _ = str.AppendLine("{");
            this._ModelName = string.Concat("UI_", this._body.ApiName);
            Helper.WriteUiShow(this._body, str);
            _ = this._body.IsPaging
                ? str.AppendFormat("\tinternal class {0} : BasicPage\r\n", this._ModelName)
                : str.AppendFormat("\tinternal class {0}\r\n", this._ModelName);
            _ = str.AppendLine("\t{");
            this._body.Params.ForEach(a =>
            {
                if ( a.CheckIsWrite() )
                {
                    Helper.WriteShow(a, str);
                    a.WriteProperty(str, this._Prefix);
                    _ = str.AppendLine();
                }
            });
            _ = str.AppendLine("\t}");
            _ = str.AppendLine("}");
            return str.ToString();
        }

        public void SaveModel ( DirectoryInfo dir )
        {
            if ( this._body.IsPaging && this._body.Params.Count(a => a.ParamType == "basic") == 0 )
            {
                this._ModelName = "BasicPage";
                this.Use.Add("WeDonekRpc.Model");
                return;
            }
            else if ( !this._body.IsPaging && this._body.Params.Count(a => a.ParamType == "basic") <= 1 )//控制生成UI实体的参数量
            {
                this.Use.Add("WeDonekRpc.Helper.Validate");
                this._body.Params.ForEach(a =>
                {
                    if ( a.ParamType == "basic" )
                    {
                        this.Use.Add(a.Type.Namespace);
                    }
                });
                return;
            }
            string text = this.CreateUi_Model();
            string path = Path.Combine(dir.FullName, string.Concat(this._service.RootName, @"\", this._ModelName, ".cs"));
            Tools.WriteText(path, text, Encoding.UTF8);
        }
    }
}
