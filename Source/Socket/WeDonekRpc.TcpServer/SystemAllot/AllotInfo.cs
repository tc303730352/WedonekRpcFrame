using WeDonekRpc.Helper;
using WeDonekRpc.TcpServer.Model;

namespace WeDonekRpc.TcpServer.SystemAllot
{
    internal class AllotInfo : Interface.IAllot
    {
        public override object Action ()
        {
            switch (this.Type)
            {
                case "UserLogin":
                    LoginDataInfo login = this.GetData<LoginDataInfo>();
                    if (!login.LoginKey.IsNull())
                    {
                        if (TcpServer.CheckPublicKey(this.ClientInfo.ServerId, login.LoginKey))
                        {
                            if (this.ClientInfo.Event != null)
                            {
                                if (!this.ClientInfo.Event.ClientBuildConnect(this.ClientInfo.ClientId, this.ClientIp, login.Arg, out string bindParam, out string error))
                                {
                                    this.Close(3);
                                    return "0," + error;
                                }
                                else
                                {
                                    this.ComplateAuthorization(bindParam);
                                }
                            }
                            else
                            {
                                this.ComplateAuthorization(null);
                            }
                            return "1";
                        }
                        else
                        {
                            this.Close(3);
                            return "0,请检查客户端版本!";
                        }
                    }
                    else
                    {
                        this.Close(3);
                        return "0,请检查客户端版本!";
                    }
                case "CheckIsOk":
                    return "ok";
                case "ErrorPage":
                    break;
                case "Heartbeat":
                    break;
                default:
                    return base.Action();
            }
            return null;
        }
    }
}
