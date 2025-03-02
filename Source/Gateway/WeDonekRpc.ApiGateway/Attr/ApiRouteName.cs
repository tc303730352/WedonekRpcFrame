using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    /// <summary>
    /// 自定义名称或路径
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class ApiRouteName : Attribute
    {
        public ApiRouteName(string name)
        {
            this.IsPath = name.IndexOf('/')!=-1;
            this.Value = name;
        }
        public ApiRouteName(string name, bool isRegex)
        {
            this.IsPath = name.IndexOf('/') != -1;
            this._IsRegex = isRegex;
            this.Value = name;
        }
        /// <summary>
        /// 是否为路径
        /// </summary>
        public bool IsPath
        {
            get;
            private set;
        }

        private readonly bool _IsRegex = false;
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get;
        }
        public bool IsRegex => this._IsRegex;

    }
}
