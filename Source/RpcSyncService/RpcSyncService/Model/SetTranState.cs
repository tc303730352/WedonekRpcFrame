using System;

using RpcModel;

namespace RpcSyncService.Model
{
        public class SetTranState
        {
                /// <summary>
                /// 事务ID
                /// </summary>
                public Guid Id
                {
                        get;
                        set;
                }
                public TransactionStatus TranStatus
                {
                        get;
                        set;
                }
        }
}
