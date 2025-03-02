namespace GatewayBuildCli.Model
{
    internal class ReturnTypeBody
    {
        public ReturnTypeBody ( Type type, bool isArray = false )
        {
            this.Type = type;
            this.IsArray = isArray;
            if ( !isArray && type.Name == "Nullable`1" )
            {
                this.IsNull = true;
                this.Type = type.GetGenericArguments()[0];
            }
            if ( Type.GetTypeCode(type) == TypeCode.Object )
            {
                this.Show = XmlShowHelper.FindParamShow(this.Type);
            }
        }
        public string Show
        {
            get;
            set;
        }
        public Type Type
        {
            get;
            set;
        }
        public bool IsArray
        {
            get;
            set;
        }
        public bool IsNull
        {
            get;
            set;
        }

        public override string ToString ()
        {
            if ( this.IsArray )
            {
                return string.Concat(this.Type.Name, "[]");
            }
            else if ( this.Type.Name == "Void" )
            {
                return "void";
            }
            else if ( this.IsNull )
            {
                return Helper.FormatTypeName(this.Type) + "?";
            }
            return Helper.FormatTypeName(this.Type);
        }
    }
}
