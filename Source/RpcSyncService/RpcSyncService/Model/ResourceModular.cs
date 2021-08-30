using System;

using RpcModularModel.Resource;

namespace RpcSyncService.Model
{
        internal class ResourceModular
        {
                public Guid Id
                {
                        get;
                        set;
                }
                public string ModularKey
                {
                        get;
                        set;
                }
                public long RpcMerId
                {
                        get;
                        set;
                }
                public long SysGroupId
                {
                        get;
                        set;
                }
                public long SystemTypeId
                {
                        get;
                        set;
                }
                public string ModularName
                {
                        get;
                        set;
                }
                public ResourceType ResourceType
                {
                        get;
                        set;
                }
        }
}
