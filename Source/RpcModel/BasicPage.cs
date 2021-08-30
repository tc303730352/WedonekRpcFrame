using RpcHelper;

namespace RpcModel
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

                public string OrderBy
                {
                        get
                        {
                                if (this.IsDesc)
                                {
                                        return string.Concat(this.SortName, " desc");
                                }
                                return this.SortName;
                        }
                }

                public string GetOrderBy(string def)
                {
                        if (this.SortName.IsNull())
                        {
                                this.SortName = def;
                        }
                        else if (def != this.SortName)
                        {
                                this.SortName = string.Concat(def, ",", this.SortName);
                        }
                        if (this.IsDesc)
                        {
                                return string.Concat(this.SortName, " desc");
                        }
                        return this.SortName;
                }

                public void InitOrderBy(string def, bool isDesc)
                {
                        if (this.SortName.IsNull())
                        {
                                this.SortName = def;
                                this.IsDesc = this.IsDesc;
                        }
                        else if (def != this.SortName)
                        {
                                this.SortName = string.Concat(def, ",", this.SortName);
                        }
                }
        }
}
