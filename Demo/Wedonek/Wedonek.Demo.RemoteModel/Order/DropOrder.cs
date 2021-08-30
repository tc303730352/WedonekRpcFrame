using RpcModel;
using System;

namespace Wedonek.Demo.RemoteModel.Order
{
        [IRemoteConfig("demo.order")]
        public class DropOrder : RpcClient.RpcRemote
        {
                public Guid OrderId
                {
                        get;
                        set;
                }
        }
}
