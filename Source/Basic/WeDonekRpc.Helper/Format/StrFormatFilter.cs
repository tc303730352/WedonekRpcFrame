using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace WeDonekRpc.Helper.Format
{
    /// <summary>
    /// 格式化字符串
    /// </summary>
    internal class FormatFilterMethod : IStrFormatFilter
    {
        private readonly MethodInfo _Method = null;
        private readonly Type _SourceType = null;
        private readonly int[] _Param = null;
        public FormatFilterMethod ( MethodInfo method, Type source )
        {
            this._Method = method;
            this._SourceType = source;
            ParameterInfo[] param = method.GetParameters();
            this._Param = Array.ConvertAll(param, a =>
            {
                return a.ParameterType.FullName == this._SourceType.FullName ? 2 : 1;
            });
        }

        public string FormatStr ( object source, string str )
        {
            if ( str == string.Empty )
            {
                return str;
            }
            object[] args = Array.ConvertAll(this._Param, a =>
            {
                return a == 1 ? str : source;
            });
            return (string)this._Method.Invoke(source, args);
        }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StrFormatFilter : Attribute, IStrFormatFilter
    {
        private readonly FilterHtmlType _filterType = FilterHtmlType.HTML;
        private readonly bool _IsDropEscape = true;
        private readonly string _FuncName = null;
        public StrFormatFilter ( FilterHtmlType filter, bool isDropEscape )
        {
            this._filterType = filter;
            this._IsDropEscape = isDropEscape;
        }
        public StrFormatFilter ( bool isDropEscape )
        {
            this._IsDropEscape = isDropEscape;
        }
        public StrFormatFilter ( string funcName )
        {
            this._FuncName = funcName;
        }
        public StrFormatFilter ()
        {

        }

        private static readonly ConcurrentDictionary<string, IStrFormatFilter> _Method = new ConcurrentDictionary<string, IStrFormatFilter>();

        private static bool _GetMethod ( object source, string func, out IStrFormatFilter method )
        {
            Type type = source.GetType();
            string name = string.Join("_", type.FullName, func);
            if ( _Method.TryGetValue(name, out method) )
            {
                return true;
            }
            MethodInfo fun = type.GetMethod(func, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);
            if ( fun == null )
            {
                return false;
            }
            method = new FormatFilterMethod(fun, type);
            _Method.TryAdd(name, method);
            return true;
        }
        public string FormatStr ( object source, string str )
        {
            if ( this._FuncName != null )
            {
                return !_GetMethod(source, this._FuncName, out IStrFormatFilter method) ? str : method.FormatStr(source, str);
            }
            if ( this._IsDropEscape )
            {
                str = Tools.DropEscapeChar(str);
            }
            return Tools.FilterHtml(str, this._filterType);
        }

    }
}
