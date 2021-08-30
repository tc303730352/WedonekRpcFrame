using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        public class QueryTaskParam
        {
                public string TaskName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务类型
                /// </summary>
                [EnumValidate("rpc.taskType.error", typeof(TaskType))]
                public TaskType? TaskType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 集群Id
                /// </summary>
                public long? RpcMerId
                {
                        get;
                        set;
                }
        }
}
