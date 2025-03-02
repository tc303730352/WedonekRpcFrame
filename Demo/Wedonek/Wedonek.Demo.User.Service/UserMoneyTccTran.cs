using Wedonek.Demo.RemoteModel.User;
using Wedonek.Demo.User.Service.Interface;
using WeDonekRpc.Client.Interface;

namespace Wedonek.Demo.User.Service
{
    /// <summary>
    /// 用户金额TCC事务
    /// </summary>
    internal class UserMoneyTccTran : IRpcTccEvent
    {
        private readonly IUserService _Servie;
        public UserMoneyTccTran (IUserService service)
        {
            this._Servie = service;
        }

        public void Commit (ICurTran tran)
        {
            System.Console.WriteLine("用户金额提交了! ");
            LockUserMoney data = tran.Body.GetBody<LockUserMoney>();
            this._Servie.SubmitNum(data.UserId, data.Money);
        }

        public void Rollback (ICurTran tran)
        {
            System.Console.WriteLine("用户金额回滚了! ");
            LockUserMoney data = tran.Body.GetBody<LockUserMoney>();
            this._Servie.RollbackNum(data.UserId, data.Money);
        }
    }
}
