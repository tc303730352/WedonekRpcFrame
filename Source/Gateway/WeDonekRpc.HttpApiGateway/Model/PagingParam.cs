using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace WeDonekRpc.HttpApiGateway.Model
{
    /// <summary>
    /// 分页参数
    /// </summary>
    /// <typeparam name="T">参数</typeparam>
    public class PagingParam<T> : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("public.query.param.null")]
        public T Query
        {
            get;
            set;
        }
        public IBasicPage ToBasicPaging ()
        {
            return this;
        }
    }
}
