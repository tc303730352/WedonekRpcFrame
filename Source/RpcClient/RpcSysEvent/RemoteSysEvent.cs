using System;
using System.Collections.Generic;

using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

namespace RpcClient.RpcSysEvent
{
        internal class RemoteSysEvent
        {
                private static readonly Dictionary<string, IRemoteEvent> _EventRoute = new Dictionary<string, IRemoteEvent>();
                public static void AddEvent<T>(string key, Func<T, TcpRemoteReply> action)
                {
                        _EventRoute.Add(key, new RemoteEvent<T>(action));
                }
                public static void RemoveEvent(string key)
                {
                        _EventRoute.Remove(key);
                }
                public static void AddEvent(string key, Func<TcpRemoteReply> action)
                {
                        _EventRoute.Add(key, new RemoteEvent(action));
                }
                public static bool MsgEvent(RemoteMsg msg, out TcpRemoteReply result)
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
