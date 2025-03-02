namespace RpcExtend.Service.AutoRetry
{
    internal class RetryTaskWheel
    {
        private readonly List<long> _tasks = [];

        private int _State = 0;

        public RetryTaskWheel (long time)
        {
            this.CurrentTime = time;
        }
        public long CurrentTime
        {
            get;
        }

        public List<long> Tasks => this._tasks;

        public bool Add (long taskId)
        {
            do
            {
                //0 待执行 1 正在插入数据 2 锁定正执行
                int state = Interlocked.CompareExchange(ref this._State, 1, 0);
                if (state == 0)
                {
                    this._tasks.Add(taskId);
                    _ = Interlocked.Exchange(ref this._State, 0);
                    return true;
                }
                else if (state == 2)
                {
                    return false;
                }
            } while (true);
        }

        public long[] LockTask ()
        {
            do
            {
                int res = Interlocked.CompareExchange(ref this._State, 2, 0);
                if (res == 0)
                {
                    return this._tasks.ToArray();
                }
                else if (res == 2)
                {
                    return Array.Empty<long>();
                }
            } while (true);
        }
    }
}
