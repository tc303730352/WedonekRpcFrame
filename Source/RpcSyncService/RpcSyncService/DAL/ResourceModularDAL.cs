using System;

using RpcSyncService.Model;

using SqlExecHelper;

using RpcHelper;

namespace RpcSyncService.DAL
{
        internal class ResourceModularDAL : SqlBasicClass
        {
                public ResourceModularDAL() : base("ResourceModular", "RpcExtendService")
                {

                }

                public Guid AddModular(ResourceModular add)
                {
                        add.Id = Tools.NewGuid();
                        if (!this.Insert(add))
                        {
                                throw new ErrorException("resource.modular.add.error");
                        }
                        return add.Id;
                }

                public Guid FindModular(string key)
                {
                        if (this.ExecuteScalar("Id", out Guid id, new ISqlWhere[] {
                                new SqlWhere("ModularKey", System.Data.SqlDbType.VarChar,32){Value=key}
                        }))
                        {
                                return id;
                        }
                        throw new ErrorException("resource.modular.find.error");
                }

        }
}
