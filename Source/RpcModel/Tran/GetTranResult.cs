using System;

namespace RpcModel.Tran
{
        /// <summary>
        /// 获取事务结果
        /// </summary>
        [IRemoteConfig("GetTranResult", "sys.sync", true, true)]
        public class GetTranResult
        {
                public Guid TranId
                {
                        get;
                        set;
                }
        }
}
