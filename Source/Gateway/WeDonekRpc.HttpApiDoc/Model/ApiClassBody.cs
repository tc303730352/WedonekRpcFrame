namespace WeDonekRpc.HttpApiDoc.Model
{
    internal class ApiClassBody
    {
        public string DefValue
        {
            get;
            set;
        }
        public ApiDataType DataType
        {
            get;
            set;
        }
        public string ParamType
        {
            get;
            set;
        }

        public ElementClass[] ElementType { get; set; }
    }
}
