using System;
using System.Reflection;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Route
{
    /// <summary>
    /// 方法路由
    /// </summary>
    internal abstract class Route : ITcpRoute
    {
        private readonly bool _IsParam = true;
        private readonly bool _IsStatic = false;
        private static readonly IIocService _Ioc = RpcClient.Ioc;
        protected readonly FuncParam[] _ParamList = null;

        protected readonly Type _SourceType = null;

        protected readonly MethodInfo _Method = null;

        public string RouteName
        {
            get;
        }
        public bool IsSystemRoute
        {
            get;
        }
        public MethodInfo Source
        {
            get => this._Method;
        }
        public TcpMsgEvent TcpMsgEvent
        {
            get;
        }
        private string _Show = null;
        public string RouteShow
        {
            get
            {
                if (this._Show == null)
                {
                    this._Show = XmlShowHelper.FindParamShow(this._Method);
                }
                return this._Show;
            }
        }

        public Route (string name, MethodInfo method)
        {
            this.IsSystemRoute = method.Module.ScopeName == "WeDonekRpc.Client.dll";
            ParameterInfo[] param = method.GetParameters();
            this._ParamList = param.ConvertAll(a => RpcClientHelper.GetParamType(a));
            this.RouteName = name;
            this._SourceType = method.DeclaringType;
            this._IsStatic = method.IsStatic;
            this._Method = method;
            this.TcpMsgEvent = new TcpMsgEvent(this._MsgEvent);
            this._IsParam = this._ParamList.IsExists(a => a.ParamType == FuncParamType.参数 || a.ParamType == FuncParamType.数据源 || a.ParamType == FuncParamType.源);
        }

        /// <summary>
        /// 将方法转成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString ()
        {
            return this._Method.ToString();
        }
        private IBasicRes _MsgEvent (IMsg msg)
        {
            if (!RpcClientHelper.InitParam(msg, this._ParamList, out object[] arg, this._IsParam))
            {
                return new BasicRes("public.param.null");
            }
            object source = null;
            if (!this._IsStatic)
            {
                source = _Ioc.Resolve(ConfigDic.RpcApiService, this._SourceType.FullName);
            }
            return this._ExecFun(source, arg);
        }
        protected virtual IBasicRes _ExecFun (object source, object[] param)
        {
            return null;
        }


    }
}
