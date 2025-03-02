using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.ExtendModel.RetryTask;
using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Reflection;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RetryService
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class AutoRetryService : IAutoRetryService
    {
        private readonly IRemoteSendService _Service;
        private readonly IAutoRetryConfig _Config;
        public AutoRetryService (IRemoteSendService service, IAutoRetryConfig config)
        {
            this._Config = config;
            this._Service = service;
        }
        public RetryResult GetRetryResult<T> (string identityId) where T : class
        {
            Type type = typeof(T);
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                throw new ErrorException("rpc.class.config.error");
            }
            return this._Service.Send<GetRetryResult, RetryResult>(new GetRetryResult
            {
                IdentityId = this._GetIdentityId(config, identityId)
            });
        }
        private string _GetIdentityId (IRemoteConfig config, string id)
        {
            return config.SysDictate + "_" + id;
        }
        public void Cancel<T> (string identityId) where T : class
        {
            Type type = typeof(T);
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                throw new ErrorException("rpc.class.config.error");
            }
            this._Service.Send(new CancelTask
            {
                IdentityId = this._GetIdentityId(config, identityId)
            });
        }
        public void Restart (long serverId, string identityId)
        {
            this._Service.Send(serverId, new RestartTask
            {
                IdentityId = identityId
            });
        }
        public void Cancel (long serverId, string identityId)
        {
            this._Service.Send(serverId, new CancelTask
            {
                IdentityId = identityId
            });
        }
        public void AddTask (RetryTaskAdd add)
        {
            this._Service.Send(new AddRetryTask
            {
                Task = add
            });
        }
        public void AddTask<T> (T model, string identityId, RetryConfig retry, string show) where T : class
        {
            Type type = model.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                throw new ErrorException("rpc.class.config.error");
            }
            if (retry == null)
            {
                retry = this._Config.GetRetrySet(config.SysDictate);
            }
            RetryTaskAdd add = new RetryTaskAdd
            {
                Show = show,
                IdentityId = this._GetIdentityId(config, identityId),
                RetryConfig = retry,
                SendBody = new RpcParamConfig
                {
                    SysDictate = config.SysDictate,
                    MsgBody = this._ToMsgBody(model, type),
                    SystemType = config.SystemType,
                    RpcMerId = config.RpcMerId,
                    RegionId = config.RegionId,
                    RemoteSet = new RpcRemoteSet()
                    {
                        AppIdentity = config.AppIdentity,
                        IdentityColumn = config.IdentityColumn,
                        IsEnableLock = config.IsEnableLock,
                        LockColumn = config.LockColumn,
                        IsProhibitTrace = config.IsProhibitTrace,
                        LockType = config.LockType,
                        Transmit = config.Transmit,
                        TransmitType = config.TransmitType,
                        ZIndexBit = config.ZIndexBit
                    }
                }
            };
            this.AddTask(add);
        }
        public void AddTask<T> (T model, string identityId, string show) where T : class
        {
            this.AddTask(model, identityId, null, show);
        }
        private Dictionary<string, object> _ToMsgBody<T> (T model, Type type) where T : class
        {
            IReflectionBody body = ReflectionHepler.GetReflection(type);
            Dictionary<string, object> data = [];
            body.Properties.ForEach(c =>
            {
                object val = c.GetValue(model);
                if (val != null)
                {
                    data.Add(c.Name, val);
                }
            });
            return data;
        }
    }
}
