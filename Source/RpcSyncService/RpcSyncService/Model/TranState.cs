using System;

using RpcModel;

namespace RpcSyncService.Model
{
        /// <summary>
        /// 事务状态
        /// </summary>
        public class TranState
        {
                /// <summary>
                /// 事务ID
                /// </summary>
                public Guid Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 主事务Id
                /// </summary>
                public Guid MainTranId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 父级事务Id
                /// </summary>
                public Guid ParentId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 事务状态
                /// </summary>
                public TransactionStatus TranStatus
                {
                        get;
                        set;
                }
                public bool IsEnd
                {
                        get;
                        set;
                }


        }
}
