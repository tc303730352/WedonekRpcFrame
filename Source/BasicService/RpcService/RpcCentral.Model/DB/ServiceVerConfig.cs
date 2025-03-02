using SqlSugar;

namespace RpcCentral.Model.DB
{
    [SugarTable("ServiceVerConfig")]
    public class ServiceVerConfig
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 方案ID
        /// </summary>
        public long SchemeId
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号 版本格式 "01.01.01" 去掉 点
        /// </summary>
        public int VerNum { get; set; }
        /// <summary>
        /// 系统节点类型
        /// </summary>
        public long SystemTypeId { get; set; }

    }
}
