
using RpcModular.Model;

namespace Wedonek.Gateway.WebSocket
{
        internal class UserLoginState : UserState
        {
                /// <summary>
                /// 用户Id
                /// </summary>
                public long UserId
                {
                        get => this.GetValue<long>("UserId");
                        set => this.SetValue("UserId", value);
                }

        }
}
