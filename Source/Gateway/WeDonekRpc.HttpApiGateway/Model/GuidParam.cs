using System;
using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.HttpApiGateway.Model
{
    public class GuidParam<T>
    {
        [NullValidate("public.data.Id.null")]
        public Guid Id { get; set; }
        [NullValidate("public.param.null")]
        public T Param { get; set; }
    }
    public class GuidNullParam<T>
    {
        [NullValidate("public.data.Id.null")]
        public Guid Id { get; set; }
        public T Param { get; set; }
    }
}
