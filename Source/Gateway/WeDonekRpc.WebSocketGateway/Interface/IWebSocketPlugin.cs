using System;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    public enum ExecStage
    {
        全部=14,
        认证=2,
        请求=4,
        执行=8
    }
    public interface IWebSocketPlugin : System.IDisposable,IEquatable<IWebSocketPlugin>
    {
        ExecStage ExecStage { get; }
        bool IsEnable { get; }
        string Name { get; }
        bool Exec(IApiService service, ApiHandler handler, out string error);
        bool RequestInit(IApiService service,out string error);
        bool Authorize(RequestBody request);
        void Init();
    }
}