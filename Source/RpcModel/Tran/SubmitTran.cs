using System;

namespace RpcModel.Tran
{
        [IRemoteConfig("SubmitTran", "sys.sync", true, true)]
        public class SubmitTran
        {
                public Guid TranId
                {
                        get;
                        set;
                }
        }
}
