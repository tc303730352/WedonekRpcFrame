using System;

using RpcModel;

using RpcModularModel.Resource;

using RpcHelper;

namespace RpcSyncService.Collect
{
        internal class ResourceModularCollect : BasicCollect<DAL.ResourceModularDAL>
        {
                public static Guid GetModular(string name, ResourceType type, MsgSource source)
                {
                        string key = string.Join("_", name, (int)type, source.SystemTypeId, source.RpcMerId).GetMd5();
                        Guid id = _FindModularId(key);
                        if (id == Guid.Empty)
                        {
                                return _AddModular(name, type, source, key);
                        }
                        return id;
                }
                private static Guid _FindModularId(string key)
                {
                        string cache = string.Concat("RM_", key);
                        if (RpcClient.RpcClient.Cache.TryGet(cache, out Guid id))
                        {
                                return id;
                        }
                        id = BasicDAL.FindModular(key);
                        if (id != Guid.Empty)
                        {
                                RpcClient.RpcClient.Cache.Add(cache, id, new TimeSpan(10, 0, 0, 0));
                        }
                        return id;
                }
                private static Guid _AddModular(string name, ResourceType type, MsgSource source, string key)
                {
                        Guid id = BasicDAL.AddModular(new Model.ResourceModular
                        {
                                ModularKey = key,
                                ModularName = name,
                                ResourceType = type,
                                SysGroupId = source.SourceGroupId,
                                SystemTypeId = source.SystemTypeId,
                                RpcMerId = source.RpcMerId
                        });
                        string cache = string.Concat("RM_", key);
                        RpcClient.RpcClient.Cache.Add(cache, id, new TimeSpan(10, 0, 0, 0));
                        return id;
                }
        }
}
