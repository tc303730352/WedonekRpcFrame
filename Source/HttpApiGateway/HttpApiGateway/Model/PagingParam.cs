using RpcModel;

using RpcHelper.Validate;

namespace HttpApiGateway.Model
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
                public T Param
                {
                        get;
                        set;
                }
                public IBasicPage ToBasicPaging()
                {
                        return new BasicPage
                        {
                                Size = this.Size,
                                NextId = this.NextId,
                                Index = this.Index,
                        };
                }
        }
}
