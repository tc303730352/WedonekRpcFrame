using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Demo.User.Service.Interface;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
namespace Wedonek.Demo.User.Service.Service
{
    internal class UserService : IUserService
    {
        private readonly IRpcTranService _Tran;
        private readonly IRedisStringController _Redis;
        public UserService ( IRpcTranService tran, IRedisStringController redis )
        {
            this._Tran = tran;
            this._Redis = redis;
        }
        public UserDatum GetUser ( long userId )
        {
            string key = string.Concat("User_", userId);
            if ( this._Redis.TryGet(key, out UserDatum user) )
            {
                return user;
            }
            throw new ErrorException("demo.user.not.find");
        }
        private long _FindUserId ( string phone )
        {
            string key = string.Concat("UPhone_", phone);
            if ( this._Redis.TryGet(key, out long userId) )
            {
                return userId;
            }
            return 0;
        }
        private long _GetNewUserId ()
        {
            return this._Redis.Increment("UserId");
        }
        public long UserLogin ( string phone )
        {
            long userId = this._FindUserId(phone);
            if ( userId == 0 )
            {
                throw new ErrorException("demo.user.no.reg");
            }
            return userId;
        }
        public void SetOrderNo ( long userId, string orderNo, string old )
        {
            string key = string.Concat("User_", userId);
            if ( this._Redis.TryGet(key, out UserDatum user) )
            {
                if ( user.OrderNo == orderNo )
                {
                    user.OrderNo = old;
                    _ = this._Redis.Set(key, user);
                }
                return;
            }
            throw new ErrorException("demo.user.not.find");
        }
        public void SetOrderNo ( long userId, string orderNo )
        {
            string key = string.Concat("User_", userId);
            if ( this._Redis.TryGet(key, out UserDatum user) )
            {
                //设置当前事务状态
                this._Tran.SetTranExtend(user.OrderNo);
                user.OrderNo = orderNo;
                _ = this._Redis.Set(key, user);
                return;
            }
            throw new ErrorException("demo.user.not.find");
        }
        public void TryLockNum ( long userId, int num )
        {
            string key = string.Concat("User_", userId);
            if ( this._Redis.TryGet(key, out UserDatum user) )
            {
                user.SurplusNum -= num;
                user.LockNum += num;
                _ = this._Redis.Set(key, user);
                return;
            }
            throw new ErrorException("demo.user.not.find");
        }
        public void SubmitNum ( long userId, int num )
        {
            string key = string.Concat("User_", userId);
            if ( this._Redis.TryGet(key, out UserDatum user) )
            {
                user.LockNum -= num;
                user.SubmitNum += num;
                _ = this._Redis.Set(key, user);
                return;
            }
            throw new ErrorException("demo.user.not.find");
        }
        public void RollbackNum ( long userId, int num )
        {
            string key = string.Concat("User_", userId);
            if ( this._Redis.TryGet(key, out UserDatum user) )
            {
                user.SurplusNum += num;
                user.LockNum -= num;
                _ = this._Redis.Set(key, user);
                return;
            }
            throw new ErrorException("demo.user.not.find");
        }
        public long AddUser ( string name, string phone )
        {
            long userId = this._FindUserId(phone);
            UserDatum add;
            if ( userId != 0 )
            {
                add = new UserDatum
                {
                    UserId = userId,
                    UserName = name,
                    UserPhone = phone
                };
            }
            else
            {
                add = new UserDatum
                {
                    UserId = this._GetNewUserId(),
                    UserName = name,
                    UserPhone = phone
                };
            }
            string key = string.Concat("User_", add.UserId);
            if ( !this._Redis.Add(key, add) )
            {
                throw new ErrorException("demo.user.reg.error");
            }
            key = string.Concat("UPhone_", phone);
            _ = this._Redis.Set(key, add.UserId);
            return add.UserId;
        }
    }
}
