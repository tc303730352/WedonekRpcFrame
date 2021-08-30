using System;

using RpcClient;

namespace Wedonek.Demo.Service.LocalEvent.Event
{
        public class AddOrderEvent : RpcLocalEvent
        {
                public long UserId
                {
                        get;
                        set;
                }
                public string SerialNo
                {
                        get;
                        set;
                }
        }
}
