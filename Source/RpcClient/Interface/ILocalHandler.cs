namespace RpcClient.Interface
{
        internal interface ILocalHandler
        {
                void HandleEvent(object data,string name);
        }
}