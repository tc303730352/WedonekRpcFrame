using System;

namespace WeDonekRpc.ApiGateway.Model
{
        public class ResultBody
        {
                public Type ResultType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 返回的参数名
                /// </summary>
                public string ParamName { get; set; }
                public string AttrName { get; set; }
        }
}
