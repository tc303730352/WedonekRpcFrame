using System;

namespace WeDonekRpc.ExtendModel.RetryTask.Model
{
    public class RetryResult
    {
        public AutoRetryTaskStatus Status { get; set; }

        public string Error { get; set; }

        public DateTime? ComplateTime { get; set; }
    }
}
