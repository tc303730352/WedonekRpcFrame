using System;
using System.Collections.Generic;
using System.Reflection;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.RouteService;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.EventBus
{
    internal class LocalComplateEvent
    {
        private readonly string _Name = null;
        private readonly IIocService _Unity = RpcClient.Ioc;
        private readonly Type _Type = null;
        private readonly ExecAction _Method = null;
        public LocalComplateEvent (Type type, string name)
        {
            this._Type = type;
            MethodInfo method = type.GetMethod("Completed");
            this._Method = RpcRouteHelper.GetExecAction(method);
            this._Name = name;
        }
        public void Completed (object sender, string name, ErrorException error)
        {
            object source = this._Unity.Resolve(this._Type, this._Name);
            try
            {
                this._Method(source, new object[] {
                                sender,
                                new LocalEventArgs(name,error)
                        });
            }
            catch (Exception e)
            {
                ErrorException ex = ErrorException.FormatError(e);
                ex.Args = new Dictionary<string, string>
                {
                    {"Type",this._Type.FullName },
                    {"Name",this._Name }
                };
                throw ex;
            }
        }
    }
}
