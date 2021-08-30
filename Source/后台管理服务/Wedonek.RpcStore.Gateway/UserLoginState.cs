using RpcModular.Model;

namespace Wedonek.RpcStore.Gateway
{
        internal class UserLoginState : UserState
        {
                public string LoginName
                {
                        get => base.GetValue<string>("LoginName");
                        set => this["LoginName"] = value;
                }
        }
}
