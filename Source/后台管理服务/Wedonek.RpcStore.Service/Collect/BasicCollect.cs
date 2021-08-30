namespace Wedonek.RpcStore.Service.Collect
{
        internal class BasicCollect<T> where T : new()
        {
                public BasicCollect()
                {
                        this.BasicDAL = new T();
                }
                protected T BasicDAL
                {
                        get;
                }

                protected Collect GetCollect<Collect>()
                {
                        return RpcClient.RpcClient.Unity.Resolve<Collect>();
                }

        }
}
