namespace AutoTask.Model
{
    public class RemoteTask
    {
        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName
        {
            get;
            set;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 区域Id
        /// </summary>
        public int? RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 任务说明
        /// </summary>
        public string TaskShow
        {
            get;
            set;
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public int TaskPriority
        {
            get;
            set;
        }
        /// <summary>
        /// 任务排序
        /// </summary>
        public string[] FailEmall
        {
            get;
            set;
        }

        /// <summary>
        /// 开始项
        /// </summary>
        public short BeginStep { get; set; }
    }
}
