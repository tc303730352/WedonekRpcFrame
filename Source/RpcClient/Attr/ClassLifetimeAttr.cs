using System;

namespace RpcClient.Attr
{
        public enum ClassLifetimeType
        {
                单例 = 1,
                默认 = 2,
                分层 = 3,
                循环引用 = 4,
                线程 = 5,
                外部 = 6
        }
        /// <summary>
        /// IOC生命周期声明
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
        public class ClassLifetimeAttr : Attribute
        {
                private readonly ClassLifetimeType _LifetimeType = ClassLifetimeType.默认;
                public ClassLifetimeAttr(ClassLifetimeType type)
                {
                        this._LifetimeType = type;
                }
                /// <summary>
                /// 类型
                /// </summary>
                public ClassLifetimeType LifetimeType => this._LifetimeType;
        }
}
