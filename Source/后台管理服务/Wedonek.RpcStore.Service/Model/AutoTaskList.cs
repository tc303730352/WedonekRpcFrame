namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 任务
        /// </summary>
        public class AutoTaskList : AutoTask
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
                /// 版本号
                /// </summary>
                public int VerNum { get; set; }
        }
}
