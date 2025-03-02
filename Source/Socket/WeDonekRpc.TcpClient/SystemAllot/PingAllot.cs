namespace WeDonekRpc.TcpClient.SystemAllot
{
        internal class PingAllot : Interface.IAllot
        {
                internal override object Action(ref string type)
                {
                        return "ok";
                }
        }
}
