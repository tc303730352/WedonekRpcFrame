using SqlSugar;

namespace RpcExtend.Model
{
    public class IdentityApp
    {
        public long Id
        {
            get;
            set;
        }
        public string AppName
        {
            get;
            set;
        }
        /// <summary>
        /// 应用扩展
        /// </summary>
        [SugarColumn(IsJson = true)]
        public Dictionary<string, string> AppExtend { get; set; }

        public DateTime? EffectiveDate
        {
            get;
            set;
        }
        public bool IsEnable
        {
            get;
            set;
        }
    }
}
