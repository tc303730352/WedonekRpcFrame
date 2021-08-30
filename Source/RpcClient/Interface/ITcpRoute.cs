using RpcClient.Model;

using RpcModel;

namespace RpcClient.Interface
{
        public delegate IBasicRes TcpMsgEvent(IMsg msg);


        internal interface ITcpRoute : IRoute
        {
                bool VerificationRoute();

        }
}
