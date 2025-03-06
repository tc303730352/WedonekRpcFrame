using System;
using System.Reflection;

using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Route;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Subscribe
{
    internal class AcionDelegate : ISubscribeEvent
    {
        private readonly bool _IsParam = true;

        protected readonly FuncParam[] _ParamList = null;

        protected readonly Delegate _Source = null;

        protected readonly MethodInfo _Method = null;

        public string EventName { get; }

        public AcionDelegate ( string name, Delegate func )
        {
            this._Source = func;
            this._Method = func.Method;
            ParameterInfo[] param = this._Method.GetParameters();
            this._ParamList = param.ConvertAll(RpcClientHelper.GetParamType);
            this.EventName = name;
            this._IsParam = this._ParamList.IsExists(a => a.ParamType == FuncParamType.参数 || a.ParamType == FuncParamType.数据源 || a.ParamType == FuncParamType.源);
        }
        public virtual bool VerificationRoute ()
        {
            return true;
        }

        private bool _ExecFun ( object[] param )
        {
            try
            {
                _ = this._Method.Invoke(this._Source, param);
                return true;
            }
            catch ( Exception e )
            {
                RpcLogSystem.AddReplyErrorLog(this._Method, this.EventName, param, ErrorException.FormatError(e));
                return false;
            }
        }

        public bool Exec ( IMsg msg )
        {
            using ( IocScope scope = RpcClient.Ioc.CreateScore() )
            {
                if ( !RpcClientHelper.InitParam(msg, RpcClient.Ioc, this._ParamList, out object[] arg, this._IsParam) )
                {
                    return false;
                }
                return this._ExecFun(arg);
            }
        }

        public bool Init ()
        {
            RpcSubscribeCollect.BindRoute(this.EventName);
            return true;
        }
    }
}
