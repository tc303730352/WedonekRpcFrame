using WeDonekRpc.Client.Model;

namespace WeDonekRpc.Client.Interface
{
        public interface ISubscribeEvent
        {
                string EventName { get; }
                bool Exec(IMsg msg);
                bool Init();
        }
}
