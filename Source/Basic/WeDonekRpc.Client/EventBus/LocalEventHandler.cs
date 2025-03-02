
using System;
using System.Reflection;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.RouteService;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.EventBus
{
    [Attr.IgnoreIoc]
    internal class LocalEventHandler : ILocalHandler
    {
        private readonly string _Name = null;
        private readonly IIocService _Unity = RpcClient.Ioc;
        private readonly Type _Type = null;
        private readonly ExecAction _Method = null;
        public LocalEventHandler (Type type, string name)
        {
            this._Type = type;
            MethodInfo method = type.GetMethod("HandleEvent");
            this._Method = RpcRouteHelper.GetExecAction(method);
            this._Name = name;
        }
        public void HandleEvent (object data, string name)
        {
            object source = this._Unity.Resolve(this._Type, this._Name);
            try
            {
                this._Method(source, new object[] {
                                data,
                                name
                        });
            }
            catch (Exception e)
            {
                ErrorException ex = ErrorException.FormatError(e);
                ex.Args = new System.Collections.Generic.Dictionary<string, string>
                 {
                    {"Type",this._Type.FullName },
                    {"Name",this._Name }
                };
                throw ex;
            }
        }
    }
}
