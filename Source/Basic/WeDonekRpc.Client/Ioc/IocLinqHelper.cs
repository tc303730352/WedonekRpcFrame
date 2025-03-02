using System;
using System.Reflection;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
namespace WeDonekRpc.Client.Ioc
{
    public static class IocLinqHelper
    {
        private const int _AutoRegIcoSort = 0;
        private static readonly Type _IgnoreType = typeof(IgnoreIoc);
        public static void Load (this IocBuffer ioc, string assemblyName, int sort = _AutoRegIcoSort)
        {
            Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies();
            Assembly obj = assembly.Find(a => a.GetName().Name == assemblyName);
            if (obj != null)
            {
                ioc.Load(obj, sort);
            }
        }
        public static void Load (this IocBuffer ioc, Assembly form, Type[] classs, int sort = _AutoRegIcoSort)
        {
            Type[] inters = form.GetTypes().FindAll(a => _CheckInterface(a));
            inters.ForEach(a =>
            {
                ioc.Register(a, classs, sort);
            });
            ioc.Load(form, sort);
        }
        private static bool _CheckInterface (Type type)
        {
            if (!type.IsInterface || !type.IsPublic)
            {
                return false;
            }
            else if (type.Namespace.StartsWith("System.")
                || type.Namespace == "System"
                || type.Namespace == "Microsoft"
                || type.Namespace.StartsWith("Microsoft."))
            {
                return false;
            }
            return true;
        }
        public static void Load (this IocBuffer ioc, Type[] types, int sort = _AutoRegIcoSort)
        {
            Type[] inters = types.FindAll(a => _CheckInterface(a));
            Type[] classs = types.FindAll(a => a.IsClass);
            inters.ForEach(a =>
            {
                ioc.Register(a, classs, sort);
            });
            classs.ForEach(a =>
            {
                if (!a.CheckToIsIoc())
                {
                    return;
                }
                Type[] tInter = a.GetInterfaces();
                if (tInter.Length > 0)
                {
                    tInter.ForEach(c =>
                    {
                        if (!inters.IsExists(c) && _CheckInterface(c) && _CheckIsReg(a, c))
                        {
                            IocBody body = ioc.Register(c, a, sort);
                            if (body != null)
                            {
                                ioc.Load(c.Assembly, types, sort);
                            }
                        }
                    });
                }
            });
        }
        public static void LoadType (this IocBuffer buffer, Type type, int sort = _AutoRegIcoSort)
        {
            Type[] types = type.Assembly.GetTypes().FindAll(c => c.IsClass);
            buffer.Register(type, types, sort);
        }
        private static bool _RegisterClass (IocBuffer buffer, Type form, int sort, ref Type[] to)
        {
            if (to.Length == 1)
            {
                return buffer.RegisterType(form, to[0], sort) != null;
            }
            else
            {
                to = to.FindAll(c =>
                {
                    return buffer.RegisterType(form, c, sort) != null;
                });
                return to.Length > 0;
            }
        }
        public static bool CheckToIsIoc (this Type type)
        {
            if (type.IsValueType || type.IsInterface || type.IsAbstract || type.IsPrimitive)
            {
                return false;
            }
            else if (type.BaseType == PublicDataDic.MulticastDelegateType
                || type.BaseType == PublicDataDic.ExceptionType
                || type.BaseType == PublicDataDic.DelegateType
                || type.BaseType == PublicDataDic.AttributeType)
            {
                return false;
            }
            else if (type.BaseType != PublicDataDic.ObjectType)
            {
                Type parent = type.BaseType;
                do
                {
                    if (parent == PublicDataDic.DelegateType
                        || type.BaseType == PublicDataDic.ExceptionType
                        || type.BaseType == PublicDataDic.AttributeType)
                    {
                        return false;
                    }
                    parent = parent.BaseType;
                } while (parent != PublicDataDic.ObjectType);
            }
            ConstructorInfo[] list = type.GetConstructors().FindAll(a => a.IsPublic && _CheckIsReg(a));
            return list.Length != 0;
        }
        private static bool _CheckIsReg (Type type, Type form)
        {
            if (type.IsValueType || type.IsAbstract || type.GetCustomAttribute(_IgnoreType) != null)
            {
                return false;
            }
            else if (!type.GetInterfaces().IsExists(a => a.FullName == form.FullName))
            {
                return false;
            }
            else if (type.IsGenericType != form.IsGenericType)
            {
                return false;
            }
            else if (type.IsGenericType)
            {
                Type[] toG = type.GetGenericArguments();
                Type[] fG = form.GetGenericArguments();
                if (!toG.IsEquals(fG))
                {
                    return false;
                }
            }
            ConstructorInfo[] list = type.GetConstructors().FindAll(a => a.IsPublic && _CheckIsReg(a));
            return list.Length != 0;
        }

