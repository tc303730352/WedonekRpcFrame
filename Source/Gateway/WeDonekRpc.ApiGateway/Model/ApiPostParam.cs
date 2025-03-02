using System;

namespace WeDonekRpc.ApiGateway.Model
{
        public class ApiPostParam
        {
                /// <summary>
                /// 参数名
                /// </summary>
                public string Name
                {
                        get;
                        set;
                }
                /// <summary>
                /// POST的数据类型
                /// </summary>
                public Type PostType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 数据接收方式
                /// </summary>
                public string ReceiveMethod
                {
                        get;
                        set;
                }
                /// <summary>
                /// 参数索引
                /// </summary>
                public int ParamIndex { get; set; }
        }
}
