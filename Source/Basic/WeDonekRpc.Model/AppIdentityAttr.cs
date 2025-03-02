using System;

namespace WeDonekRpc.Model
{
    /// <summary>
    /// 应用身份标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AppIdentityAttr : Attribute
    {
    }
}
