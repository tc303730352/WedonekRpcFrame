using System;
using System.Data;

using SqlExecHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class RpcMerConfigDAL : SqlBasicClass
        {
                public RpcMerConfigDAL() : base("RpcMerConfig")
                {

                }
                public bool CheckIsExists(long rpcMerId, long sysTypeId, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new ISqlWhere[]{
                                new SqlWhere("RpcMerId", SqlDbType.BigInt){Value=rpcMerId},
                                new SqlWhere("SystemTypeId", SqlDbType.BigInt) { Value = sysTypeId }
                        });
                }
                public bool GetConfigs(long rpcMerId, out RpcMerConfig[] configs)
                {
                        return this.Get("RpcMerId", rpcMerId, out configs);
                }
                public bool GetConfig(Guid id, out RpcMerConfig config)
                {
                        return this.GetRow("Id", id, out config);
                }

                public bool Add(RpcMerConfig add)
                {
                        return this.Insert(add);
                }

                public bool Set(Guid id, SetMerConfig config)
                {
                        return this.Update(config, "Id", id);
                }
                public bool Drop(Guid id)
                {
                        return this.Drop("Id", id) > 0;
                }
        }
}
