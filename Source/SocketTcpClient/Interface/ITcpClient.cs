using System;

using SocketTcpClient.Interface;

namespace SocketTcpClient
{
        public interface ITcpClient
        {
                int TimeOut { get; }
                int SyncTimeOut { get; }
                int GetClientConNum();
                bool Ping(out TimeSpan ping);
                bool CheckIsUsable(out string error);
                void CloseClient();
                bool Send(string type, out string error);

                bool Send(string type, out string result, out string error);

                bool Send<Result>(string type, out Result result, out string error);

                void Send(string type, string data, Async async, object arg);
                bool Send(string type, string data, out string error);
                bool Send(string type, string data, out string str, out string error);
                bool Send<Result>(string type, string data, out Result model, out string error);
                bool Send<T, Result>(string type, T data, out Result result, out string error)
                        where T : class
                        where Result : class;
                void Send<T>(string type, T data, Async async, object arg) where T : class;
                bool Send<T>(string type, T data, out string error) where T : class;
                bool Send<T>(string type, T data, out string str, out string error) where T : class;
        }
}