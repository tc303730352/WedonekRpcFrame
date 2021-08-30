using System;

namespace RpcModel.Tran
{
        [IRemoteConfig("EndTran", "sys.sync", false, true)]
        public class EndTran
        {
                public Guid TranId
                {
                        get;
                        set;
                }
        }
}
