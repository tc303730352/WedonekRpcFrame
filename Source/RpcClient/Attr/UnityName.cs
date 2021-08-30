using System;

namespace RpcClient.Attr
{
        /// <summary>
        /// 指定IOC名称名称
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
        public class UnityName : Attribute
        {
                public string Name { get; }

                public UnityName(string name)
                {
                        this.Name = name;
                }
        }
}
