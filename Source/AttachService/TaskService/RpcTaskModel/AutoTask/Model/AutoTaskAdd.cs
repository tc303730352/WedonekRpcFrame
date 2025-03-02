using WeDonekRpc.Helper.Validate;

namespace RpcTaskModel.AutoTask.Model
{
    public class AutoTaskAdd
    {
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public int? RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 所属集群
        /// </summary>
        [NumValidate("rpc.task.mer.id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 任务名
        /// </summary>
        [NullValidate("rpc.task.name.null")]
        [LenValidate("rpc.task.name.len", 2, 50)]
        public string TaskName
        {
            get;
            set;
        }
        /// <summary>
        /// 任务说明
        /// </summary>
        [LenValidate("rpc.task.show.len", 0, 255)]
        public string TaskShow
        {
            get;
            set;
        }
        /// <summary>
        /// 任务优先级
        /// </summary>
        [NumValidate("rpc.task.priority.error", 0, int.MaxValue)]
        public int TaskPriority
        {
            get;
            set;
        }
        /// <summary>
        /// 任务开始的步骤
        /// </summary>
        [NumValidate("rpc.task.begin.error", 0, short.MaxValue)]
        public short BeginStep
        {
            get;
            set;
        }
        /// <summary>
        /// 任务失败时通知的Emall列表
        /// </summary>
        [FormatValidate("rpc.task.fail.email.error", ValidateFormat.Email)]
        public string[] FailEmall
        {
            get;
            set;
        }
    }
}
