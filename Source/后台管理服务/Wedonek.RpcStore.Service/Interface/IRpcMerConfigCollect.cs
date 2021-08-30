using System;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IRpcMerConfigCollect
        {
                Guid Add(AddMerConfig add);
                void Drop(Guid id);
                RpcMerConfig GetConfig(Guid id);
                RpcMerConfig[] GetConfigs(long rpcMerId);
                void Set(Guid id, SetMerConfig param);
        }
}