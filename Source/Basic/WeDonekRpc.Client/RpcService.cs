
using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client
{
    internal class RpcService : IService
    {
        public static IService Service = new RpcService();
        public event ReceiveMsgEvent ReceiveMsg;



        public event LoadExtend LoadExtend;
        public event Action StartUpComplate;
        public event Action StartUpComplating;

        public event SendIng SendIng;
        public event SendEnd SendComplate;
        public event ReceiveEndEvent ReceiveEnd;
        public event Action InitComplating;

        public event Action BeginIniting;
        public event Action Closing;

        public event ServerNodeStateChange RemoteState;
        public event NoServerEvent NoServerEvent;

        public void RemoteStateChange (IRemoteRootNode remote, UsableState oldState, UsableState state)
        {
            if (this.RemoteState != null)
            {
                this.RemoteState(remote, oldState, state);
            }
        }
        public void NoServerErrorEvent (IRemoteConfig config, string sysType, object model)
        {
            if (this.NoServerEvent != null)
            {
                this.NoServerEvent(config, sysType, model);
            }
        }
        public void SendEvent (ref SendBody send, int sendNum)
        {
            if (this.SendIng != null)
            {
                this.SendIng(ref send, sendNum);
            }
        }

        public Dictionary<string, string> LoadExtendEvent (string dictate)
        {
            if (LoadExtend == null)
            {
                return null;
            }
            Dictionary<string, string> extend = [];
            LoadExtend(dictate, ref extend);
            if (extend.Count == 0)
            {
                return null;
            }
            return extend;
        }

        public void StartUp ()
        {
            if (this.StartUpComplate != null)
            {
                this.StartUpComplate();
            }
        }
        public void InitEvent ()
        {
            if (this.InitComplating != null)
            {
                this.InitComplating();
            }
        }
        public void BeginInit ()
        {
            if (this.BeginIniting != null)
            {
                this.BeginIniting();
            }
        }
        public void StartUpIng ()
        {
            if (this.StartUpComplating != null)
            {
                this.StartUpComplating();
            }
        }
        public void ReceiveMsgEvent (IMsg msg)
        {
            if (this.ReceiveMsg != null)
            {
                this.ReceiveMsg(msg);
            }
        }

        public void ServiceClosing ()
        {
            if (this.Closing != null)
            {
                this.Closing();
            }
        }

        public void ReceiveEndEvent (RemoteMsg msg, TcpRemoteReply reply)
        {
            if (this.ReceiveEnd != null)
            {
                this.ReceiveEnd(msg, reply);
            }
        }

        public void SendEnd (ref SendBody send, IRemoteResult result)
        {
            if (this.SendComplate != null)
            {
                this.SendComplate(ref send, result);
            }
        }
    }
}
