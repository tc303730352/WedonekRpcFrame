using System;
using System.Data;

using RpcSyncService.Model.DAL_Model;

using SqlExecHelper;

namespace RpcSyncService.DAL
{
        internal class SysConfigDAL : SqlBasicClass
        {
                public SysConfigDAL() : base("SysConfig")
                {

                }
                public bool GetSysConfig(long typeId, out SysConfigModel[] configs)
                {
                        return this.Get(out configs, new InSqlWhere<long>("SystemTypeId", SqlDbType.BigInt, new long[] {
                                0,
                                typeId
                        }));
                }

                public bool GetToUpdateTime(long typeId, out DateTime toUpdateTime)
                {
                        return this.ExecuteScalar("ToUpdateTime", SqlFuncType.max, out toUpdateTime, new InSqlWhere<long>("SystemTypeId", SqlDbType.BigInt, new long[] { 0, typeId }));
                }
        }
}
