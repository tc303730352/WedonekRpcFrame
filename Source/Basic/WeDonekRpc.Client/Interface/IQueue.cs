using System;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    internal interface IQueue : IDisposable
    {
        void Subscrib ();
        void BindRoute (string routeKey);
        bool Public (QueueRemoteMsg msg, string[] routeKey, string exchange);
    }
}