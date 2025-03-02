using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using EmitMapper;
using EmitMapper.Mappers;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Mapper
{
    /// <summary>
    /// DTO实体转换
    /// </summary>
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class MapperService : IMapperCollect, IMapperManage
    {
        /// <summary>
        /// 默认转换器
        /// </summary>
        private readonly IMapperHandler _DefMapper;
        /// <summary>
        /// 当前线程启用的转换器
        /// </summary>
        private static readonly AsyncLocal<IMapperHandler> _Mapper = new AsyncLocal<IMapperHandler>();
        /// <summary>
        /// 转换对象缓存 缓存EmitMapper 的 ObjectsMapperBaseImpl 对象 避免 每次转换都实例化
        /// </summary>
        private static readonly ConcurrentDictionary<string, ObjectsMapperBaseImpl> _Cache = new ConcurrentDictionary<string, ObjectsMapperBaseImpl>();
        /// <summary>
        /// 转换方案
        /// </summary>
        private static readonly ConcurrentDictionary<string, IMapperHandler> _Scheme = new ConcurrentDictionary<string, IMapperHandler>();

        public MapperService ()
        {
            this._DefMapper = new MapperHandler();
        }
        /// <summary>
        /// DTO转换筛选器
        /// </summary>
        private IMapperHandler _Local => _Mapper.Value == null ? this._DefMapper : _Mapper.Value;

        public IMapperConfig Config => this._DefMapper.Config;

        public IMapperHandler GetMapper (string scheme)
        {
            if (this._TryGetScheme(scheme, out IMapperHandler mapper))
            {
                return mapper;
            }
            throw new ErrorException("rpc.mapper.scheme.not.find")
            {
                Args = {
                        { "Scheme", scheme }
                }
            };
        }
        #region 实体转换器缓存

        private ObjectsMapperBaseImpl _Getimpl (Type form, Type to, IEmitConfig config)
        {
            string key = string.Join("_", form.FullName, to.FullName, config.ConfigId);
            if (_Cache.TryGetValue(key, out ObjectsMapperBaseImpl lmpl))
            {
                return lmpl;
            }
            lmpl = new ObjectMapperManager().GetMapperImpl(form, to, config.Config);
            _ = _Cache.TryAdd(key, lmpl);
            return lmpl;
        }
        #endregion

        public To Mapper<From, To> (From form) where From : class
        {
            return this._Local.Mapper<From, To>(form);
        }
        public To Mapper<From, To> (From form, To to) where From : class
        {
            return this._Local.Mapper(form, to);
        }
        public To[] Mapper<From, To> (From[] form) where From : class
        {
            return this._Local.Mapper<From, To>(form);
        }

        public List<To> Mapper<From, To> (List<From> form) where From : class
        {
            return this._Local.Mapper<From, To>(form);
        }

        public To Mapper<From, To> (From form, IMapperConfig config) where From : class
        {
            ObjectsMapperBaseImpl cache = this._Getimpl(typeof(From), typeof(To), (IEmitConfig)config);
            return (To)cache.Map(form);
        }
        public To[] Mapper<From, To> (From[] form, IMapperConfig config) where From : class
        {
            ObjectsMapperBaseImpl cache = this._Getimpl(typeof(From[]), typeof(To[]), (IEmitConfig)config);
            return (To[])cache.Map(form);
        }

        public List<To> Mapper<From, To> (List<From> form, IMapperConfig config) where From : class
        {
            ObjectsMapperBaseImpl cache = this._Getimpl(typeof(List<From>), typeof(List<To>), (IEmitConfig)config);
            return (List<To>)cache.Map(form);
        }

        public To Mapper<From, To> (From form, To to, IMapperConfig config) where From : class
        {
            ObjectsMapperBaseImpl cache = this._Getimpl(typeof(From), typeof(To), (IEmitConfig)config);
            return (To)cache.Map(form, to, null);
        }
        private bool _TryGetScheme (string schemeName, out IMapperHandler mapper)
        {
            if (_Scheme.TryGetValue(schemeName, out mapper))
            {
                return true;
            }
            ISchemeMapper scheme = RpcClient.Ioc.Resolve<ISchemeMapper>(schemeName);
            mapper = new MapperHandler(scheme);
            return _Scheme.TryAdd(schemeName, mapper);
        }

        public void SetMapper (IMapperHandler mapper)
        {
            _Mapper.Value = mapper;
        }

        public void Reset ()
        {
            _Mapper.Value = null;
        }
    }
}
