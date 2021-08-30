using RpcHelper;

using RpcModular;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class AdminService : IAdminService
        {
                private readonly ISysConfigCollect _Config = null;
                private readonly IAccreditService _Accredit = null;
                public AdminService(ISysConfigCollect config, IAccreditService accredit)
                {
                        this._Accredit = accredit;
                        this._Config = config;
                }
                public void SetAdminPwd(UserLoginState state, string pwd)
                {
                        string key = string.Concat("RpcAdmin_", state.LoginName);
                        long id = this._Config.FindConfigId(RpcClient.RpcClient.CurrentSource.SystemTypeId, key);
                        this._Config.SetSysConfig(id, new RpcStore.Service.Model.SysConfigSetParam
                        {
                                Value = string.Concat(pwd, "_6xy3#7a%adxyz").GetMd5().ToLower(),
                                ValueType = RpcStore.Service.Model.SysConfigValueType.字符串
                        });
                }
                public string AdminLogin(AdminLogin login)
                {
                        string key = string.Concat("RpcAdmin_", login.LoginName);
                        string pwd = RpcClient.RpcClient.Config.GetConfigVal(key);
                        if (pwd.IsNull())
                        {
                                throw new ErrorException("rpc.admin.pwd.error");
                        }
                        else if (pwd != string.Concat(login.Pwd, "_6xy3#7a%adxyz").GetMd5().ToLower())
                        {
                                throw new ErrorException("rpc.admin.pwd.error");
                        }
                        UserLoginState state = new UserLoginState
                        {
                                LoginName = login.LoginName
                        };
                        this._Accredit.AddAccredit(key, null, state);
                        return state.AccreditId;
                }

                public void RegAdmin(AdminLogin reg, string clientIp)
                {
                        string limitIp = RpcClient.RpcClient.Config.GetConfigVal("limitIp", "127.0.0.1");
                        if (limitIp != clientIp)
                        {
                                throw new ErrorException("rpc.no.prower");
                        }
                        string key = string.Concat("RpcAdmin_", reg.LoginName);
                        this._Config.AddSysConfig(new SysConfigAddParam
                        {
                                Name = key,
                                RpcMerId = 0,
                                SystemTypeId = RpcClient.RpcClient.CurrentSource.SystemTypeId,
                                ValueType = SysConfigValueType.字符串,
                                Value = string.Concat(reg.Pwd, "_6xy3#7a%adxyz").GetMd5().ToLower()
                        });
                }
        }
}
