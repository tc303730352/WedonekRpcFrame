
using System;
using System.Collections.Generic;

using RpcClient.Interface;
using RpcClient.Model;

using RpcHelper;

namespace RpcClient
{
        internal class RpcService : IService
        {
                public static IService Service = new RpcService();
                public event ReceiveMsgEvent ReceiveMsg;

                public event SendMsgEvent SendMsg;
                public event Action StartUpComplate;

                public Dictionary<string, string> LoadExtend(string dictate)
                {
                        if (SendMsg == null)
                        {
                                return null;
                        }
                        Dictionary<string, string> extend = new Dictionary<string, string>();
                        SendMsg(dictate, ref extend);
                        if (extend.Count == 0)
                        {
                                return null;
                        }
                        return extend;
                }
                public void StartUp()
                {
                        if (StartUpComplate != null)
                        {
                                StartUpComplate();
                        }
                }

                public bool ReceiveMsgEvent(IMsg msg, out string error)
                {
                        if (ReceiveMsg == null)
                        {
                                error = null;
                                return true;
                        }
                        try
                        {
                                ReceiveMsg(msg);
                                error = null;
                                return true;
                        }
                        catch (Exception e)
                        {
                                ErrorException ex = ErrorException.FormatError(e);
                                if (ex.IsSystemError)
                                {
                                        ex.SaveLog("Rpc");
                                }
                                error = ex.ToString();
                                return false;
                        }
                }
        }
}
