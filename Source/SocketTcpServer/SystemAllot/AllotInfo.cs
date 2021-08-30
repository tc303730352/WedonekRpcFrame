using SocketTcpServer.Model;

namespace SocketTcpServer.SystemAllot
{
        internal class AllotInfo : Interface.IAllot
        {
                public override object Action()
                {
                        switch (this.Type)
                        {
                                case "UserLogin":
                                        LoginDataInfo objLoginData = this.GetData<LoginDataInfo>();
                                        if (objLoginData != null)
                                        {
                                                if (SocketTcpServer.CheckPublicKey(this.ServerId, objLoginData.LoginKey))
                                                {
                                                        if (this.ClientInfo.Event != null)
                                                        {
                                                                if (!this.ClientInfo.Event.ClientBuildConnect(this.ClientId, this.ClientIp, objLoginData.Arg, out string bindParam, out string error))
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
