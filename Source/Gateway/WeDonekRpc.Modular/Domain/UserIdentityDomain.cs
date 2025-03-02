using System.Collections.Generic;
using WeDonekRpc.Modular.Model;

namespace WeDonekRpc.Modular.Domain
{
    internal class UserIdentityDomain
    {
        private readonly UserIdentity _Identity;

        internal UserIdentityDomain (UserIdentity identity)
        {
            this._Identity = identity;
        }
        /// <summary>
        /// 标识ID
        /// </summary>
        public string IdentityId
        {
            get;
            private set;
        }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid
        {
            get;
            private set;

        }
        /// <summary>
        /// 应用扩展
        /// </summary>
        public Dictionary<string, string> AppExtend
        {
            get;
            private set;
        }
    }
}
