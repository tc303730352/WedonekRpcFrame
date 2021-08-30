using HttpApiDocHelper.Model;

namespace HttpApiDocHelper.Interface
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