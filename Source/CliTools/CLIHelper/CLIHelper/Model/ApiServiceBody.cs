using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WeDonekRpc.Helper;

namespace GatewayBuildCli.Model
{
    internal class ApiServiceBody
    {
        private readonly string _ServiceName;

        private readonly string _Namespace;
        private ApiBody[] _ApiList;

        public string ServiceName => this._ServiceName;
        public string RootName
        {
            get;
        }
        public string Root
        {
            get;
        }
        internal ApiBody[] ApiList { get => this._ApiList; }

        public string Namespace => this._Namespace;

        public ApiServiceBody (string name, string namespaces, string root)
        {
            this.Root = root;
            char[] chars = name.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            this.RootName = new string(chars);
            this._Namespace = namespaces;
            if (this.RootName.EndsWith("Service"))
            {
                this._ServiceName = this.RootName;
            }
            else
            {
                this._ServiceName = string.Concat(this.RootName, "Service");
            }
        }

        public void Init (Type[] types)
        {
            this._ApiList = types.ConvertAll(a =>
            {
                return new ApiBody(a);
            });
        }
        public void Save (DirectoryInfo serviceDir, DirectoryInfo interDir)
        {
            string csValue = this._CreateClass();
            string path = Path.Combine(serviceDir.FullName, string.Concat(this._ServiceName, ".cs"));
            Tools.WriteText(path, csValue, Encoding.UTF8);
            path = Path.Combine(interDir.FullName, string.Concat("I", this._ServiceName, ".cs"));
            string interValue = this._CreateInterface();
            Tools.WriteText(path, interValue, Encoding.UTF8);
        }
        private string _CreateInterface ()
        {
            StringBuilder str = new StringBuilder();
            List<string> use = [];
            this._ApiList.ForEach(a =>
            {
                use.AddRange(a.UseList);
            });
            string[] us = use.Distinct().ToArray();
            us.ForEach(a =>
            {
                _ = str.AppendFormat("using {0};\r\n", a);
            });
            _ = str.AppendLine();
            _ = str.AppendFormat("namespace {0}.Interface\r\n", this._Namespace);
            _ = str.AppendLine("{");
            _ = str.AppendFormat("\tpublic interface I{0}\r\n", this._ServiceName);
            _ = str.AppendLine("\t{");
            this._ApiList.ForEach(a =>
            {
                a.WriteBody(str);
                _ = str.AppendLine();
            });
            _ = str.AppendLine("\t}");
            _ = str.AppendLine("}");
            return str.ToString();
        }
        private string _CreateClass ()
        {
            StringBuilder str = new StringBuilder();
            List<string> use = [];
            this._ApiList.ForEach(a =>
            {
                use.AddRange(a.UseList);
            });
            string[] us = use.Distinct().ToArray();
            us.ForEach(a =>
            {
                _ = str.AppendFormat("using {0};\r\n", a);
            });
            _ = str.AppendFormat("using {0}.Interface;\r\n", this._Namespace);
            _ = str.AppendLine();
            _ = str.AppendFormat("namespace {0}.Services\r\n", this._Namespace);
            _ = str.AppendLine("{");
            _ = str.AppendFormat("\tinternal class {0} : I{0}\r\n", this._ServiceName);
            _ = str.AppendLine("\t{");
            this._ApiList.ForEach(a =>
            {
                a.WriteMothed(str);
                _ = str.AppendLine();
            });
            _ = str.AppendLine("\t}");
            _ = str.AppendLine("}");
            return str.ToString();
        }
    }
}
