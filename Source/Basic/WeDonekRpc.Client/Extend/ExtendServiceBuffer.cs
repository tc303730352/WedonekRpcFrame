using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;

namespace WeDonekRpc.Client.Extend
{
    public class ExtendServiceBuffer : IDisposable
    {
        private readonly IocBuffer _Ioc;
        private readonly List<string> _RpcExtends = new List<string>(10);
        internal ExtendServiceBuffer (IocBuffer ioc)
        {
            this._Ioc = ioc;
        }
        /// <summary>
        /// 加载扩展服务
        /// </summary>
        /// <param name="types"></param>
        internal void Reg (Type type)
        {
            if (type.GetInterface(ConfigDic.IExtendService.FullName) != null)
            {
                IocBody ioc = this._Ioc.Register(ConfigDic.IExtendService, type);
                if (ioc == null)
                {
                    ioc = this._Ioc.Find(ConfigDic.IExtendService, type);
                    if (ioc == null)
                    {
                        return;
                    }
                }
                if (!this._RpcExtends.Contains(ioc.Name))
                {
                    this._RpcExtends.Add(ioc.Name);
                }
            }
        }
        internal void Init ()
        {
            if (this._RpcExtends.Count > 0)
            {
                IRpcService rpc = RpcService.Service;
                IIocService ioc = RpcClient.Ioc;
                this._RpcExtends.ForEach(c =>
                {
                    if (ioc.TryResolve(c, out IRpcExtendService service))
                    {
                        service.Load(rpc);
                    }
                });
            }
        }

        public void Dispose ()
        {
            this._RpcExtends.Clear();
        }
    }
}
