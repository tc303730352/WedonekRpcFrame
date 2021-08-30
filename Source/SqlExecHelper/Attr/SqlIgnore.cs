using System;

namespace SqlExecHelper
{
        /// <summary>
        /// SQL执行时忽略方式
        /// </summary>
        [AttributeUsage(AttributeTargets.Property, Inherited = false)]
        public class SqlIgnore : Attribute
        {
                public SqlIgnoreType IgnoreType
                {
                        get;
                        set;
                }
                public SqlIgnore(SqlIgnoreType type = SqlIgnoreType.query | SqlIgnoreType.insert | SqlIgnoreType.update)
                {
                        this.IgnoreType = type;
                }

        }
}
