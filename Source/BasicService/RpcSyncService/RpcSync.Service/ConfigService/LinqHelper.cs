using RpcSync.Collect.Model;
using RpcSync.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Service.ConfigService
{
    internal static class LinqHelper
    {
        public static ConfigItem[] FindItems (this SysConfigItem config, MsgSource source, RpcServerType serverType)
        {
            ConfigItem[] items = config.Items.FindAll(a => a.CheckIsMatching(source, serverType));
            return items.OrderByDescending(a => a.Weight).Distinct().OrderBy(a => a.Name).ToArray();
        }

        public static SysConfigData[] Filters (this SysConfigItem config, MsgSource source, RpcServerType serverType)
        {
            ConfigItem[] configs = config.Items.FindAll(a => a.CheckIsMatching(source, serverType));
            if (configs.Length == 1)
            {
                ConfigItem con = configs[0];
                return new SysConfigData[]
                {
                    new SysConfigData
                    {
                        Name = con.Name,
                        IsJson = con.ValueType == SysConfigValueType.JSON,
                        Value = con.Value,
                        Prower = con.Prower
                    }
                };
            }
            else if (configs.Length > 0)
            {
                return configs.OrderByDescending(a => a.Weight).Distinct().Select(a => new SysConfigData
                {
                    Name = a.Name,
                    IsJson = a.ValueType == SysConfigValueType.JSON,
                    Value = a.Value,
                    Prower = a.Prower
                }).OrderBy(a => a.Name).ToArray();
            }
            return null;
        }
        public static bool CheckIsMatching (this ConfigItem config, string name, MsgSource source, RpcServerType serverType)
        {
            if (name != config.Name)
            {
                return false;
            }
            return config.CheckIsMatching(source, serverType);
        }
        public static bool CheckIsMatching (this ConfigItem item, MsgSource source, RpcServerType serverType)
        {
            if (item.IsPublic)
            {
                return item.ServiceType == RpcServerType.未知 || item.ServiceType == serverType;
            }
            else if (item.ServerId != 0)
            {
                return item.ServerId == source.ServerId;
            }
            else if (item.ServiceType != RpcServerType.未知 && item.ServiceType != serverType)
            {
                return false;
            }
            else if (item.RegionId != 0 && item.RegionId != source.RegionId)
            {
                return false;
            }
            else if (item.ApiVer != 0 && item.ApiVer != source.VerNum)
            {
                return false;
            }
            else if (item.ContainerGroup != 0 && ( !source.ContGroup.HasValue || item.ContainerGroup != source.ContGroup.Value ))
            {
                return false;
            }
            else if (item.RpcMerId != 0 && item.RpcMerId != source.RpcMerId)
            {
                return false;
            }
            return true;
        }
    }
}
