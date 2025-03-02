using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    /// <summary>
    /// Api权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class ApiPrower : Attribute
    {
        /// <summary>
        /// 是否验证
        /// </summary>
        public bool IsAccredit { get; }
        /// <summary>
        /// 验证权限
        /// </summary>
        public string[] Prower
        {
            get;
        }
        public ApiPrower(params string[] prower)
        {
            this.IsAccredit = true;
            this.Prower = prower;
        }
        public ApiPrower(bool isAccredit)
        {
            this.IsAccredit = isAccredit;
        }
    }
}
