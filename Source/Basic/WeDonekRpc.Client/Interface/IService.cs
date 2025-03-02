
using System.Collections.Generic;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    internal interface IService : IRpcService
    {
        void RemoteStateChange (IRemoteRootNode remote, UsableState oldState, UsableState state);
        void InitEvent ();
        void ReceiveMsgEvent (IMsg msg);

        void SendEvent (ref SendBody send, int sendNum);

        void SendEnd (ref SendBody send, IRemoteResult result);
        void ServiceClosing ();
        Dictionary<string, string> LoadExtendEvent (string dictate);
        void BeginInit ();
        void StartUp ();
        void StartUpIng ();
        void ReceiveEndEvent (RemoteMsg msg, TcpRemoteReply reply);

        /// <summary>
        /// 无服务事件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sysType"></param>
        void NoServerErrorEvent (IRemoteConfig config, string sysType, object model);
    }
}
