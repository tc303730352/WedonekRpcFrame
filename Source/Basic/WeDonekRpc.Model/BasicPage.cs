using WeDonekRpc.Helper;

namespace WeDonekRpc.Model
{
    /// <summary>
    /// 分页查询
    /// </summary>
    public class BasicPage : IBasicPage
    {
        /// <summary>
        ///页码
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 每页最后一条记录唯一建(缓存用)
        /// </summary>
        public object NextId { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortName { get; set; }
        /// <summary>
        /// 是否倒序
        /// </summary>
        public bool IsDesc { get; set; }
        private string _OrderBy;

        public string OrderBy
        {
            get => this._OrderBy;
        }


        public void InitOrderBy (string def, bool isDesc)
        {
            if (this.SortName.IsNull())
            {
                this.SortName = def;
                this.IsDesc = isDesc;
                this._OrderBy = isDesc ? def + " desc" : def;
            }
            else
            {
                this._OrderBy = this.IsDesc ? this.SortName + " desc" : this.SortName;
            }
        }
    }
}
