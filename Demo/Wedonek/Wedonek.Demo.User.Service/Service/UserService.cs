using RpcCacheClient.Interface;
using RpcHelper;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Demo.User.Service.Interface;
namespace Wedonek.Demo.User.Service.Service
{
        internal class UserService : IUserService
        {
                public UserDatum GetUser(long userId)
                {
                        string key = string.Concat("User_", userId);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out UserDatum user))
                        {
                                return user;
                        }
                        throw new ErrorException("demo.user.not.find");
                }
                private long _FindUserId(string phone)
                {
                        string key = string.Concat("UPhone_", phone);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out long userId))
                        {
                                return userId;
                        }
                        return 0;
                }
                private long _GetNewUserId()
                {
                        IRedisCacheController redis = RpcCacheClient.RpcCacheService.GetRedis();
                        return redis.Increment("UserId", 1);
                }
                public long UserLogin(string phone)
                {
                        long userId = this._FindUserId(phone);
                        if (userId == 0)
                        {
                                throw new ErrorException("demo.user.no.reg");
                        }
                        return userId;
                }
                public void SetOrderNo(long userId, string orderNo,string old)
                {
                        string key = string.Concat("User_", userId);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out UserDatum user))
                        {
                                if (user.OrderNo == orderNo)
                                {
                                        user.OrderNo = old;
                                        RpcClient.RpcClient.Cache.Set(key, user);
                                }
                                return;
                        }
                        throw new ErrorException("demo.user.not.find");
                }
                public void SetOrderNo(long userId, string orderNo)
                {
                        string key = string.Concat("User_", userId);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out UserDatum user))
                        {
                                //设置当前事务状态
                                RpcClient.RpcClient.RpcTran.SetTranExtend(user.OrderNo);
                                user.OrderNo = orderNo;
                                RpcClient.RpcClient.Cache.Set(key, user);
                                return;
                        }
                        throw new ErrorException("demo.user.not.find");
                }
                public void AddOrderNum(long userId,int num)
                {
                        string key = string.Concat("User_", userId);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out UserDatum user))
                        {
                                user.OrderNum += num;
                                RpcClient.RpcClient.Cache.Set(key, user);
                                return;
                        }
                        throw new ErrorException("demo.user.not.find");
                }
                public long AddUser(string name, string phone)
                {
                        long userId = this._FindUserId(phone);
                        UserDatum add = null;
                        if (userId != 0)
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
                        if (!RpcClient.RpcClient.Cache.Add(key, add))
                        {
                                throw new ErrorException("demo.user.reg.error");
                        }
                        key = string.Concat("UPhone_", phone);
                        RpcClient.RpcClient.Cache.Set(key, add.UserId);
                        return add.UserId;
                }
        }
}
