namespace RpcStore.RemoteModel.ResourceModular.Model
{
    public class ResourceModularDatum
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 系统类别名
        /// </summary>
        public string SystemTypeName { get; set; }

        public long RpcMerId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModularName { get; set; }
        /// <summary>
        /// 备注
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
