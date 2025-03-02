using System;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    public interface IRpcSubscribeCollect : System.IDisposable
    {
        bool CheckIsExists(string name);

        bool Add(string name, Action action);
        bool Add<T>(string name, Action<T> action);
        bool Add<T>(string name, Action<T, MsgSource> action);
        bool Add(string name, Action<MsgSource> action);
    }
}