using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RouteService
{
    internal delegate void ExecAction (object target, object[] args);
    internal delegate object ExecFunc (object target, object[] args);

    internal delegate IBasicRes ExecRoute (object target, object[] args);

    internal delegate Task ExecTaskAction (object target, object[] args);
    internal class RpcRouteHelper
    {

        public static ExecTaskAction GetExecTaskAction (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression arrays = Expression.Parameter(typeof(object[]), "args");
            ParameterInfo[] parameters = method.GetParameters();
            Expression[] list = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = Expression.Convert(Expression.ArrayIndex(arrays, Expression.Constant(i)), parameters[i].ParameterType);
            }
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, list);
                return Expression.Lambda<ExecTaskAction>(methodExp, target, arrays).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method, list);
                return Expression.Lambda<ExecTaskAction>(methodExp, target, arrays).Compile();
            }
        }

        public static ExecRoute GetExecRoute (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression arrays = Expression.Parameter(typeof(object[]), "args");
            ParameterInfo[] parameters = method.GetParameters();
            Expression[] list = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = Expression.Convert(Expression.ArrayIndex(arrays, Expression.Constant(i)), parameters[i].ParameterType);
            }
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, list);
                return Expression.Lambda<ExecRoute>(methodExp, target, arrays).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method, list);
                return Expression.Lambda<ExecRoute>(methodExp, target, arrays).Compile();
            }
        }
        public static ExecAction GetExecAction (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression arrays = Expression.Parameter(typeof(object[]), "args");
            ParameterInfo[] parameters = method.GetParameters();
            Expression[] list = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = Expression.Convert(Expression.ArrayIndex(arrays, Expression.Constant(i)), parameters[i].ParameterType);
            }
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, list);
                return Expression.Lambda<ExecAction>(methodExp, target, arrays).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method, list);
                return Expression.Lambda<ExecAction>(methodExp, target, arrays).Compile();
            }
        }
        public static ExecFunc GetExecFunc (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression arrays = Expression.Parameter(typeof(object[]), "args");
            ParameterInfo[] parameters = method.GetParameters();
            Expression[] list = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = Expression.Convert(Expression.ArrayIndex(arrays, Expression.Constant(i)), parameters[i].ParameterType);
            }
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, list);
                return Expression.Lambda<ExecFunc>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target, arrays).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method, list);
                return Expression.Lambda<ExecFunc>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target, arrays).Compile();
            }
        }

    }
}
