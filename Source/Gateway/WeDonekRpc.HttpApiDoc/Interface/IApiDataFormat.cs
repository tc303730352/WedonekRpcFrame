using WeDonekRpc.HttpApiDoc.Model;

namespace WeDonekRpc.HttpApiDoc.Interface
{
    internal interface IApiDataFormat
    {
        ApiDataType DataType { get; set; }
        string DefValue { get; set; }
        ElementClass[] ElementType { get; set; }
        string ParamName { get; set; }
        string ParamShow { get; set; }
        string ParamType { get; set; }
    }
}