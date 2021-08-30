using System.Threading;
namespace SqlExecHelper
{
        internal class LockHelper : System.IDisposable
        {
                private readonly object _LockData = new object();

                private int _OutTime = 2000;

                /// <summary>
                /// 超时时间
                /// </summary>
                public int OutTime
                {
                        get => this._OutTime;
                        set => this._OutTime = value;
                }

                private readonly int _WaitOutTime = 2000;

                /// <summary>
                /// 等待超时时间
                /// </summary>
                public int WaitOutTime
                {
                        get => this._OutTime;
                        set => this._OutTime = value;
                }

                /// <summary>
                /// 获得锁
                /// </summary>
                /// <returns></returns>
                public bool GetLock()
                {
                        if (Monitor.TryEnter(this._LockData, this._OutTime))
                        {
                                this._IsExit = false;
                                return true;
                        }
                        return false;
                }

                /// <summary>
                /// 释放锁
                /// </summary>
                public void Exit()
                {
                        Monitor.Exit(this._LockData);
                        this._IsExit = true;
                }
                /// <summary>
                ///  释放对象上的锁并阻止当前线程，直到它重新获取该锁。
                /// </summary>
                /// <returns></returns>
                public bool Wait()
                {
                        return Monitor.Wait(this._LockData, this._WaitOutTime);
                }

                /// <summary>
                ///  通知等待队列中的线程锁定对象状态的更改
                /// </summary>
                public void Pulse()
                {
                        Monitor.Pulse(this._LockData);
                }
                private bool _IsExit = true;
                public void Dispose()
                {
                        if (!this._IsExit)
                        {
                                this.Exit();
                        }
                }
        }
}
