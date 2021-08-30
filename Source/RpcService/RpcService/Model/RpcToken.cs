using System;

using RpcHelper;

namespace RpcService.Model
{
        [Serializable]
        internal class RpcToken
        {
                public string TokenId
                {
                        get;
                        set;
                }
                public long OAuthMerId
                {
                        get;
                        set;
                }
                public string AppId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 有效截止时间
                /// </summary>
                public int EffectiveTime
                {
                        get;
                        set;
                }
                /// <summary>
                /// 应用版本号
                /// </summary>
                public int AppVerNum { get; set; }

                /// <summary>
                /// 当前连接的服务器ID
                /// </summary>
                public long ConServerId { get; set; }

                internal void SetConServerId(long serverId)
                {
                        this.ConServerId = serverId;
                        this.Save();
                }
                public void Save()
                {
                        string key = string.Concat("Token_", this.TokenId);
                        RpcService.Cache.Set(key, this, HeartbeatTimeHelper.GetTime(this.EffectiveTime - 5));
                }
                public static bool Load(string tokenId, out RpcToken token)
                {
                        string key = string.Concat("Token_", tokenId);
                        return RpcService.Cache.TryGet(key, out token);
                }
        }
}
