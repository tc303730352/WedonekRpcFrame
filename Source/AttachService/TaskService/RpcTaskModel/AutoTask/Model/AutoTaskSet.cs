using WeDonekRpc.Helper.Validate;

namespace RpcTaskModel.AutoTask.Model
{
    public class AutoTaskSet
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
        /// 任务名
        /// </summary>
        [NullValidate("task.name.null")]
        [LenValidate("task.name.len", 2, 50)]
        public string TaskName
        {
            get;
            set;
        }
        /// <summary>
        /// 任务说明
        /// </summary>
        [LenValidate("task.show.len", 0, 255)]
        public string TaskShow
        {
            get;
            set;
        }
        /// <summary>
        /// 任务优先级
        /// </summary>
        [NumValidate("task.priority.error", 0, int.MaxValue)]
        public int TaskPriority
        {
            get;
            set;
        }
        /// <summary>
        /// 任务开始的步骤
        /// </summary>
        [NumValidate("task.begin.error", 0, short.MaxValue)]
        public short BeginStep
        {
            get;
            set;
        }
        /// <summary>
        /// 任务失败时通知的Emall列表
        /// </summary>
        [FormatValidate("task.fail.email.error", ValidateFormat.Email)]
        public string[] FailEmall
        {
            get;
            set;
        }
    }
}
