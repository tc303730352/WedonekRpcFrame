
using System.Collections.Generic;

using RpcClient.Model;

namespace RpcClient.Interface
{
        internal interface IService : IRpcService
        {
                bool ReceiveMsgEvent(IMsg msg, out string error);
                Dictionary<string, string> LoadExtend(string dictate);

                void StartUp();
        }
}
