namespace WeDonekRpc.Helper.TaskTools
{
    public class TaskResult<T>
    {
        public T Result
        {
            get;
            set;
        }
        public bool IsEnd { get; set; }

        public bool IsError { get; set; }

        public string Error { get; set; }
        public object State { get; set; }
    }
}
