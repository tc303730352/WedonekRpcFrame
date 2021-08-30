using RpcModel;

using RpcHelper;

namespace RpcClient
{
        /// <summary>
        /// 分页实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class BasicPage<T> : RpcRemoteByPaging<T>, IBasicPage<T>
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
                public string SortName { get; set; }
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

                public IBasicPage ToBasicPage()
                {
                        return new BasicPage
                        {
                                Index = Index,
                                NextId = NextId,
                                Size = Size
                        };
                }
        }
}
