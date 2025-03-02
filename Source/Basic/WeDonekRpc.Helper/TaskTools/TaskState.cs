using System;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Helper.TaskTools
{
    public class TaskState<T> : ITaskState<T>
    {
        private Guid _TaskId { get; }
        public bool IsEnd => this._IsEnd;

        public int EndTime { get; private set; }

        private volatile bool _IsEnd = false;
        private volatile bool _IsError = false;

        private string _Error = null;

        private object _State = null;
        private T _Result = default;
        public TaskState ( Guid taskId )
        {
            this._TaskId = taskId;
        }
        public void Complate ( T result )
        {
            if ( this._IsEnd )
            {
                return;
            }
            this.EndTime = HeartbeatTimeHelper.HeartbeatTime;
            this._Result = result;
            this._IsEnd = true;
        }
        public void SetState ( object state )
        {
            this._State = state;
        }
        public void ExecFail ( string error )
        {
            if ( this._IsEnd )
            {
                return;
            }
            this._Error = error;
            this._IsError = true;
            this.EndTime = HeartbeatTimeHelper.HeartbeatTime;
            this._IsEnd = true;
            new LogInfo(error, LogGrade.ERROR, "LocalTask") { LogTitle = "本地任务" }.Save();
        }

        public TaskResult<T> GetResult ()
        {
            return new TaskResult<T>
            {
                State = this._State,
                Error = this._Error,
                IsEnd = this._IsEnd,
                IsError = this._IsError,
                Result = this._Result
            };
        }
    }
}
