using System.Reflection;
using System.Text;
using WeDonekRpc.Helper;

namespace GatewayBuildCli.Model
{
    internal class ApiBody
    {
        public ApiBody ( Type type )
        {
            this.Source = type;
            this.ApiName = Helper.GetApiName(type);
            this.Return = Helper.GetReturnType(type, out bool isPaging);
            this.IsPaging = isPaging;
            this.Show = XmlShowHelper.FindParamShow(type);
            List<string> use =
                        [
                                type.Namespace,
                                this.Return.Type.Namespace,
                        ];

            PropertyInfo[] pros = type.GetProperties().FindAll(Helper.CheckProperty);
            this.Params = pros.Convert(a =>
            {
                if ( this.IsPaging && a.DeclaringType.Name == ConstDic._BasicPageType.Name )
                {
                    return null;
                }
                return new ApiParam(a);
            });
            if ( this.IsPaging )
            {
                this.Params = this.Params.Add(new ApiParam[] {
                                        new ApiParam("paging"),
                                        new ApiParam("count"),
                                });
                use.Add("RpcModel");
            }
            this.Params.ForEach(a =>
            {
                use.Add(a.Type.Namespace);
            });
            this.UseList = use.Distinct().ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPaging
        {
            get;
            set;
        }
        public string[] UseList
        {
            get;
            set;
        }
        public string ApiName
        {
            get;
            set;
        }
        public string Show
        {
            get;
            set;
        }
        public ReturnTypeBody Return
        {
            get;
            set;
        }
        public ApiParam[] Params
        {
            get;
            set;
        }
        public Type Source
        {
            get;
            set;
        }
        public void WriteBody ( StringBuilder str )
        {
            StringBuilder arg = new StringBuilder();
            this.Params.ForEach(a =>
            {
                _ = arg.Append(",");
                _ = arg.Append(a.ToString());
            });
            if ( arg.Length > 0 )
            {
                _ = arg.Remove(0, 1);
            }
            Helper.WriteShow(this, str);
            _ = str.AppendFormat("\t\t{0} {1}({2});\r\n", this.Return.ToString(), this.ApiName, arg.ToString());

        }

        public void WriteMothed ( StringBuilder str )
        {
            StringBuilder arg = new StringBuilder();
            this.Params.ForEach(a =>
            {
                _ = arg.Append(",");
                _ = arg.Append(a.ToString());
            });
            if ( arg.Length > 0 )
            {
                _ = arg.Remove(0, 1);
            }
            _ = str.AppendFormat("\t\tpublic {0} {1}({2})\r\n", this.Return.ToString(), this.ApiName, arg.ToString());
            _ = str.AppendLine("\t\t{");
            _ = str.Append("\t\t\t");
            if ( this.Return.Type != ConstDic._VoidType )
            {
                _ = str.Append("return ");
            }
            _ = str.AppendFormat("new {0}", this.Source.Name);
            if ( this.Params.Length == 0 )
            {
                _ = str.Append("()");
            }
            else
            {
                _ = str.Append("\r\n\t\t\t");
                _ = str.AppendLine("{");
                this.Params.ForEach(a =>
                {
                    a.InitParam(str);
                });
                _ = str.Remove(str.Length - 1, 1);
                _ = str.Append("\t\t\t}");
            }
            _ = this.IsPaging ? str.AppendLine(".Send(out count);") : str.AppendLine(".Send();");
            _ = str.AppendLine("\t\t}");
        }
    }
}
