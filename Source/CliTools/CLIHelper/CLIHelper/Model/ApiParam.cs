using System.Reflection;
using System.Text;
using WeDonekRpc.Helper;

namespace GatewayBuildCli.Model
{
    internal class ApiParam
    {
        public ApiParam ( PropertyInfo a )
        {
            this.ParamType = "basic";
            this.Show = XmlShowHelper.FindParamShow(a.DeclaringType, a.Name);
            this.ProName = a.Name;
            this.SourceProName = a.Name;
            this.Type = a.PropertyType;
            char[] chars = a.Name.ToArray();
            chars[0] = char.ToLower(chars[0]);
            this.Name = new string(chars);
            if ( a.PropertyType.Name == "Nullable`1" )
            {
                this.IsNull = true;
                this.Type = a.PropertyType.GetGenericArguments()[0];
            }
            if ( this.Show.IsNull() && Type.GetTypeCode(this.Type) == TypeCode.Object )
            {
                this.Show = XmlShowHelper.FindParamShow(this.Type);
            }
        }
        public ApiParam ( string type )
        {
            this.ParamType = type;
            if ( type == "paging" )
            {
                this.Show = "分页参数";
                this.Type = ConstDic._BasicPage;
                this.ProName = ConstDic._BasicPage.Name;
                this.Name = "paging";
            }
            else
            {
                this.Show = "数据总数";
                this.Type = typeof(int);
                this.ProName = "int";
                this.Name = "count";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Show { get; }
        public string ParamType { get; }
        public string Name { get; }
        public string ProName { get; }
        public bool IsNull { get; }
        public Type Type { get; }
        public string SourceProName { get; }

        public void InitParam ( StringBuilder str )
        {
            if ( this.ParamType == "basic" )
            {
                _ = str.Append("\t\t\t\t");
                _ = str.AppendLine(this.ProName + "=" + this.Name + ",");
            }
            else if ( this.ParamType == "paging" )
            {
                _ = str.Append("\t\t\t\t");
                _ = str.AppendLine("Index=" + this.Name + ".Index,");
                _ = str.Append("\t\t\t\t");
                _ = str.AppendLine("Size=" + this.Name + ".Size,");
                _ = str.Append("\t\t\t\t");
                _ = str.AppendLine("NextId=" + this.Name + ".NextId,");
                _ = str.Append("\t\t\t\t");
                _ = str.AppendLine("SortName=" + this.Name + ".SortName,");
                _ = str.Append("\t\t\t\t");
                _ = str.AppendLine("IsDesc=" + this.Name + ".IsDesc");
            }
        }
        public string GetValidate ( string prefix )
        {
            if ( this.IsNull )
            {
                return string.Empty;
            }
            if ( this.Type.IsEnum )
            {
                return string.Format("[EnumValidate(\"{0}.{1}.error\",typeof({2}))]", prefix, this.Name, this.Type.Name);
            }
            if ( this.Type == PublicDataDic.GuidType )
            {
                return string.Format("[NullValidate(\"{0}.{1}.null\")]", prefix, this.Name);
            }
            else if ( this.Type.IsPrimitive )
            {
                TypeCode code = Type.GetTypeCode(this.Type);
                if ( code == TypeCode.String || code == TypeCode.Object || code == TypeCode.DateTime )
                {
                    return string.Format("[NullValidate(\"{0}.{1}.null\")]", prefix, this.Name);
                }
                else if ( code != TypeCode.Char )
                {
                    return string.Format("[NumValidate(\"{0}.{1}.error\",1)]", prefix, this.Name);
                }
                return string.Empty;
            }
            return string.Format("[NullValidate(\"{0}.{1}.null\")]", prefix, this.Name);
        }
        public bool CheckIsWrite ()
        {
            return this.ParamType == "basic";
        }
        public void WriteProperty ( StringBuilder str, string prefix )
        {
            string validate = this.GetValidate(prefix);
            if ( !validate.IsNull() )
            {
                _ = str.AppendFormat("\t\t{0}\r\n", validate);
            }
            _ = str.AppendFormat("\t\tpublic {0} {1} ", this.GetProType(), this.SourceProName);
            _ = str.AppendLine("{get;set;}");
        }
        public string GetProType ()
        {
            if ( this.IsNull )
            {
                return Helper.FormatTypeName(this.Type) + "? ";
            }
            return Helper.FormatTypeName(this.Type);
        }
        public override string ToString ()
        {
            if ( this.ParamType == "count" )
            {
                return string.Concat("out ", this.ProName, " ", this.Name);
            }
            else if ( this.ParamType == "paging" )
            {
                return string.Concat("IBasicPage ", this.Name);
            }
            return string.Concat(this.GetProType(), " ", this.Name);
        }
    }
}
