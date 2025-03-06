using System;
using System.Reflection;

using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Route;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Subscribe
{
    internal class SubscribeEvent : ISubscribeEvent
    {
        private readonly FuncParam[] _ParamList;
        private readonly MethodInfo _Method = null;
        private readonly Type _SourceType;
        private readonly bool _IsStatic;
        private readonly bool _IsParam;

        public SubscribeEvent ( string name, MethodInfo method )
        {
            ParameterInfo[] param = method.GetParameters();
            this._ParamList = param.ConvertAll(RpcClientHelper.GetParamType);
            this._Method = method;
            this.EventName = name;
            this._SourceType = method.DeclaringType;
            this._IsStatic = method.IsStatic;
            this._IsParam = this._ParamList.IsExists(a => a.ParamType == FuncParamType.参数 || a.ParamType == FuncParamType.数据源 || a.ParamType == FuncParamType.源);
        }
        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName
        {
            get;
        }
        public bool Init ()
        {
            if ( !this._ParamList.TrueForAll(a => a.ParamType == FuncParamType.数据源 || a.ParamType == FuncParamType.源 || a.ParamType == FuncParamType.参数) )
            {
                return false;
            }
            RpcSubscribeCollect.BindRoute(this.EventName);
            return true;
        }
        public bool Exec ( IMsg msg )
        {
            using ( IocScope scope = RpcClient.Ioc.CreateScore() )
            {
                if ( !RpcClientHelper.InitParam(msg, RpcClient.Ioc, this._ParamList, out object[] arg, this._IsParam) )
                {
                    return false;
                }
                object source = null;
                if ( !this._IsStatic )
                {
                    source = RpcClient.Ioc.Resolve(ConfigDic.RpcSubscribeService, this._SourceType.FullName);
                }
                return this._ExecFun(source, arg);
            }
        }
        private bool _ExecFun ( object source, object[] param )
        {
            try
            {
                _ = this._Method.Invoke(source, param);
                return true;
            }
            catch ( Exception e )
            {
                RpcLogSystem.AddReplyErrorLog(this._Method, this.EventName, this._SourceType, param, ErrorException.FormatError(e));
                return false;
            }
        }
    }
}
