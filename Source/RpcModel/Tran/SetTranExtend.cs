using System;

namespace RpcModel.Tran
{
        [IRemoteConfig("SetTranExtend", "sys.sync", false, true)]
        public class SetTranExtend
        {
                public Guid TranId
                {
                        get;
                        set;
                }
                public string Extend
                {
                        get;
                        set;
                }
        }
}
