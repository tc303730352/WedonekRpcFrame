using System;

namespace WeDonekRpc.Client.Attr
{
    /// <summary>
    /// IOC排序 排序位 大的覆盖小的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IocSort : Attribute
    {
        public IocSort(int sort)
        {
            this.Sort = sort;
        }
        public int Sort { get; }
    }
}
