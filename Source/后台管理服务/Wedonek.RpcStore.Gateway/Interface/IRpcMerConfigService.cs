using System;

using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IRpcMerConfigService
        {
                Guid Add(AddMerConfig add);
                void Drop(Guid id);
                RpcMerConfig GetConfig(Guid id);
                RpcMerConfigDatum[] GetConfigs(long rpcMerId);
                void Set(Guid id, SetMerConfig param);
        }
}