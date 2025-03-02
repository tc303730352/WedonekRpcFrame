using System;
using System.Text;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Model
{
    public class IocBody
    {
        public IocBody (Type form, Type to, string name, int sort) : this(form, to, sort)
        {
            this.Name = name;
        }
        public IocBody (Type form, Type to, object data, int sort) : this(form, to, sort)
        {
            this.LifetimeType = ClassLifetimeType.SingleInstance;
            this.Source = data;
        }
        public IocBody (Type form, Type to, object data, string name, int sort) : this(form, to, name, sort)
        {
            this.LifetimeType = ClassLifetimeType.SingleInstance;
            this.Source = data;
        }
        public IocBody (Type form, Type to, string name, ClassLifetimeType lifetimeType, int sort) : this(form, to, name, sort)
        {
            this.LifetimeType = lifetimeType;
        }
        public IocBody (Type form, Type to, string name, Type parent, int sort) : this(form, to, parent, sort)
        {
            this.Name = name;
        }
        public IocBody (Type form, Type to, Type parent, int sort) : this(form, to, sort)
        {
            this.LifetimeType = ClassLifetimeType.InstancePerOwned;
            this.Parent = parent;
        }
        public IocBody (Type form, Type to, int sort)
        {
            this.Sort = sort;
            this.Form = form;
            this.To = to;
        }

        public IocBody (Type form, Type to, ClassLifetimeType lifetimeType, int sort) : this(form, to, sort)
        {
            this.LifetimeType = lifetimeType;
        }
        public ClassLifetimeType? LifetimeType
        {
            get;
            internal set;
        }
        /// <summary>
        /// 实现的类
        /// </summary>
        public Type Form
        {
            get;
        }
        /// <summary>
        /// 目的接口
        /// </summary>
        public Type To
        {
            get;
        }
        /// <summary>
        /// 父级
        /// </summary>
        public Type Parent
        {
            get;
            internal set;
        }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 原数据
        /// </summary>
        public object Source
        {
            get;
        }
        /// <summary>
        /// 排序位
        /// </summary>
        public int Sort
        {
            get;
            set;
        }
        internal bool Check ()
        {
            if (this.LifetimeType == ClassLifetimeType.SingleInstance && this.Source != null)
            {
                return true;
            }
            return this.Form != this.To && this.To.CheckToIsIoc();
        }
        public void SetLifetimeType (Type parent)
        {
            this.LifetimeType = ClassLifetimeType.InstancePerOwned;
            this.Parent = parent;
        }
        public void SetLifetimeType (ClassLifetimeType lifetimeType)
        {
            this.LifetimeType = lifetimeType;
        }
        private string _Key;
        internal string Key
        {
            get
            {
                if (this._Key == null)
                {
                    StringBuilder str = new StringBuilder(this.Form.FullName);
                    if (!this.Name.IsNull())
                    {
                        _ = str.Append('_');
                        _ = str.Append(this.Name);
                    }
                    if (this.LifetimeType == ClassLifetimeType.InstancePerOwned)
                    {
                        _ = str.Append('_');
                        _ = str.Append(this.Parent.FullName);
                    }
                    this._Key = str.ToString().GetMd5();
                }
                return this._Key;
            }
        }
    }
}
