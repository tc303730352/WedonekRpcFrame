using System;

namespace RpcSyncService.Model
{
        internal class OverTimeTran
        {
                /// <summary>
                /// 事务ID
                /// </summary>
                public Guid Id
                {
                        get;
                        set;
                }
                public Guid MainTranId
                {
                        get;
                        set;
                }
        }
}
