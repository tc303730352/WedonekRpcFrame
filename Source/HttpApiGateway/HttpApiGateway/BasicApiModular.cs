using System;
using System.Reflection;

using HttpApiGateway.Collect;
using HttpApiGateway.Config;
using HttpApiGateway.Interface;

using RpcClient;

using RpcHelper;

namespace HttpApiGateway
{
        /// <summary>
        /// 模块的基类
        /// </summary>
        public class BasicApiModular : IApiModular
        {
                public BasicApiModular(string name)
                {
                        this.ServiceName = name;
                }
                public IModularConfig Config
                {
                        get;
                } = new ModularConfig();

                public string ServiceName
                {
                        get;
                }

                public string ApiRouteFormat => this.Config.ApiRouteFormat;

                public void InitModular()
                {
                        this._LoadModular();
                        this.Init();
                }
                private void _LoadModular()
                {
                        Type type = this.GetType();
                        Assembly assembly = type.Assembly;
                        RpcClient.RpcClient.Load(assembly);
                        Type[] types = assembly.GetTypes();
                        using (IResourceCollect resource = ResourceCollect.Create(this))
                        {
                                types.ForEach(a =>
                                {
                                        if (a.IsInterface)
                                        {
                                                return;
                                        }
                                        else if (UnityCollect.RegisterApi(a))
                                        {
                                                IApiController api = UnityCollect.GetApi(a);
                                                ApiRouteCollect.RegApi(a, api, this, resource.RegRoute);
                                        }
                                        else if (UnityCollect.RegisterGateway(a))
                                        {
                                                ApiRouteCollect.RegApi(a, this, resource.RegRoute);
                                        }
                                });
                        }
                }

                protected virtual void Init()
                {

                }

                public void Start()
                {
                }

                public void Dispose()
                {
                }
        }
}
