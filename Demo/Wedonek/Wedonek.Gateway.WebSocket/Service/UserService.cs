using System.Threading.Tasks;
using Wedonek.Demo.RemoteModel.User;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Gateway.WebSocket.Interface;
using WeDonekRpc.Modular;
using WeDonekRpc.WebSocketGateway.Interface;

namespace Wedonek.Gateway.WebSocket.Service
{
    internal class UserService : IUserService
    {
        /// <summary>
        /// 获取当前会话
        /// </summary>
        private readonly ICurrentSession _Session;
        /// <summary>
        /// 当前服务模块信息
        /// </summary>
        private ICurrentModular _Modular;
        /// <summary>
        /// 当前请求
        /// </summary>
        private readonly ICurrentService _Request;

        public UserService (ICurrentSession session, ICurrentModular modular, ICurrentService request)
        {
            this._Request = request;
            this._Modular = modular;
            this._Session = session;
        }
        public Task<UserDatum> GetUser (IUserState state)
        {
            return new GetUser
            {
                UserId = state.GetValue<long>("UserId")
            }.AsyncSend();
        }
        /// <summary>
        /// 广播一个上线消息
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int SendMsg (string text)
        {
            if (!this._Modular.IsHasValue)//检查是否获取到了当前模块(跨线程操作会丢失模块信息需处理)
            {
                this._Modular = this._Modular.Init("WebSocketDemo");//未获取到-初始化模块
            }
            IBatchSend send = this._Modular.BatchSend(a => a.IsAccredit && a.IsOnline);
            return send.Send("OnlineMsg", text);
        }
    }
}
