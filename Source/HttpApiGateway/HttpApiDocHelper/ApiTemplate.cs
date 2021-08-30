using HttpApiDocHelper.Interface;
using HttpApiDocHelper.Model;

using RpcHelper;
namespace HttpApiGateway
{
        internal class ApiTemplate : IApiTemplate
        {
                public ApiAttrShow ErrorMsg { get; set; } = new ApiAttrShow
                {
                        AttrName = "errmsg",
                        AttrShow = "错误信息",
                        AttrType = PublicDataDic.StrType
                };
                public ApiAttrShow ErrorCode { get; set; } = new ApiAttrShow
                {
                        AttrName = "errorcode",
                        AttrShow = "错误码(0 为成功 其它代表发生错误)",
                        AttrType = typeof(int)
                };
                public ApiAttrShow Data { get; set; } = new ApiAttrShow
                {
                        AttrName = "data",
                        AttrShow = "返回的数据"
                };
                public ApiAttrShow Count { get; set; } = new ApiAttrShow
                {
                        AttrName = "count",
                        AttrShow = "数据总量",
                        AttrType = typeof(long)
                };
                public ApiAttrShow PagingData { get; set; } = new ApiAttrShow
                {
                        AttrName = "list",
                        AttrShow = "数据列表"
                };
        }
}
