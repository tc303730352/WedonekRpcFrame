using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RpcSysEvent
{
    internal class RemoteSysEvent
    {
        private static readonly Dictionary<string, IRemoteEvent> _EventRoute = [];
        public static void AddEvent<T> (string key, Func<T, MsgSource, TcpRemoteReply> action)
        {
            _EventRoute.Add(key, new RemoteEvent<T>(action));
        }
        public static void RemoveEvent (string key)
        {
            _ = _EventRoute.Remove(key);
        }
        public static void AddEvent (string key, Func<MsgSource, TcpRemoteReply> action)
        {
            _EventRoute.Add(key, new RemoteEvent(action));
        }
        public static bool MsgEvent (RemoteMsg msg, out TcpRemoteReply result)
        {
            if (_EventRoute.TryGetValue(msg.MsgKey, out IRemoteEvent val))
            {
                result = val.MsgEvent(msg);
                return true;
            }
            result = null;
            return false;
        }
    }
}
