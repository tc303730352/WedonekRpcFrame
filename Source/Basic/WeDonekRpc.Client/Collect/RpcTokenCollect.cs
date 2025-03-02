using WeDonekRpc.Client.Server;

namespace WeDonekRpc.Client.Collect
{
    internal class RpcTokenCollect
    {
        private static readonly RpcToken _Token = new RpcToken();
        internal static bool GetAccessToken(out RpcToken token, out string error)
        {
            if (!_Token.Init() || !_Token.IsInit)
            {
                token = null;
                error = _Token.Error;
                return false;
            }
            else if (_Token.CheckToken())
            {
                error = null;
                token = _Token;
                return true;
            }
            else
            {
                token = null;
                error = _Token.Error;
                return false;
            }
        }
    }
}
