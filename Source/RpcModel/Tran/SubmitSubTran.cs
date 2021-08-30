using System;

namespace RpcModel.Tran
{
        [IRemoteConfig("SubmitSubTran", "sys.sync", false, true)]
        public class SubmitSubTran
        {
                public Guid TranId
                {
                        get;
                        set;
                }
        }
}
