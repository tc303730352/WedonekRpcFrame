using WeDonekRpc.HttpApiDoc.Interface;

namespace WeDonekRpc.HttpApiDoc.Model
{
        internal class ReturnBody
        {
                public ApiReturnType ReturnType
                {
                        get;
                        set;
                } = ApiReturnType.无;

                public string Show { get; set; }

                public IApiDataFormat[] Pros
                {
                        get;
                        set;
                }
        }
}
