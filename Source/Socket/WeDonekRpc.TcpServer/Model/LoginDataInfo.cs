using System.Runtime.CompilerServices;

namespace WeDonekRpc.TcpServer.Model
{
    public class LoginDataInfo
    {
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public LoginDataInfo ()
        {

        }
        public LoginDataInfo (string key, string[] arg)
        {
            this.LoginKey = key;
            this.Arg = arg;
        }
        /// <summary>
        /// 登陆的密钥
        /// </summary>
        public string LoginKey;

        /// <summary>
        /// 参数
        /// </summary>
        public string[] Arg;
    }
}
