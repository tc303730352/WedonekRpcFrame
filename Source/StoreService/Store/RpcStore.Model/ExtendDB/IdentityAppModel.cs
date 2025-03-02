using SqlSugar;

namespace RpcStore.Model.ExtendDB
{
    [SugarTable("IdentityApp")]
    public class IdentityAppModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public string AppId { get; set; }
        public string AppName { get; set; }

        public string AppShow { get; set; }

        /// <summary>
        /// ”¶”√¿©’π
        /// </summary>
        [SugarColumn(IsJson = true)]
        public Dictionary<string, string> AppExtend { get; set; }


        public DateTime? EffectiveDate { get; set; }

        public bool IsEnable { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
