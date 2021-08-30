using System;

namespace RpcModel.Tran
{
        [IRemoteConfig("ForceRollbackTran", "sys.sync", false, true)]
       public  class ForceRollbackTran
        {
                public Guid TranId
                {
                        get;
                        set;
                }
        }
}
