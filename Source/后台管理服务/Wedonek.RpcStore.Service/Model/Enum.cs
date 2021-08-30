namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 配置范围
        /// </summary>
        public enum ConfigRange
        {
                全局 = 0,
                集群 = 2,
                节点类别 = 4,
                节点 = 1
        }
        public enum SysConfigValueType
        {
                字符串 = 0,
                JSON = 1
        }
        public enum TaskSendType
        {
                指令 = 0,
                URI = 1,
                广播 = 2
        }
        public enum TaskType
        {
                定时任务 = 0,
                间隔任务 = 1,
                定时间隔任务 = 2
        }
}
