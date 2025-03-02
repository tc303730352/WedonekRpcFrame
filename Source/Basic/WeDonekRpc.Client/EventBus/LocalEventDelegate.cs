using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.Client.EventBus
{
    public delegate void LocalEvent<T>(T eventData, string eventName);
    [Attr.IgnoreIoc]
    internal class LocalEventDelegate<T> : IEventHandler<T>
    {
        private LocalEvent<T> _Action;
        public LocalEventDelegate(LocalEvent<T> action)
        {
            _Action = action;
        }

        public void HandleEvent(T eventData, string eventName)
        {
            _Action(eventData, eventName);
        }
    }
}
