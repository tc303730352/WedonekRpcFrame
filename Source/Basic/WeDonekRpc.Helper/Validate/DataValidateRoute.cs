using System;
using System.Collections.Generic;

namespace WeDonekRpc.Helper.Validate
{
    /// <summary>
    /// 委托验证的方法
    /// </summary>
    /// <param name="source">原数据对象</param>
    /// <param name="attrType">属性类型</param>
    /// <param name="val">属性值</param>
    /// <param name="root">包含源数据的上一级对象（单级为空）</param>
    /// <returns>是否通过验证</returns>
    public delegate bool DataValidateFun (object source, Type attrType, object val, object root);

    /// <summary>
    /// 数据验证事件委托
    /// </summary>
    public class DataValidateRoute
    {
        private static readonly Dictionary<string, DataValidateFun> _Route = [];

        internal static bool ValidateData (DataValidateAttr attr, Type attrType, object source, object val, object root)
        {
            if (_Route.TryGetValue(attr.RouteName, out DataValidateFun fun))
            {
                return !fun.Invoke(source, attrType, val, root);
            }
            return false;
        }
        public static void SetRoute (string name, DataValidateFun fun)
        {
            if (_Route.ContainsKey(name))
            {
                _Route[name] = fun;
            }
            else
            {
                _Route.Add(name, fun);
            }
        }
    }
}
