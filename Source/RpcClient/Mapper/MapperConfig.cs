using System;
using System.Dynamic;
using System.Text;

using EmitMapper;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations;

using RpcClient.Interface;

using RpcHelper;

namespace RpcClient.Mapper
{
        public class MapperConfig : IMapperConfig, IMappingConfigurator
        {
                private readonly DefaultMapConfig _DefConfig = null;
                internal MapperConfig(DefaultMapConfig config)
                {
                        this._DefConfig = config;
                        this._Init();
                }
                public MapperConfig()
                {
                        this._DefConfig = new DefaultMapConfig();
                        this._Init();
                }
                private void _Init()
                {
                        this._DefConfig.ConvertUsing<DateTime, long>(a =>
                                   {
                                           if (a == DateTime.MinValue)
                                           {
                                                   return 0;
                                           }
                                           return a.ToLong();
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
                                   }).ConvertUsing<string, ExpandoObject>(a => a != string.Empty ? a.Json<ExpandoObject>() : null);
                }
                public IMapperConfig ConvertUsing<From, To>(Func<From, To> func)
                {
                        this._DefConfig.ConvertUsing(func);
                        return this;
                }
                public IMapperConfig NullSubstitution<From, To>(Func<object, To> func)
                {
                        this._DefConfig.NullSubstitution<From, To>(func);
                        return this;
                }
                public IMapperConfig MatchMembers(Func<string, string, bool> matcher)
                {
                        this._DefConfig.MatchMembers(matcher);
                        return this;
                }
                public IMapperConfig IgnoreMembers<From, To>(params string[] pros)
                {
                        if (!pros.IsNull())
                        {
                                this._DefConfig.IgnoreMembers<From, To>(pros);
                        }
                        return this;
                }

                public IMapperConfig PostProcess<T>(Func<T, object, T> processor)
                {
                        this._DefConfig.PostProcess<T>((a, b) => processor(a, b));
                        return this;
                }

                public IMappingOperation[] GetMappingOperations(Type from, Type to)
                {
                        return this._DefConfig.GetMappingOperations(from, to);
                }

                public IRootMappingOperation GetRootMappingOperation(Type from, Type to)
                {
                        return this._DefConfig.GetRootMappingOperation(from, to);
                }

                public string GetConfigurationName()
                {
                        return this._DefConfig.GetConfigurationName();
                }

                public StaticConvertersManager GetStaticConvertersManager()
                {
                        return this._DefConfig.GetStaticConvertersManager();
                }
        }
}
