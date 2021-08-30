using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using RpcClient;

using RpcModular;

using Wedonek.Demo.RemoteModel.Subscribe;
using Wedonek.Demo.RemoteModel.User;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;

namespace Wedonek.Gateway.Modular.Service
{
        internal class UserService : IUserService
        {
                private IAccreditService _Accredit = null;

                public UserService(IAccreditService accredit)
                {
                        _Accredit = accredit;
                }
                public long Reg(UserRegParam reg)
                {
                        return new AddUser
                        {
                                UserName = reg.UserName,
                                UserPhone = reg.UserPhone
                        }.Send();
                }
                public UserDatum GetUser(UserLoginState state)
                {
                        return new GetUser
                        {
                                UserId = state.UserId
                        }.Send();
                }
                public void KickOutUser(long userId)
                {
                        string applyId = string.Concat("Demo_", userId);//唯一码用于单点
                        _Accredit.KickOutAccredit(applyId);
                }
                public IUserState GetAccredit(string accreditId)
                {
                        return _Accredit.GetAccredit(accreditId);
                }
                public string UserLogin(string phone)
                {
                        long userId = new UserLogin
                        {
                                Phone = phone
                        }.Send();
                        //用户状态值
                        UserLoginState state = new UserLoginState
                        {
                                UserId = userId,
                                Prower = new string[]
                              {
                                      "demo.order"
                              }
                        };
                        string applyId = string.Concat("Demo_", userId);//唯一码用于单点
                        string accreditId = _Accredit.AddAccredit(applyId, new string[] {
                                "demo.order",
                                "demo.user"
                        }, state);
                        //发送用户上线广播
                        new UserGoOnline
                        {
                                UserId = userId,
                                Phone = phone
                        }.Send();
                        return accreditId;
                }

                public string PressureTest()
                {
                        int errorNum = 0;
                        Stopwatch watch = new Stopwatch();
                        watch.Start();
                        for (int a = 0; a < 100; a++)
                        {
                                if (new PressureTest
                                {
                                        Num = a
                                }.Send() != a)
                                {
                                        ++errorNum;
                                }
                        }
                        watch.Stop();
                        return string.Concat(watch.ElapsedMilliseconds + " _ " + errorNum);
                }
        }
}
