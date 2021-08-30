using System.Collections.Generic;

using EmitMapper;
using EmitMapper.MappingConfiguration;

using RpcClient.Interface;
using RpcClient.Mapper;
namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class MapperCollect : IMapperCollect
        {
                private readonly IMapperConfig _DefConfig = null;
                public MapperCollect()
                {
                        this._DefConfig = new MapperConfig(DefaultMapConfig.Instance);
                }

                public IMapperConfig Config => this._DefConfig;

                public To Mapper<From, To>(From form) where From : class
                {
                        ObjectsMapper<From, To> mapper = ObjectMapperManager.DefaultInstance.GetMapper<From, To>();
                        return mapper.Map(form);
                }
                public To Mapper<From, To>(From form, To to) where From : class
                {
                        ObjectsMapper<From, To> mapper = ObjectMapperManager.DefaultInstance.GetMapper<From, To>();
                        return mapper.Map(form, to);
                }
                public To[] Mapper<From, To>(From[] form) where From : class
                {
                        ObjectsMapper<From[], To[]> mapper = ObjectMapperManager.DefaultInstance.GetMapper<From[], To[]>();
                        return mapper.Map(form);
                }

                public List<To> Mapper<From, To>(List<From> form) where From : class
                {
                        ObjectsMapper<List<From>, List<To>> mapper = ObjectMapperManager.DefaultInstance.GetMapper<List<From>, List<To>>();
                        return mapper.Map(form);
                }

                public To Mapper<From, To>(From form, IMapperConfig config) where From : class
                {
                        ObjectsMapper<From, To> mapper = ObjectMapperManager.DefaultInstance.GetMapper<From, To>((IMappingConfigurator)config);
                        return mapper.Map(form);
                }
                public To[] Mapper<From, To>(From[] form, IMapperConfig config) where From : class
                {
                        ObjectsMapper<From[], To[]> mapper = ObjectMapperManager.DefaultInstance.GetMapper<From[], To[]>((IMappingConfigurator)config);
                        return mapper.Map(form);
                }

                public List<To> Mapper<From, To>(List<From> form, IMapperConfig config) where From : class
                {
                        ObjectsMapper<List<From>, List<To>> mapper = ObjectMapperManager.DefaultInstance.GetMapper<List<From>, List<To>>((IMappingConfigurator)config);
                        return mapper.Map(form);
                }
        }
}
