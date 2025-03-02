using System.Text;
using RpcSync.Collect.Model;
using RpcSync.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Collect.linq
{
    internal static class LinqHelper
    {
        public static void InitConfig (this ConfigItem config)
        {
            if (config.ServerId != 0)
            {
                config.Weight = int.MaxValue;
            }
            else
            {
                int weight = 0;
                if (config.RegionId != 0)
                {
                    weight += 10;
                }
                if (config.ServiceType != RpcServerType.未知)
                {
                    weight += 1000;
                }
                if (config.ContainerGroup != 0)
                {
                    weight += 100;
                }
                if (config.ApiVer != 0)
                {
                    weight += 10000;
                }
                if (config.RpcMerId != 0)
                {
                    weight += 1;
                }
                config.Weight = weight;
            }
            config.IsPublic = config.Weight == 0;
            if (config.ValueType == SysConfigValueType.Null)
            {
                config.Value = null;
            }
        }
        public static string GetConfigMd5 (this SysConfig[] configs)
        {
            if (configs.IsNull())
            {
                return string.Empty;
            }
            long maxTime = configs.Max(c => c.ToUpdateTime).ToLong();
            StringBuilder str = new StringBuilder(( configs.Length * 30 ) + 10);
            configs.ForEach(a =>
            {
                _ = str.Append(a.Name);
                _ = str.Append(",");
            });
            _ = str.Append(maxTime);
            return str.ToString().GetMd5();
        }
        public static string ToMsgBody (this TcpRemoteMsg msg)
        {
            StringBuilder body = new StringBuilder("{", 300);
            _ = body.Append("\"MsgBody\":");
            _ = body.Append(msg.MsgBody);
            if (msg.Tran != null)
            {
                _ = body.Append(",\"Tran\":");
                _ = body.Append(msg.Tran.ToJson());
            }
            if (msg.Track != null)
            {
                _ = body.Append(",\"Track\":");
                _ = body.Append(msg.Track.ToJson());
            }
            if (msg.Stream != null)
            {
                _ = body.Append(",\"StreamLen\":");
                _ = body.Append(msg.Stream.Length.ToString());
            }
            if (msg.Extend != null && msg.Extend.Count > 0)
            {
                _ = body.Append(",\"Extend\":");
                _ = body.Append(msg.Extend.ToJson());
            }
            _ = body.Append("}");
            return body.ToString();
        }
    }
}
