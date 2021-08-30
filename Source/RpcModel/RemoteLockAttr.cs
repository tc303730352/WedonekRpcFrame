using System;

namespace RpcModel
{
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public class RemoteLockAttr : Attribute
        {

        }
}
