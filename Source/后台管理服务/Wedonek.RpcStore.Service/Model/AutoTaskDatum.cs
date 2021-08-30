namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 任务基本资料
        /// </summary>
        public class AutoTaskDatum
        {
                /// <summary>
                /// 任务Id
                /// </summary>
                public long Id
                {
                        get;
                        set;
                }

                /// <summary>
                /// 
                /// </summary>
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务名
                /// </summary>
                public string TaskName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务类型
                /// </summary>
                public TaskType TaskType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务间隔
                /// </summary>
                public int TaskTimeSpan
                {
                        get;
                        set;
                }

                /// <summary>
                /// 任务排序
                /// </summary>
                public int TaskPriority
                {
                        get;
                        set;
                }
                /// <summary>
                /// 发送类型错误
                /// </summary>
                public TaskSendType SendType
                {
                        get;
                        set;
                }

                /// <summary>
                /// 版本号
                /// </summary>
                public int VerNum { get; set; }
        }
}
