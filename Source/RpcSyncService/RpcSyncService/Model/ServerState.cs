using System;

using SqlExecHelper;

namespace RpcSyncService.Model
{
        internal class ServerState
        {
                [SqlColumnType("Id", SqlFuncType.count)]
                public long Count
                {
                        get;
                        set;
                }
                [SqlColumnType("AddTime", SqlFuncType.max)]
                public DateTime MaxTime
                {
                        get;
                        set;
                }
        }
}
