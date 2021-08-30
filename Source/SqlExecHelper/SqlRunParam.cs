using System.Collections.Concurrent;
using System.Threading;

namespace SqlExecHelper
{
        public class SqlRunParam : System.IDisposable
        {
                private static readonly ConcurrentDictionary<int, RunParam> _RunParam = new ConcurrentDictionary<int, RunParam>();
                private readonly int _ThreadId = 0;

                public SqlRunParam(string lockType, string index)
                {
                        int id = Thread.CurrentThread.ManagedThreadId;
                        this._ThreadId = id;
                        _RunParam.TryAdd(id, new RunParam(lockType, index));
                }

                internal static RunParam GetParam(RunParam def)
                {
                        int id = Thread.CurrentThread.ManagedThreadId;
                        return _RunParam.TryGetValue(id, out RunParam param) ? param : def;
                }
                public void Dispose()
                {
                        _RunParam.TryRemove(this._ThreadId, out _);
                }
        }
}
