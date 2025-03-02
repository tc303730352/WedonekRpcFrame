using System.Collections.Generic;
using WeDonekRpc.Helper;
using WeDonekRpc.ModularModel.Identity;
using WeDonekRpc.ModularModel.Identity.Model;

namespace WeDonekRpc.Modular.Model
{
    /// <summary>
    /// 用户标识
    /// </summary>
    public class UserIdentity : DataSyncClass
    {
        internal UserIdentity (string identityId)
        {
            this.IdentityId = identityId;
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
        protected override void SyncData ()
        {
            IdentityDatum datum = new GetIdentity
            {
                IdentityId = this.IdentityId
            }.Send();
            this.IsValid = datum.IsValid;
            this.AppName = datum.AppName;
            this.AppExtend = datum.AppExtend;
        }
    }
}
