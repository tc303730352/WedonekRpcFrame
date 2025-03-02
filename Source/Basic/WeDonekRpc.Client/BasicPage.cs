using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client
{
    /// <summary>
    /// 分页实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicPage<T> : RpcRemote<PagingResult<T>>, IBasicPage
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
        public T[] Send (out int count)
        {
            PagingResult<T> result = base.Send();
            count = result.Count;
            return result.List;
        }
        public IBasicPage ToBasicPage ()
        {
            return new BasicPage
            {
                Index = this.Index,
                NextId = this.NextId,
                Size = this.Size,
                SortName = this.SortName,
                IsDesc = this.IsDesc,
            };
        }
    }

    public class BasicPage<T, Extend> : RpcRemote<PagingResult<T, Extend>>, IBasicPage
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

        public IBasicPage ToBasicPage ()
        {
            return new BasicPage
            {
                Index = this.Index,
                NextId = this.NextId,
                Size = this.Size
            };
        }
    }
}
