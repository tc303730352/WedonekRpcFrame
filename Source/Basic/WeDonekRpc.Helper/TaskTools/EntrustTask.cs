using System;
using System.Threading.Tasks;

namespace WeDonekRpc.Helper.TaskTools
{
    public delegate void EntrustFunc<T, Result> (T param, ITaskState<Result> result);
    public class EntrustTask<T, Result> : IEntrustTask<Result>
    {
        private readonly T _Param;
        private readonly ITaskState<Result> _TaskState = null;
        private readonly EntrustFunc<T, Result> _Task = null;
        private readonly Action _TaskEnd;
        private Task _RunTask = null;
        private readonly int _OverTime = 0;
        public Guid TaskId
        {
            get;
        } = Guid.NewGuid();
        public int EndTime => this._TaskState.EndTime;

        public bool IsEnd => this._TaskState.IsEnd;

        public EntrustTask (T param, EntrustFunc<T, Result> func, int overTime = 60)
        {
            this._Task = func;
            this._Param = param;
            this._OverTime = HeartbeatTimeHelper.HeartbeatTime + overTime;
            this._TaskState = new TaskState<Result>(this.TaskId);
        }
        public EntrustTask (T param, EntrustFunc<T, Result> func, Action end, int overTime = 60)
        {
            this._TaskEnd = end;
            this._Task = func;
            this._Param = param;
            this._OverTime = HeartbeatTimeHelper.HeartbeatTime + overTime;
            this._TaskState = new TaskState<Result>(this.TaskId);
        }

        public void ExecTask ()
        {
            this._RunTask = Task.Run(() =>
              {
                  try
                  {
                      this._Task.Invoke(this._Param, this._TaskState);
                  }
                  catch (Exception e)
                  {
                      ErrorException error = ErrorException.FormatError(e);
                      this._TaskState.ExecFail(error.ToString());
                      error.SaveLog("EntrustTask");
                  }
                  finally
                  {
                      this._TaskEnd?.Invoke();
                  }

              });
        }

        public TaskResult<Result> GetResult ()
        {
            return this._TaskState.GetResult();
        }

        public void CheckIsOverTime (int time)
        {
            if (this._OverTime <= time)
            {
                this._TaskState.ExecFail("task.exec.overtime");
                if (this._RunTask.Status != TaskStatus.Running)
                {
                    this._RunTask.Dispose();
                }
            }
        }
    }
}
