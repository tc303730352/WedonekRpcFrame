using System.Threading.Tasks;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Gateway.WebSocket.Interface;
using WeDonekRpc.WebSocketGateway;

namespace Wedonek.Gateway.WebSocket.Controller
{
    /// <summary>
    /// WebSocket用户接口
    /// </summary>
    internal class UserController : WebSocketController
    {
        private readonly IUserService _User;

        public UserController (IUserService user)
        {
            this._User = user;
        }

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <returns>用户资料</returns>
        public Task<UserDatum> Get ()
        {
            return this._User.GetUser(this.UserState);
        }
        /// <summary>
        /// 发送文本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int SendMsg (string text)
        {
            return this._User.SendMsg(text);
        }
    }
}
