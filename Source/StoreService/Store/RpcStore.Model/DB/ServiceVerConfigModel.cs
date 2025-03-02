using SqlSugar;

namespace RpcStore.Model.DB
{
    /// <summary>
    /// 服务节点版本配置
    /// </summary>
    [SugarTable("ServiceVerConfig")]
    public class ServiceVerConfigModel
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 所属集群
        /// </summary>
        public long SchemeId
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号 版本格式 "001.01.01" 去掉 点
        /// </summary>
        public int VerNum { get; set; }
        /// <summary>
        /// 系统节点类型
        /// </summary>
        public long SystemTypeId { get; set; }

    }
}
