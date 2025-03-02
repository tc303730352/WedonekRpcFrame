namespace RpcStore.RemoteModel.ResourceModular.Model
{
    /// <summary>
    /// 资源模块
    /// </summary>
    public class BasicModular
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 资源模块名
        /// </summary>
        public string ModularName { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public ResourceType ResourceType { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
