using System;

using RpcModel;

namespace RpcClient.Model
{
        public class AccessToken
        {
                public AccessToken(AccessTokenRes obj)
                {
                        this.Access_Token = obj.access_token;
                        this.RpcMerId = obj.RpcMerId;
                        this.Effective = DateTime.Now.AddSeconds(obj.expires_in);
                }
                public long RpcMerId
                {
                        get;
                        set;
                }
                public string Access_Token
                {
                        get;
                        set;
                }

                public DateTime Effective
                {
                        get;
                        set;
                }
        }
}
