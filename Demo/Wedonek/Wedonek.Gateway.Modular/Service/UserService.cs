using Wedonek.Demo.RemoteModel.Subscribe;
using Wedonek.Demo.RemoteModel.User;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.Modular;
using WeDonekRpc.Modular.Model;

namespace Wedonek.Gateway.Modular.Service
{
    internal class UserService : IUserService
    {
        private readonly IAccreditService _Accredit = null;

        public UserService ( IAccreditService accredit )
        {
            this._Accredit = accredit;
        }
        public long Reg ( UserRegParam reg )
        {
            return new AddUser
            {
                UserName = reg.UserName,
                UserPhone = reg.UserPhone
            }.Send();
        }
        public UserDatum GetUser ( IUserState state )
        {
            return new GetUser
            {
                UserId = state.GetValue<long>("UserId")
            }.Send();
        }
        public void KickOutUser ( long userId )
        {
            string applyId = string.Concat("Demo_", userId);//唯一码用于单点
            this._Accredit.KickOutAccredit(applyId);
        }
        public IUserState GetAccredit ( string accreditId )
        {
            return this._Accredit.GetAccredit(accreditId);
        }
        public string UserLogin ( string phone )
        {
            long userId = new UserLogin
            {
                Phone = phone
            }.Send();
            //用户状态值
            UserState state = new UserState
            {
                Prower = new string[]
                {
                    "demo.order"
                }
            };
            state["UserId"] = userId;
            string applyId = string.Concat("Demo_", userId);//唯一码用于单点
            string accreditId = this._Accredit.AddAccredit(applyId, state, 7200);
            //发送用户上线广播
            new UserGoOnline
            {
                UserId = userId,
                Phone = phone
            }.Send();
            return accreditId;
        }

    }
}
