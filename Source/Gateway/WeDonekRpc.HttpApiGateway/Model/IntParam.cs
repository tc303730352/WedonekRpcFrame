using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.HttpApiGateway.Model
{
    public class IntParam<T>
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [NumValidate("public.data.id.null", 1, int.MaxValue)]
        public int Id { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [NullValidate("public.param.null")]
        public T Param { get; set; }
    }
}
