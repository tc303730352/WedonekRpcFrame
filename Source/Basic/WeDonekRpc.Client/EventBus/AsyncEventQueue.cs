using System;
using System.Collections.Generic;

namespace WeDonekRpc.Client.EventBus
{
    internal class AsyncEventQueue
    {
        private readonly List<Delegate> _events = new List<Delegate>();

        public void Add (Delegate[] adds)
        {
            this._events.AddRange(adds);
        }

    }
}
