using SqlSugar;

namespace RpcCentral.Model.DB
{
    [SugarTable("ContainerList")]
    public class Contrainer
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
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
