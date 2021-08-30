using Wedonek.RpcStore.Gateway.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IAdminService
        {
                string AdminLogin(AdminLogin login);
                void SetAdminPwd(UserLoginState state, string pwd);
                void RegAdmin(AdminLogin reg, string clientIp);
        }
}