using System.Collections.Generic;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// DTO转换器
    /// </summary>
    [Attr.IgnoreIoc]
    public interface IMapperHandler
    {
        IMapperConfig Config { get; }
        To Mapper<From, To> (From form) where From : class;

        To Mapper<From, To> (From form, To to) where From : class;

        To[] Mapper<From, To> (From[] form) where From : class;
        List<To> Mapper<From, To> (List<From> form) where From : class;
    }
}
