using System;

using RpcModel.Tran.Model;

namespace RpcModel.Tran
{
        [IRemoteConfig("AddTranLog", "sys.sync", true, true)]
        public class AddTranLog
        {
                public Guid TranId
                {
                        get;
                        set;
                }
                public TranLogDatum TranLog
                {
                        get;
                        set;
                }
                public Guid? MainTranId { get; set; }
        }
}
