using SqlSugar;

namespace RpcStore.Model.DB
{
    /// <summary>
    /// 服务组
    /// </summary>
    [SugarTable("ServerGroup")]
    public class ServerGroupModel
    {
        /// <summary>
        /// 服务组ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 组类别
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 类别名
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }
    }
}