        private static bool _CheckIsReg (ConstructorInfo constructor)
        {
            ParameterInfo[] param = constructor.GetParameters();
            if (param.Length == 0)
            {
                return true;
            }
            return param.TrueForAll(a => _CheckInterface(a.ParameterType));
        }
        public static void Register (this IocBuffer buffer, Type form, Type[] classs, int sort = _AutoRegIcoSort)
        {
            if (form.GetCustomAttribute(_IgnoreType) != null)
            {
                return;
            }
            Type[] list = classs.FindAll(c => _CheckIsReg(c, form));
            if (list.Length == 0)
            {
                return;
            }
            else if (_RegisterClass(buffer, form, sort, ref list))
            {
                list.ForEach(b => buffer.Register(b, sort));
            }
        }
        public static bool IsIgnore (this Type type)
        {
            return type.GetCustomAttribute(_IgnoreType) != null;
        }
        public static bool IsRegistered (this IIocService ioc, IocBody body)
        {
            if (body.Name.IsNull())
            {
                return ioc.IsRegistered(body.Form);
            }
            return ioc.IsRegistered(body.Form, body.Name);
        }
        /// <summary>
        /// 注册构造函数中的接口
        /// </summary>
        /// <param name="type"></param>
        public static void RegConstructor (this IocBuffer buffer, Type type, int sort = _AutoRegIcoSort)
        {
            ConstructorInfo[] list = type.GetConstructors().FindAll(a => a.IsPublic && _CheckIsReg(a));
            if (list.Length == 0)
            {
                return;
            }
            list.ForEach(c =>
            {
                ParameterInfo[] param = c.GetParameters();
                if (!param.IsNull())
                {
                    param.ForEach(a =>
                    {
                        buffer.Load(a.ParameterType.Assembly, sort);
                    });
                }
            });
        }

        public static void InitBody (this IocBody body, Action<IocBody> def)
        {
            if (body.Name.IsNull())
            {
                body.Name = IocHelper.GetUnityName(body.To);
            }
            body.Sort = IocHelper.GetSort(body.To, body.Sort);
            if (!body.LifetimeType.HasValue)
            {
                ClassLifetimeType? lifetimeType = IocHelper.GetLifetimeType(body.Form, body.To, out Type parent);
                if (lifetimeType.HasValue)
                {
                    body.LifetimeType = lifetimeType.Value;
                    body.Parent = parent;
                }
                else
                {
                    def(body);
                }
            }
        }
        public static bool RegConstructors (this IocBuffer buffer, Type type, int sort = _AutoRegIcoSort)
        {
            ConstructorInfo[] argType = type.GetConstructors();
            if (argType.Length == 0)
            {
                return false;
            }
            bool isReg = false;
            argType.ForEach(a =>
            {
                if (!a.IsPublic || !_CheckIsReg(a))
                {
                    return;
                }
                isReg = true;
                ParameterInfo[] param = a.GetParameters();
                if (param.Length == 0)
                {
                    return;
                }
                param.ForEach(b =>
                {
                    buffer.Load(b.ParameterType.Assembly, sort);
                });
            });
            return isReg;
        }
    }
}
