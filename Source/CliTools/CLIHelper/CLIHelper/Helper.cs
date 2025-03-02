using System.Reflection;
using System.Text;
using GatewayBuildCli.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace GatewayBuildCli
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    internal class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="str"></param>
        public static void WriteShow ( ApiBody body, StringBuilder str )
        {
            _ = str.AppendFormat("\t\t/// <summary>\r\n\t\t/// {0}\r\n\t\t /// </summary>\r\n", body.Show);
            if ( body.Params.Length > 0 )
            {
                body.Params.ForEach(c =>
                {
                    _ = str.AppendFormat("\t\t /// <param name=\"{0}\">{1}</param>\r\n", c.Name, c.Show);
                });
            }
            if ( body.Return.Type != PublicDataDic.VoidType )
            {
                _ = str.AppendFormat("\t\t/// <returns>{0}</returns>\r\n", body.Return.Show);
            }
        }
        public static void WriteApiShow ( ApiBody body, int mode, StringBuilder str )
        {
            _ = str.AppendFormat("\t\t/// <summary>\r\n\t\t/// {0}\r\n\t\t /// </summary>\r\n", body.Show);
            if ( mode == 0 )
            {
                _ = str.AppendLine("\t\t /// <param name=\"param\">参数</param>");
            }
            else if ( mode == 1 )
            {
                body.Params.ForEach(c =>
                {
                    if ( c.ParamType == "basic" )
                    {
                        _ = str.AppendFormat("\t\t /// <param name=\"{0}\">{1}</param>\r\n", c.Name, c.Show);
                    }
                });
            }
            if ( body.Return.Type != PublicDataDic.VoidType )
            {
                _ = str.AppendFormat("\t\t/// <returns>{0}</returns>\r\n", body.Return.Show);
            }
        }
        public static void WriteUiShow ( ApiBody body, StringBuilder str )
        {
            _ = str.AppendFormat("\t/// <summary>\r\n\t/// {0} UI参数实体\r\n\t/// </summary>\r\n", body.Show);
        }
        public static void WriteShow ( ApiParam param, StringBuilder str )
        {
            _ = str.AppendFormat("\t\t/// <summary>\r\n\t\t/// {0}\r\n\t\t /// </summary>\r\n", param.Show);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nSpace"></param>
        /// <param name="name"></param>
        /// <param name="defName"></param>
        /// <returns></returns>
        public static string FormatNamespace ( string nSpace, string name, string defName )
        {
            if ( name == nSpace )
            {
                return defName;
            }
            if ( nSpace.StartsWith(name) )
            {
                return nSpace.Remove(0, name.Length + 1);
            }
            else if ( nSpace.IndexOf('.') == -1 )
            {
                return nSpace;
            }
            string[] t = nSpace.Split('.');
            return t[^1];
        }
        public static string GetRootName ( string name )
        {
            string str = name.ToLower();
            if ( str.EndsWith(".remotemodel") )
            {
                name = name.Remove(str.Length - 12);
            }
            if ( str.StartsWith("rpc") )
            {
                name = name.Remove(0, 3);
            }
            char[] chars = name.ToArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
        public static string GetNamespace ( string name )
        {
            string str = name.ToLower();
            if ( str.EndsWith(".remotemodel") )
            {
                name = name.Remove(str.Length - 12);
            }
            if ( str.StartsWith("rpc") )
            {
                name = name.Remove(0, 3);
            }
            name = string.Concat(name, ".Gatewary.Modular");
            char[] chars = name.ToArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
        public static string GetApiName ( Type type )
        {
            IRemoteConfig config = type.GetCustomAttribute<IRemoteConfig>();
            return config.SysDictate ?? type.Name;
        }
        public static bool CheckProperty ( PropertyInfo pro )
        {
            if ( pro.GetCustomAttribute(ConstDic._AppIdAttr) != null )
            {
                return false;
            }
            return pro.CanWrite && pro.CanRead;
        }

        internal static ReturnTypeBody GetReturnType ( Type type, out bool isPaging )
        {
            Type parent = type.BaseType;
            if ( parent == ConstDic._RpcRemoteType || parent == ConstDic._RpcBroadcastType )
            {
                isPaging = false;
                return new ReturnTypeBody(ConstDic._VoidType);
            }
            else if ( parent == ConstDic._UpFileType )
            {
                isPaging = false;
                return new ReturnTypeBody(ConstDic._UpFileType);
            }
            string name = parent.Name;
            if ( name.StartsWith("RpcRemoteArray`1") )
            {
                isPaging = false;
                return new ReturnTypeBody(parent.GenericTypeArguments[0], true);
            }
            if ( name.StartsWith("BasicPage`1") )
            {
                isPaging = true;
                return new ReturnTypeBody(parent.GenericTypeArguments[0], true);
            }
            else
            {
                isPaging = false;
                return new ReturnTypeBody(parent.GenericTypeArguments[0], false);
            }
        }
        public static string FormatTypeName ( Type type )
        {
            switch ( type.Name )
            {
                case "Int32":
                    return "int";
                case "Int16":
                    return "short";
                case "Int64":
                    return "long";
                case "String":
                    return "string";
                case "Object":
                    return "object";
                case "Boolean":
                    return "bool";
            }
            if ( type.IsGenericType )
            {
                StringBuilder str = new StringBuilder(type.Name.Substring(0, type.Name.IndexOf('`')));
                _ = str.Append("<");
                type.GenericTypeArguments.ForEach(c =>
                {
                    _ = str.Append(FormatTypeName(c));
                    _ = str.Append(",");
                });
                _ = str.Remove(str.Length - 1, 1);
                _ = str.Append(">");
                return str.ToString();
            }
            return type.Name;
        }

        public static void CopDir ( DirectoryInfo source, DirectoryInfo destFolder )
        {
            foreach ( FileInfo file in source.GetFiles() )
            {
                string path = Path.Combine(destFolder.FullName, file.Name);
                if ( System.IO.File.Exists(path) )
                {
                    return;
                }
                _ = file.CopyTo(path);
            }
            foreach ( DirectoryInfo dir in source.GetDirectories() )
            {
                string path = Path.Combine(destFolder.FullName, dir.Name);
                DirectoryInfo dest = new DirectoryInfo(path);
                if ( !dest.Exists )
                {
                    dest.Create();
                }
                CopDir(dir, dest);
            }
        }
    }
}
