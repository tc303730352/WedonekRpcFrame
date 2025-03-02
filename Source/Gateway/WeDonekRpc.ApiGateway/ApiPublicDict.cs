using System;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Modular;

namespace WeDonekRpc.ApiGateway
{
    public class ApiPublicDict
    {
        public static readonly string ErrorParamName = "error";
        public static readonly string CountParamName = "count";
        public static readonly string ParamNullError = "public.param.null";
        public static readonly string SuccessJosn = "{\"errorcode\":0}";
        public static readonly Type ApiMethodIgnore = typeof(ApiMethodIgnore);
        public static readonly Type IUserStateType = typeof(IUserState);
        public static readonly Type ApiRouteName = typeof(ApiRouteName);
        public static readonly Type ApiEventAttrName = typeof(ApiEventAttr);
        public static readonly Type ApiUpConfig = typeof(ApiUpConfig);
        public static readonly Type ApiStop = typeof(ApiStop);
        public static readonly Type ProwerAttr = typeof(ApiPrower);

        public static Type IUserIdentity = typeof(IUserIdentity);
    }
}
