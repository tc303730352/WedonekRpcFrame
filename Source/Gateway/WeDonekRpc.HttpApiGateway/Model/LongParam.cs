using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.HttpApiGateway.Model
{
    public class LongParam<T>
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [NumValidate("public.data.id.null", 1)]
        public long Id { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [NullValidate("public.data.value.null")]
        public T Value { get; set; }
    }
    public class LongNullParam<T>
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [NumValidate("public.data.id.null", 1)]
        public long Id { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public T Value { get; set; }
    }
}
