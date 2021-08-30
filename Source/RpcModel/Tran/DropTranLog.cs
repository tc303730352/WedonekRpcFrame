using System;

namespace RpcModel.Tran
{
        [IRemoteConfig("DropTranLog", "sys.sync", false, true)]
        public class DropTranLog
        {
                public Guid TranId
                {
                        get;
                        set;
                }
        }
}
