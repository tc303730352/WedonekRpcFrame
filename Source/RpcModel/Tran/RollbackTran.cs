using System;

namespace RpcModel.Tran
{
        [IRemoteConfig("RollbackTran", "sys.sync", false, true)]
        public class RollbackTran
        {
                public Guid TranId
                {
                        get;
                        set;
                }
        }
}
