using System;

namespace WeDonekRpc.WebSocketGateway.Model
{
        internal class FuncParam
        {
                /// <summary>
                /// 属性名称
                /// </summary>
                public string Name
                {
                        get;
                        set;
                }
                /// <summary>
                /// 符合命名的属性名（首字母大写）
                /// </summary>
                public string AttrName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 参数类型
                /// </summary>
                public FuncParamType ParamType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否为基础类型(datetime,int,long,Uri等)
                /// </summary>
                public bool IsBasicType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 数据类型
                /// </summary>
                public Type DataType
                {
                        get;
                        set;
                }
                public string IocName
                {
                        get;
                        set;
                }
        }
}
