using System;
using System.Collections.Generic;
using System.Reflection;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.EventBus;
using WeDonekRpc.Client.Extend;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Mapper;
using WeDonekRpc.Client.RouteService;
using WeDonekRpc.Client.Subscribe;
using WeDonekRpc.Client.Tran;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Helper.Http;
using WeDonekRpc.Helper.Interface;

namespace WeDonekRpc.Client
{
    /// <summary>
    /// 初始化结束前
    /// </summary>
    /// <param name="option"></param>
    public delegate void RpcInitEnding (IIocService ioc);
    /// <summary>
    /// 加载结束前
    /// </summary>
    /// <param name="option"></param>
    public delegate void RpcLoadEnding (RpcInitOption option);
    public class RpcInitOption : IDisposable
    {
        private readonly List<IRpcInitModular> _Modulars = [];
        private readonly List<string> _HasAssembly = [];
        /// <summary>
        /// IOC容器
        /// </summary>
        private readonly IocBuffer _Buffer;
        /// <summary>
        /// 路由服务
        /// </summary>
        private readonly RouteBuffer _Route;
        /// <summary>
        /// 订阅
        /// </summary>
        private readonly RpcSubscribeBuffer _Subscribe;
        /// <summary>
        /// 本地事件
        /// </summary>
        private readonly LocalEventBuffer _LocalEvent;

        /// <summary>
        /// ORM 框架
        /// </summary>
        private readonly MapperBuffer _Mapper;
        /// <summary>
        /// 扩展服务
        /// </summary>
        private readonly ExtendServiceBuffer _ExtendService;
        /// <summary>
        /// 远程事务
        /// </summary>
        private readonly IRpcTranRegService _Tran;

        public IocBuffer Ioc => this._Buffer;

        public RouteBuffer Route => this._Route;

        public LocalEventBuffer LocalEvent => this._LocalEvent;

        public MapperBuffer Mapper => this._Mapper;
        /// <summary>
        /// 事务
        /// </summary>
        public IRpcTranRegService Tran => this._Tran;

        /// <summary>
        /// 初始化结束前
        /// </summary>
        public event RpcInitEnding InitEnding;
        /// <summary>
        /// 加载结束前
        /// </summary>
        public event RpcLoadEnding LoadEnding;

        private readonly IRpcEvent _RpcEvent;
        internal RpcInitOption (IRpcEvent rpcEvent)
        {
            this._RpcEvent = rpcEvent;
            this._Buffer = new IocBuffer(this._LoadTypes);
            this._ExtendService = new ExtendServiceBuffer(this._Buffer);
            this._Tran = new RpcTranRegService(this._Buffer);
            this._LocalEvent = new LocalEventBuffer(this._Buffer);
            this._Mapper = new MapperBuffer(this._Buffer);
            this._Route = new RouteBuffer(this._Buffer, RpcClient.Route);
            this._Subscribe = new RpcSubscribeBuffer(this._Buffer);
        }
        private void _LoadTypes (Type[] types, string name)
        {
            if (this._HasAssembly.Contains(name))
            {
                return;
            }
            this._HasAssembly.Add(name);
            types.ForEach(a =>
            {
                this._ExtendService.Reg(a);
                this._LocalEvent.Reg(a);
                this._Mapper.Add(a);
                this._Route.AddRoute(a);
                this._Subscribe.AddRoute(a);
            });
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        private void _Load (Action<RpcInitOption> action)
        {
            this._InitIoc();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            AppDomain.CurrentDomain.AssemblyLoad += this.CurrentDomain_AssemblyLoad;
            assemblies.ForEach(c =>
            {
                this._LoadAssembly(c);
            });
            this._RpcEvent.Load(this);
            LoadEnding?.Invoke(this);
            if (action != null)
            {
                action(this);
            }
        }

        private void CurrentDomain_AssemblyLoad (object sender, AssemblyLoadEventArgs args)
        {
            this._LoadAssembly(args.LoadedAssembly);
        }

        private void _LoadAssembly (Assembly assembly)
        {
            string name = assembly.GetName().Name;
            if (this._HasAssembly.Contains(name))
            {
                return;
            }
            this._HasAssembly.Add(name);
            Type[] types = assembly.GetTypes();
            bool isLoad = false;
            types.ForEach(a =>
            {
                this._ExtendService.Reg(a);
                this._LocalEvent.Reg(a);
                this._Mapper.Add(a);
                this._Route.AddRoute(a);
                this._Subscribe.AddRoute(a);
                if (this._LoadModular(a))
                {
                    isLoad = true;
                }
            });
            if (isLoad)
            {
                this._Buffer.Load(types);
            }
        }

        internal void Init (Action<RpcInitOption> loadEv, Action<IIocService> initEv)
        {
            this._Load(loadEv);
            AppDomain.CurrentDomain.AssemblyLoad -= this.CurrentDomain_AssemblyLoad;
            IocService.Init(this._Buffer);
            LocalEventService.InitLocalEvent(this._LocalEvent);
            this._Route.Init();
            IIocService ioc = RpcClient.Ioc;
            using (IocScope scope = ioc.CreateScore())
            {
                InitEnding?.Invoke(ioc);
                this._ExtendService.Init();
                this._Modulars.ForEach(a =>
                {
                    a.Init(ioc);
                });
                this._RpcEvent.ServerInit(ioc);
                if (initEv != null)
                {
                    initEv(ioc);
                }
                IRpcService service = RpcService.Service;
                this._Modulars.ForEach(a =>
                {
                    a.InitEnd(ioc, service);
                });
            }
        }
        /// <summary>
        /// 加载初始化模块
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private bool _LoadModular (Type type)
        {
            if (type.GetInterface(ConfigDic.InitModularType.FullName) != null)
            {
                IRpcInitModular modular = (IRpcInitModular)Activator.CreateInstance(type);
                modular.Load(this);
                this._Modulars.Add(modular);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 基本服务
        /// </summary>
        private void _InitIoc ()
        {
            _ = this._Buffer.RegisterInstance<IRpcService>(RpcService.Service);
            _ = this._Buffer.RegisterInstance<IConfigCollect>(LocalConfig.Local);
            _ = this._Buffer.RegisterInstance(RpcClient.LocalEvent);
            _ = this._Buffer.RegisterInstance(RpcClient.Ioc);
            _ = this._Buffer.RegisterInstance(RpcClient.Route);
            _ = this._Buffer.RegisterInstance(RpcClient.Config);
            _ = this._Buffer.RegisterInstance(RpcClient.Subscribe);
            _ = this._Buffer.RegisterInstance(RpcClient.RpcTran);
            this._Buffer.Load(this.GetType().Assembly);
            RpcTranService.RegTran("DefTran", new DefTranTemplate());
        }

        public void LoadModular<T> () where T : IRpcInitModular
        {
            this._LoadAssembly(typeof(T).Assembly);
        }
        /// <summary>
        /// 注册程序集中可用容器和事件
        /// </summary>
        /// <param name="assemblyName"></param>
        public void Load (string assemblyName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly obj = assemblies.Find(a => a.GetName().Name == assemblyName);
            if (obj != null)
            {
                this._LoadAssembly(obj);
            }
        }

        public void Dispose ()
        {
            this._HasAssembly.Clear();
            this._Modulars.Clear();
            this._Buffer.Dispose();
            this._Route.Dispose();
            this._ExtendService.Dispose();
        }
    }
}
