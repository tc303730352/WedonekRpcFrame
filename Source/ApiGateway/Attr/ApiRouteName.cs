using System;

using ApiGateway.Helper;
using ApiGateway.Interface;

namespace ApiGateway.Attr
{
        /// <summary>
        /// 自定义名称或路径
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
        public class ApiRouteName : Attribute
        {
                public ApiRouteName(string name)
                {
                        this._IsPath = name.IndexOf('/') != -1;
                        this.Value = name;
                }
                /// <summary>
                /// 是否为路径
                /// </summary>
                private readonly bool _IsPath = false;
                /// <summary>
                /// 值
                /// </summary>
                public string Value
                {
                        get;
                }

                public string FormatPath(IApiConfig config, IModular modular)
                {
                        if (this._IsPath)
                        {
                                return ApiHelper.GetApiPath(config, modular, this.Value);
                        }
                        return ApiHelper.GetApiPath(config, this.Value, modular);
                }
        }
}
