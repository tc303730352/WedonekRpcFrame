using WeDonekRpc.TcpClient.Model;

namespace WeDonekRpc.TcpClient.SystemAllot
{
        internal class SysAllot : Interface.IAllot
        {
                internal override object Action(ref string type)
                {
                        switch (type)
                        {
                                case "Welcome":
                                        type = "UserLogin";
                                        ServerConConfig config = Config.SocketConfig.GetConArg(this.ClientInfo.ServerId);
                                        return new LoginDataInfo(config.ConKey, config.Arg);
                                case "UserLogin":
                                        string res = this.GetData();
                                        if (res == "1")
                                        {
                                                this.ComplateAuthorization();
                                        }
                                        else if (!string.IsNullOrEmpty(res))
                                        {
                                                if (Config.SocketConfig.SocketEvent != null)
                                                {
                                                        string[] temp = res.Split(',');
                                                        Config.SocketConfig.SocketEvent.ServerConnectError(temp[1]);
                                                }
                                                this.Close();
                                        }
                                        break;
                                case "ErrorPage":
                                        string error = this.GetData();
                                        if (!string.IsNullOrEmpty(error))
                                        {
                                                Manage.PageManage.SendError(this.PageId, error);
                                        }
                                        break;
                                case "Heartbeat":
                                        break;
                                default:
                                        return base.Action(ref type);
                        }
                        return null;
                }
        }
}
