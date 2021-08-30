using System;
using System.Collections.Generic;

using EmitMapper;
using EmitMapper.MappingConfiguration;

using RpcHelper;

namespace ERpcService.EmitMapper
{
        public class EmitMapperCollect
        {
                private static readonly Dictionary<string, IMappingConfigurator> _Config = new Dictionary<string, IMappingConfigurator>();
                private static readonly IMappingConfigurator _DefConfig = new DefaultMapConfig();

                public static void RegConfig<T>(IMappingConfigurator config)
                {
                        _Config.AddOrSet(typeof(T).FullName, config);
                }

                private static IMappingConfigurator _GetConfig(Type type)
                {
                        if (_Config.TryGetValue(type.FullName, out IMappingConfigurator config))
                        {
                                return config;
                        }
                        return _DefConfig;
                }
                private static ObjectsMapper<T, Result> _GetMapper<T, Result>()
                {
                        return ObjectMapperManager.DefaultInstance.GetMapper<T, Result>(_GetConfig(typeof(Result)));
                }

                public static Result Map<T, Result>(T mode) where T : class
                {
                        if (mode == null)
                        {
                                return default;
                        }
                        return _GetMapper<T, Result>().Map(mode);
                }
                public static Result Map<T, Result>(T mode, Result result) where T : class
                {
                        return _GetMapper<T, Result>().Map(mode);
                }
                internal static Result[] Map<T, Result>(T[] mode, Result[] result) where T : class
                {
                        ObjectsMapper<T, Result> mapper = _GetMapper<T, Result>();
                        for (int i = 0; i < result.Length; i++)
                        {
                                result[i] = mapper.Map(mode[i], result[i]);
                        }
                        return result;
                }
                public static Result[] Map<T, Result>(T[] mode) where T : class
                {
                        return _GetMapper<T[], Result[]>().Map(mode);
                }


        }
}
