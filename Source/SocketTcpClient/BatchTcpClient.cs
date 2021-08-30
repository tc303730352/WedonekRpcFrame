using System;
using System.Net;

using SocketTcpClient.Interface;

using RpcHelper;
namespace SocketTcpClient
{
        public class BatchTcpClient : ITcpClient
        {

                public TcpClient[] _ClientList = null;

                private readonly TcpClient _MainClient = null;

                private readonly int _Num = 0;

                public int TimeOut => Config.SocketConfig.Timeout;

                public int SyncTimeOut => Config.SocketConfig.SyncTimeOut;

                public BatchTcpClient(IPAddress[] address, int port)
                {
                        this._Num = address.Length;
                        TcpClient[] clients = address.ConvertAll(a => new TcpClient(a.ToString(), port));
                        int index = Tools.GetRandom() % this._Num;
                        this._MainClient = this._ClientList[index];
                        this._ClientList = clients;
                }

                public bool Ping(out TimeSpan time)
                {
                        return this._MainClient.Ping(out time);
                }

                public void CloseClient()
                {
                        this._ClientList.ForEach(a => a.CloseClient());
                }

                public void Send(string type, string data, Async async, object arg)
                {
                        this._MainClient.Send(type, data, async, arg);
                }

                public bool Send(string type, string data, out string error)
                {
                        return this._MainClient.Send(type, data, out error);
                }

                public bool Send(string type, string data, out string str, out string error)
                {
                        return this._MainClient.Send(type, data, out str, out error);
                }

                public bool Send<Result>(string type, string data, out Result result, out string error)
                {
                        return this._MainClient.Send(type, data, out result, out error);
                }

                public bool Send<T, Result>(string type, T data, out Result result, out string error)
                        where T : class
                        where Result : class
                {
                        return this._MainClient.Send(type, data, out result, out error);
                }

                public void Send<T>(string type, T data, Async async, object arg) where T : class
                {
                        this._MainClient.Send(type, data, async, arg);
                }

                public bool Send<T>(string type, T data, out string error) where T : class
                {
                        return this._MainClient.Send(type, data, out error);
                }

                public bool Send<T>(string type, T data, out string str, out string error) where T : class
                {
                        return this._MainClient.Send(type, data, out str, out error);
                }

                public bool Send(string type, out string error)
                {
                        return this._MainClient.Send(type, out error);
                }

                public bool Send(string type, out string result, out string error)
                {
                        return this._MainClient.Send(type, out result, out error);
                }

                public bool Send<Result>(string type, out Result result, out string error)
                {
                        return this._MainClient.Send(type, out result, out error);
                }

                public int GetClientConNum()
                {
                        return this._MainClient.GetClientConNum();
                }

                public bool CheckIsUsable(out string error)
                {
                        return this._MainClient.CheckIsUsable(out error);
                }
        }
}
