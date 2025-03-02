using System;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;
namespace WeDonekRpc.ApiGateway.PlugIn
{
    [IgnoreIoc]
    public class BasicPlugIn : IPlugIn
    {
        private readonly ReadWriteLockHelper _Lock = new ReadWriteLockHelper();
        private PlugInStateChange _Change;
        public BasicPlugIn (string name)
        {
            this.Name = name;
        }
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name
        {
            get;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable
        {
            get;
        }
        /// <summary>
        /// 销毁插件
        /// </summary>
        /// <param name="change"></param>
        public void Dispose (PlugInStateChange change)
        {
            if (change != null)
            {
                if (this._Lock.GetWriteLock())
                {
                    this._Change -= change;
                    this._Lock.ExitWrite();
                }
            }
            this._Lock.Dispose();
            this._Dispose();
        }
        /// <summary>
        /// 内部销毁插件
        /// </summary>
        protected virtual void _Dispose ()
        {

        }
        /// <summary>
        /// 插件状态变更事件
        /// </summary>
        protected void _ChangeEvent ()
        {
            if (this._Change != null)
            {
                Delegate[] methods = null;
                if (this._Lock.GetReadLock())
                {
                    methods = this._Change.GetInvocationList();
                    this._Lock.ExitRead();
                }
                methods?.ForEach(a => a.DynamicInvoke(this));
            }
        }
        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <param name="change"></param>
        public void Init (PlugInStateChange change)
        {
            if (change != null)
            {
                if (this._Lock.GetWriteLock())
                {
                    this._Change += change;
                    this._Lock.ExitWrite();
                }
            }
            this._InitPlugIn();
        }
        /// <summary>
        /// 内部初始化
        /// </summary>
        protected virtual void _InitPlugIn ()
        {

        }
    }
}
