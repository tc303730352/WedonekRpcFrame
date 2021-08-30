using RpcClient.Interface;

using RpcModular;

using Wedonek.Demo.RemoteModel.User;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Demo.User.Service.Interface;

namespace Wedonek.Demo.User.Service.Event
{
        /// <summary>
        /// 用户事件
        /// </summary>
        internal  class UserEvent : RpcClient.Interface.IRpcApiService
        {
                private readonly IUserService _User = null;

                public UserEvent(IUserService user)
                {
                        this._User = user;
                }
                public int PressureTest(PressureTest test)
                {
                        return test.Num;
                }
                /// <summary>
                /// 设置订单号
                /// </summary>
                /// <param name="set"></param>
                public void SetOrderNo(SetOrderNo set, IUserIdentity identity)
                {
                        _User.SetOrderNo(set.UserId, set.OrderNo);
                }
                /// <summary>
                /// 添加用户下单数
                /// </summary>
                /// <param name="add"></param>
                public void AddOrderNum(AddOrderNum add)
                {
                        this._User.AddOrderNum(add.UserId, add.OrderNum);
                }
                /// <summary>
                /// 新建用户
                /// </summary>
                /// <param name="add"></param>
                /// <returns></returns>
                public long AddUser(AddUser add)
                {
                        return this._User.AddUser(add.UserName, add.UserPhone);
                }
                /// <summary>
                /// 获取用户资料
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public UserDatum GetUser(GetUser obj)
                {
                        return this._User.GetUser(obj.UserId);
                }
                /// <summary>
                /// 用户登陆
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public long UserLogin(UserLogin obj)
                {
                        return this._User.UserLogin(obj.Phone);
                }
        }
}
