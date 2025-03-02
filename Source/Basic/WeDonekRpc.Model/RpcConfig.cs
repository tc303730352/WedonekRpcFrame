using WeDonekRpc.Helper;

namespace WeDonekRpc.Model
{
    public class RpcConfig
    {
        public string[] AllowConIp
        {
            get;
            set;
        }
        public bool CheckAccreditIp (string clientIp)
        {
            if (this.AllowConIp.Length == 1)
            {
                if (this.AllowConIp[0] == "*")
                {
                    return true;
                }
                return this.AllowConIp[0] == clientIp;
            }
            return this.AllowConIp.FindIndex(a => a == clientIp) != -1;
        }
    }
}
