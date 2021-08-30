using HttpApiDocHelper.Interface;

namespace HttpApiDocHelper.Model
{
        internal class ApiDataFormat : IApiDataFormat
        {
                public string ParamName
                {
                        get;
                        set;
                }
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

                public string ParamShow
                {
                        get;
                        set;
                }
                public ElementClass[] ElementType { get; set; }

        }
}
