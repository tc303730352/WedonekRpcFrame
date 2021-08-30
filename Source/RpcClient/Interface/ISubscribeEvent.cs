using RpcClient.Model;

namespace RpcClient.Interface
{
        public interface ISubscribeEvent
        {
                string EventName { get; }
                bool Exec(IMsg msg);
                bool Init();
        }
}
