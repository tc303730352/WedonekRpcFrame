using System;
using WeDonekRpc.Model.Server;

namespace WeDonekRpc.Client.Model
{
    /// <summary>
    /// 授权Token
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 授权Token
        /// </summary>
        /// <param name="obj">授权码</param>
        public AccessToken(ref AccessTokenRes obj)
        {
            this.Access_Token = obj.access_token;
            this.RpcMerId = obj.RpcMerId;
            this.Effective = DateTime.Now.AddSeconds(obj.expires_in);
        }
        /// <summary>
        /// 服务集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 授权Token
        /// </summary>
        public string Access_Token
        {
            get;
            set;
        }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime Effective
        {
            get;
            set;
        }
    }
}
