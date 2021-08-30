namespace RpcClient.Interface
{
        internal interface ISampler
        {
                bool Sample(out long spanId);
        }
}