using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("DictItem")]
    public class DictItemModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string DictCode
        {
            get;
            set;
        }
        [SugarColumn(IsPrimaryKey = true)]
        public string ItemCode
        {
            get;
            set;
        }

        public string PrtItemCode
        {
            get;
            set;
        }
        public string ItemText
        {
            get;
            set;
        }

    }
}
