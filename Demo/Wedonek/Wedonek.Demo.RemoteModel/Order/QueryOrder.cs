namespace Wedonek.Demo.RemoteModel.Order
{
        [RpcModel.IRemoteConfig("demo.order")]
        public class QueryOrder : RpcClient.BasicPage<Model.OrderData>
        {
                public long UserId
                {
                        get;
                        set;
                }
        }
}
