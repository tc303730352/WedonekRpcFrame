using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("ContainerList")]
    public class ContainerListModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 容器组
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// 容器ID
        /// </summary>
        public string ContrainerId
        {
            get;
            set;
        }

        public string InternalIp
        {
            get;
            set;
        }
        public int InternalPort
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
