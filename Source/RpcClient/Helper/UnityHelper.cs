using System;
using System.Reflection;

using RpcClient.Attr;

using Unity.Lifetime;

namespace RpcClient.Helper
{
        internal class UnityHelper
        {
                private static readonly Type _AttrLifetime = typeof(ClassLifetimeAttr);
                public static ITypeLifetimeManager GetLifetime(Type form, Type to)
                {
                        ClassLifetimeType type = _GetLifetimeType(form, to);
                        switch (type)
                        {
                                case ClassLifetimeType.单例:
                                        return new ContainerControlledLifetimeManager();
                                case ClassLifetimeType.分层:
                                        return new HierarchicalLifetimeManager();
                                case ClassLifetimeType.外部:
                                        return new ExternallyControlledLifetimeManager();
                                case ClassLifetimeType.循环引用:
                                        return new PerResolveLifetimeManager();
                                case ClassLifetimeType.线程:
                                        return new PerThreadLifetimeManager();
                                default:
                                        return new TransientLifetimeManager();
                        }
                }
                private static ClassLifetimeType _GetLifetimeType(Type form, Type to)
                {
                        Attribute attr = to.GetCustomAttribute(_AttrLifetime);
                        if (attr != null)
                        {
                                return ((ClassLifetimeAttr)attr).LifetimeType;
                        }
                        attr = form.GetCustomAttribute(_AttrLifetime);
                        if (attr != null)
                        {
                                return ((ClassLifetimeAttr)attr).LifetimeType;
                        }
                        return ClassLifetimeType.默认;
                }

        }
}
