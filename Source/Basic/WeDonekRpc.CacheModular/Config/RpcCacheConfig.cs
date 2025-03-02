using System.Collections.Generic;
using System.Net;
using WeDonekRpc.CacheClient.Config;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheModular;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Model;

namespace WeDonekRpc.CacheModular.Config
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RpcCacheConfig : IRpcCacheConfig
    {
        private readonly IConfigSection _Config;
        public RpcCacheConfig ( ISysConfig config )
        {
            this._Config = config.GetSection("rpc:cache");
        }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType => this._Config.GetValue("CacheType", CacheType.Redis);
        public CacheConfig Cache => this._GetConfig();


        private string[] _FormatIp ( string[] ip, string defPort )
        {
            if ( ip.IsNull() )
            {
                return ip;
            }
            List<string> ips = [];
            ip.ForEach(a =>
            {
                int index = a.IndexOf(':');
                string port;
                string ip;
                if ( index == -1 )
                {
                    ip = a;
                    port = defPort;
                }
                else
                {
                    ip = a.Substring(0, index);
                    port = a.Substring(index + 1);
                }
                if ( !ip.Validate(Helper.Validate.ValidateFormat.IP) )
                {
                    IPAddress[] address = Dns.GetHostAddresses(ip);
                    if ( !address.IsNull() )
                    {
                        address.ForEach(c =>
                        {
                            ips.Add(string.Concat(c.ToString(), ":", port));
                        });
                    }
                    return;
                }
                ips.Add(string.Concat(ip, ":", port));
            });
            return ips.ToArray();
        }
        private CacheConfig _GetConfig ()
        {
            MsgSource source = RpcClient.CurrentSource;
            CacheConfig config = new CacheConfig
            {
                SysKey = string.Join(":", source.RpcMerId, source.SystemTypeId),
                Memcached = this._Config.GetValue<MemcachedConfig>("Memcached"),
                Redis = this._Config.GetValue<RedisConfig>("Redis"),
            };
            if ( config.Memcached != null )
            {
                config.Memcached.ServerIp = this._FormatIp(config.Memcached.ServerIp, "11211");
            }

            return config;
        }
    }
}
