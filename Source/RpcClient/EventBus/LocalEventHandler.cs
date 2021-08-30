
using System;
using System.Reflection;

using RpcClient.Interface;

namespace RpcClient.EventBus
{
        [Attr.IgnoreIoc]
        internal class LocalEventHandler : ILocalHandler
        {
                private readonly string _Name = null;
                private readonly IUnityCollect _Unity = RpcClient.Unity;
                private readonly Type _Type = null;
                private readonly MethodInfo _Method = null;
                public LocalEventHandler(Type type, string name)
                {
                        this._Type = type;
                        this._Method = type.GetMethod("HandleEvent");
                        this._Name = name;
                }
                public void HandleEvent(object data,string name)
                {
                        object source = this._Unity.Resolve(this._Type, this._Name);
                        this._Method.Invoke(source, new object[] {
                                data,
                                name
                        });
                }
        }
}
