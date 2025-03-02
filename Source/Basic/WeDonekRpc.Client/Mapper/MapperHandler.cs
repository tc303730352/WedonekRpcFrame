using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration;
using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.Client.Mapper
{
    internal class MapperHandler : IMapperHandler
    {
        private readonly IEmitConfig _Config;
        private readonly ConcurrentDictionary<string, ObjectsMapperBaseImpl> _Cache = new ConcurrentDictionary<string, ObjectsMapperBaseImpl>();

        public IMapperConfig Config => this._Config;

        public MapperHandler (ISchemeMapper mapper)
        {
            this._Config = new MapperConfig();
            mapper.InitConfig(this._Config);
        }
        public MapperHandler ()
        {
            this._Config = new MapperConfig(DefaultMapConfig.Instance);
        }
        private ObjectsMapperBaseImpl _Getimpl (Type form, Type to)
        {
            string key = string.Join("_", form.FullName, to.FullName, this._Config.VerNum);
            if (this._Cache.TryGetValue(key, out ObjectsMapperBaseImpl lmpl))
            {
                return lmpl;
            }
            lmpl = ObjectMapperManager.DefaultInstance.GetMapperImpl(form, to, this._Config.Config);
            _ = this._Cache.TryAdd(key, lmpl);
            return lmpl;
        }
        public To Mapper<From, To> (From form) where From : class
        {
            ObjectsMapperBaseImpl cache = this._Getimpl(typeof(From), typeof(To));
            return (To)cache.Map(form);
        }
        public To Mapper<From, To> (From form, To to) where From : class
        {
            ObjectsMapperBaseImpl cache = this._Getimpl(typeof(From), typeof(To));
            return (To)cache.Map(form, to, null);
        }
        public To[] Mapper<From, To> (From[] form) where From : class
        {
            ObjectsMapperBaseImpl cache = this._Getimpl(typeof(From[]), typeof(To[]));
            return (To[])cache.Map(form);
        }

        public List<To> Mapper<From, To> (List<From> form) where From : class
        {
            ObjectsMapperBaseImpl cache = this._Getimpl(typeof(List<From>), typeof(List<To>));
            return (List<To>)cache.Map(form);
        }

    }
}
