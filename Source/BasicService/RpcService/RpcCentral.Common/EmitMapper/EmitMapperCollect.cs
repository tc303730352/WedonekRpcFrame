using System.Dynamic;
using System.Net;
using System.Text;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using WeDonekRpc.Helper;

namespace RpcCentral.Common.EmitMapper
{
    public class EmitMapperCollect
    {
        private static readonly Dictionary<string, IMappingConfigurator> _Config = new Dictionary<string, IMappingConfigurator>();
        private static readonly IMappingConfigurator _DefConfig;

        static EmitMapperCollect()
        {
            _DefConfig=DefaultMapConfig.Instance.ConvertUsing<DateTime, long>(a =>
            {
                if (a == DateTime.MinValue)
                {
                    return 0;
                }
                return a.ToLong();
            }).ConvertUsing<DateTime?, long>(a =>
                       {
                           if (!a.HasValue)
                           {
                               return 0;
                           }
                           return a.Value.ToLong();
                       })
                        .ConvertUsing<DateTime?, long?>(a =>
                        {
                            if (!a.HasValue)
                            {
                                return null;
                            }
                            return a.Value.ToLong();
                        })
                       .ConvertUsing<long, DateTime?>(a =>
                       {
                           if (a == 0)
                           {
                               return null;
                           }
                           return a.ToDateTime();
                       }).ConvertUsing<long?, DateTime?>(a =>
                       {
                           if (a.HasValue)
                           {
                               return a.Value == 0 ? null : a.Value.ToDateTime();
                           }
                           return null;
                       })
                       .ConvertUsing<long, DateTime>(a =>
                       {
                           if (a == 0)
                           {
                               return DateTime.MinValue;
                           }
                           return a.ToDateTime();
                       })
                       .ConvertUsing<string, Guid>(a =>
                       {
                           if (string.IsNullOrEmpty(a))
                           {
                               return Guid.Empty;
                           }
                           return new Guid(a);
                       })
                       .ConvertUsing<string, Uri>(a =>
                       {
                           if (string.IsNullOrEmpty(a))
                           {
                               return null;
                           }
                           return new Uri(a);
                       })
                       .ConvertUsing<string, byte[]>(a =>
                       {
                           if (string.IsNullOrEmpty(a))
                           {
                               return null;
                           }
                           return Encoding.UTF8.GetBytes(a);
                       })
                       .ConvertUsing<byte[], string>(a =>
                       {
                           if (a != null)
                           {
                               return string.Empty;
                           }
                           return Encoding.UTF8.GetString(a);
                       })
                       .ConvertUsing<string, string[]>(a =>
                       {
                           if (a.IsNull())
                           {
                               return null;
                           }
                           else if (a[0] == '[' && a[^1] == ']')
                           {
                               return a.Json<string[]>();
                           }
                           else if (a.IndexOf(",") != -1)
                           {
                               return a.Split(',');
                           }
                           return new string[] { a };
                       })
                       .ConvertUsing<int, int?>(a => a)
                        .ConvertUsing<bool, bool?>(a => a)
                        .ConvertUsing<long, long?>(a => a)
                        .ConvertUsing<short, short?>(a => a)
                       .ConvertUsing<decimal?, decimal>(a => a.GetValueOrDefault())
                       .ConvertUsing<long?, long>(a => a.GetValueOrDefault())
                       .ConvertUsing<int?, int>(a => a.GetValueOrDefault())
                       .ConvertUsing<short?, short>(a => a.GetValueOrDefault())
                       .ConvertUsing<bool?, bool>(a => a.GetValueOrDefault())
                       .ConvertUsing<IPAddress, long>(a => a == null ? 0 : a.Address)
                       .ConvertUsing<IPAddress, long?>(a => a?.Address)
                       .ConvertUsing<IPAddress, string>(a => a?.ToString())
                       .ConvertUsing<long, IPAddress>(a => a != 0 ? new IPAddress(a) : null)
                       .ConvertUsing<long?, IPAddress>(a => a.HasValue ? new IPAddress(a.Value) : null)
                       .ConvertUsing<string, IPAddress>(a => a.IsNull() ? null : IPAddress.Parse(a))
                       .ConvertUsing<string, ExpandoObject>(a => a != string.Empty ? a.Json<ExpandoObject>() : null);
        }
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
