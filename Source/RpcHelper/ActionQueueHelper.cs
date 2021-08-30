using System;

namespace RpcHelper
{
        public delegate void DelegateAction<T>(T arg);
        /// <summary>
        /// 委托执行队列
        /// </summary>
        public class ActionQueueHelper
        {
                private struct _ActionClass
                {
                        public Delegate Delegate
                        {
                                get;
                                set;
                        }
                        public dynamic Arg
                        {
                                get;
                                set;
                        }
                        public void Invoke()
                        {
                                this.Delegate.DynamicInvoke(new object[]
                                {
                                        this.Arg
                                });
                        }
                }
                private static readonly IDataQueueHelper<_ActionClass> _ActionQueue = new DataQueueHelper<_ActionClass>(_ActionFun, 10, 10);

                private static void _ActionFun(_ActionClass data)
                {
                        data.Invoke();
                }
                public static void AddAction<T>(DelegateAction<T> action, T arg)
                {
                        _ActionQueue.AddQueue(new _ActionClass
                        {
                                Delegate = action,
                                Arg = arg
                        });
                }
        }
}
