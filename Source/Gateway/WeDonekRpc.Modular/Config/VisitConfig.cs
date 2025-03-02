namespace WeDonekRpc.Modular.Config
{
    /// <summary>
    /// 服务节点访问统计配置
    /// </summary>
    internal class VisitConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        } = true;
        /// <summary>
        /// 间隔时间
        /// </summary>
        public int Interval
        {
            get;
            set;
        } = 10;
    }
}
