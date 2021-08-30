namespace RpcClient.Interface
{
        public interface IEventHandler<T>
        {
                void HandleEvent(T eventData,string eventName);
        }
}
