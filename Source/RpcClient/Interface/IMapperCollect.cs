using System.Collections.Generic;

namespace RpcClient.Interface
{
        public interface IMapperCollect
        {
                IMapperConfig Config { get; }

                To Mapper<From, To>(From form) where From : class;

                To Mapper<From, To>(From form, To to) where From : class;
                To[] Mapper<From, To>(From[] form) where From : class;
                List<To> Mapper<From, To>(List<From> form) where From : class;

                To Mapper<From, To>(From form, IMapperConfig config) where From : class;
                To[] Mapper<From, To>(From[] form, IMapperConfig config) where From : class;
                List<To> Mapper<From, To>(List<From> form, IMapperConfig config) where From : class;
        }
}