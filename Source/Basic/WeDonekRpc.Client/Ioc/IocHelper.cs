using System;
using System.Reflection;
using System.Text;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Ioc
{
    internal class IocHelper
    {
        private static readonly Type _AttrLifetime = typeof(ClassLifetimeAttr);
        private static readonly Type _IocSortAttr = typeof(IocSort);
        /// <summary>
        /// IOC标注的名称
        /// </summary>
        private static readonly Type _UnityName = typeof(IocName);
        public static string GetUnityName (Type to)
        {
            IocName name = (IocName)to.GetCustomAttribute(_UnityName);
            if (name != null)
            {
                return name.Name;
            }
            return null;
        }
        public static string GetIocBodyKey (IocBody body)
        {
            StringBuilder str = new StringBuilder(body.Form.FullName);
            if (!body.Name.IsNull())
            {
                _ = str.Append('_');
                _ = str.Append(body.Name);
            }
            if (body.LifetimeType == ClassLifetimeType.InstancePerOwned)
            {
                _ = str.Append('_');
                _ = str.Append(body.Parent.FullName);
            }
            else if (body.Source != null)
            {
                _ = str.Append('_');
                _ = str.Append(body.To.FullName);
            }
            return str.ToString().GetMd5();
        }
        public static int GetSort (Type to, int def)
        {
            IocSort attr = (IocSort)to.GetCustomAttribute(_IocSortAttr);
            if (attr == null)
            {
                return def;
            }
            return attr.Sort;
        }

        public static ClassLifetimeType? GetLifetimeType (Type form, Type to, out Type parent)
        {
            Attribute attr = to.GetCustomAttribute(_AttrLifetime);
            if (attr == null)
            {
                attr = form.GetCustomAttribute(_AttrLifetime);
            }
            if (attr != null)
            {
                ClassLifetimeAttr lifetime = (ClassLifetimeAttr)attr;
                parent = lifetime.Parent;
                return lifetime.LifetimeType;
            }
            parent = null;
            return null;
        }

    }
}
