using SqlSugar;

namespace RpcSync.Model.DB
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
        public int GroupId
        {
            get;
            set;
        }
        public string ContrainerId
        {
            get;
            set;
        }
        public string VirtualMac
        {
            get;
            set;
        }
        public string Remark
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
