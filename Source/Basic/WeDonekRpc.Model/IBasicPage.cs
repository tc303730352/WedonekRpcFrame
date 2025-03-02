using WeDonekRpc.Helper.Validate;
namespace WeDonekRpc.Model
{
    public interface IBasicPage
    {
        /// <summary>
        /// 页码
        /// </summary>
        [NumValidate("public.paging.index", 1, int.MaxValue)]
        int Index
        {
            get;
            set;
        }
        /// <summary>
        /// 每页大小
        /// </summary>
        [NumValidate("public.paging.size", 4, 100)]
        int Size
        {
            get;
            set;
        }
        /// <summary>
        /// 上一页的最后一条数据ID(第一页为空）
        /// </summary>
        object NextId
        {
            get;
            set;
        }
        /// <summary>
        /// 排序列
        /// </summary>
        string SortName { get; set; }
        /// <summary>
        /// 是否降序
        /// </summary>
        bool IsDesc { set; get; }

        string OrderBy { get; }
        /// <summary>
        /// 排序默认字段
        /// </summary>
        void InitOrderBy (string def, bool isDesc);
    }
    public interface IBasicPage<T> : IBasicPage
    {
        IBasicPage ToBasicPage ();
    }
    public interface IBasicPage<T, Extend> : IBasicPage
    {
        IBasicPage ToBasicPage ();
    }
}
