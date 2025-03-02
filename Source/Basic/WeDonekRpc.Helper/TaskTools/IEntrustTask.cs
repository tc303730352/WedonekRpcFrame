using System;

namespace WeDonekRpc.Helper.TaskTools
{
    public interface ITaskState<Result>
    {
        bool IsEnd { get; }
        int EndTime { get; }
        void SetState (object state);
        void Complate (Result result);

        void ExecFail (string error);

        TaskResult<Result> GetResult ();
    }
    public interface IEntrustTask
    {
        Guid TaskId { get; }
        void ExecTask ();
        bool IsEnd { get; }
        int EndTime { get; }
        void CheckIsOverTime (int time);
    }
    public interface IEntrustTask<Result> : IEntrustTask
    {
        TaskResult<Result> GetResult ();
    }

}