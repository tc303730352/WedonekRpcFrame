using System;

namespace RpcModel.Tran
{
        /// <summary>
        /// 获取事务状态
        /// </summary>
        [IRemoteConfig("GetTranState", "sys.sync", true, true)]
        public class GetTranState
        {
                /// <summary>
                /// 事务Id
                /// </summary>
                public Guid TranId
                {
                        get;
                        set;
                }
        }
}
