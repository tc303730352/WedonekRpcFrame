using HttpApiDocHelper.Interface;

namespace HttpApiDocHelper.Model
{
        internal class ClassData
        {
                public string ClassName
                {
                        get;
                        set;
                }
                public string Show
                {
                        get;
                        set;
                }
                public IApiDataFormat[] ProList
                {
                        get;
                        set;
                }
        }
}
