using System.Collections.Generic;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// OOM实体映射
    /// </summary>
    public interface IMapperCollect : IMapperHandler
    {
        /// <summary>
        /// 获取方案的转换器
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        IMapperHandler GetMapper (string scheme);

        To Mapper<From, To> (From form, To to, IMapperConfig config) where From : class;

        To Mapper<From, To> (From form, IMapperConfig config) where From : class;
        To[] Mapper<From, To> (From[] form, IMapperConfig config) where From : class;
        List<To> Mapper<From, To> (List<From> form, IMapperConfig config) where From : class;
    }
}