using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.EventBus
{
    public delegate void LocalEventComplete (object sender, LocalEventArgs e);
    /// <summary>
    /// 本地事件
    /// </summary>
    internal class LocalEvent
    {
        /// <summary>
        /// 本地事件器
        /// </summary>
        private static readonly Dictionary<string, ILocalHandler> _Handlers = [];
        /// <summary>
        /// 处理事件委托
        /// </summary>
        private event Action<object, string> _HandleEvent = null;
        private LocalComplateEvent _CompleteEvent = null;
        private static readonly IIocService _Ioc = RpcClient.Ioc;
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName
        {
            get;
        }
        public LocalEvent (string name)
        {
            this.EventName = name;
        }
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="arg">事件参数</param>
        /// <param name="name">事件名</param>
        public void Post (object arg)
        {
            if (_HandleEvent != null)
            {
                this._ExecAction(arg);
            }
        }
        /// <summary>
        /// 异步发布事件
        /// </summary>
        /// <param name="arg">事件参数</param>
        public void AsyncPost (object arg)
        {
            if (this._HandleEvent != null)
            {
                _ = Task.Factory.StartNew(() =>
                {
                    using (IocScope scope = _Ioc.CreateScore())
                    {
                        this._ExecAction(arg);
                    }
                });
            }
        }
        /// <summary>
        /// 执行委托队列
        /// </summary>
        /// <param name="arg"></param>
        private void _ExecAction (object arg)
        {
            ErrorException error = null;
            try
            {
                _HandleEvent(arg, this.EventName);
            }
            catch (Exception e)
            {
                error = ErrorException.FormatError(e);
                error.Args.Add("Body", arg.ToJson());
                error.SaveLog("LocalEvent");
            }
            finally
            {
                this._Complete(arg, error);
            }
        }
        private void _Complete (object arg, ErrorException error)
        {
            if (this._CompleteEvent == null)
            {
                return;
            }
            try
            {
                this._CompleteEvent.Completed(arg, this.EventName, error);
            }
            catch (Exception e)
            {
                ErrorException ex = ErrorException.FormatError(e);
                error.Args.Add("Body", arg.ToJson());
                ex.SaveLog("LocalEvent");
            }
        }
        public void RegCompleteEvent (Type type, string name)
        {
            if (this._CompleteEvent == null)
            {
                this._CompleteEvent = new LocalComplateEvent(type, name);
            }
            else
            {
                throw new ErrorException("rpc.local.event.complate.reg.repeat")
                {
                    Args = new System.Collections.Generic.Dictionary<string, string>
                    {
                        {"Name",name },
                        {"Type",type.FullName }
                    }
                };
            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public void Reg (Type type, string name)
        {
            if (_Handlers.ContainsKey(name))
            {
                return;
            }
            ILocalHandler handler = new LocalEventHandler(type, name);
            if (_Handlers.TryAdd(name, handler))
            {
                _HandleEvent += handler.HandleEvent;
            }
        }
    }
}
