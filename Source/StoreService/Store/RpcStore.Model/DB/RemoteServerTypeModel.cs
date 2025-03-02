using WeDonekRpc.Model;
using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("RemoteServerType")]
    public class RemoteServerTypeModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 类型值
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string SystemName
        {
            get;
            set;
        }
        /// <summary>
        /// 默认端口
        /// </summary>
        public int DefPort
        {
            get;
            set;
        }

        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType
        {
            get;
            set;
        }

        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
