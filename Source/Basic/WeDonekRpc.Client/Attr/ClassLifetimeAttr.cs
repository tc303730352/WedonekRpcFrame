using System;

namespace WeDonekRpc.Client.Attr
{
    /// <summary>
    /// 
    /// </summary>
    public enum ClassLifetimeType
    {
        /// <summary>
        /// 嵌套范围单例
        /// </summary>
        Scope = 3,
        /// <summary>
        /// 单例
        /// </summary>
        SingleInstance = 1,
        /// <summary>
        /// 每次请求服务都会返回一个新实例
        /// </summary>
        InstancePerDependency = 2,
        /// <summary>
        /// 分层周期生命范围单例
        /// </summary>
        InstancePerMatchingLifetimeScope = 0,
        /// <summary>
        /// 隐式关联类型创建嵌套的生命周期范围
        /// </summary>
        InstancePerOwned = 4
    }
    /// <summary>
    /// IOC生命周期声明
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ClassLifetimeAttr : Attribute
    {
        private readonly ClassLifetimeType _LifetimeType = ClassLifetimeType.InstancePerMatchingLifetimeScope;
        public ClassLifetimeAttr (ClassLifetimeType type)
        {
            this._LifetimeType = type;
        }
        public ClassLifetimeAttr (Type parent)
        {
            this.Parent = parent;
            this._LifetimeType = ClassLifetimeType.InstancePerOwned;
        }
        /// <summary>
        /// 类型
        /// </summary>
        public ClassLifetimeType LifetimeType => this._LifetimeType;

        public Type Parent
        {
            get;
            set;
        }
    }
}
