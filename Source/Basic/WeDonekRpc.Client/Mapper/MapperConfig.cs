using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Numerics;
using System.Text;

using EmitMapper;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations;

using WeDonekRpc.Client.Interface;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Mapper
{
    [Attr.IgnoreIoc]
    public class MapperConfig : IEmitConfig
    {
        private volatile int _VerNum = 0;
        private string _ConfigId = null;
        public string ConfigId
        {
            get
            {
                if (this._ConfigId == null)
                {
                    this._ConfigId = Tools.NewGuid().ToString("N");
                }
                return this._ConfigId;
            }
        }
        public int VerNum => this._VerNum;

        public DefaultMapConfig Config { get; }

        internal MapperConfig (DefaultMapConfig config)
        {
            this.Config = config;
            this._Init();
        }
        public MapperConfig (bool isLoadDef = true)
        {
            this.Config = new DefaultMapConfig();
            if (isLoadDef)
            {
                this._Init();
            }
        }

        private void _Init ()
        {
            _ = this.Config.ConvertUsing<DateTime, long>(a =>
                       {
                           if (a == DateTime.MinValue)
                           {
                               return 0;
                           }
                           return a.ToLong();
                       })
                       .ConvertUsing<DateTime?, long>(a =>
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
                        .ConvertUsing<string, BigInteger>(a =>
                        {
                            if (a.IsNull())
                            {
                                return BigInteger.Zero;
                            }
                            return BigInteger.Parse(a);
                        })
                        .ConvertUsing<string, BigInteger?>(a =>
                        {
                            if (a.IsNull())
                            {
                                return null;
                            }
                            return BigInteger.Parse(a);
                        })
                        .ConvertUsing<BigInteger, string>(a =>
                        {
                            return a.ToString();
                        })
                       .ConvertUsing<string, DateTime>(a =>
                       {
                           if (a.IsNull())
                           {
                               return DateTime.MinValue;
                           }
                           return DateTime.Parse(a);
                       })
                       .ConvertUsing<string, DateTime?>(a =>
                       {
                           if (a.IsNull())
                           {
                               return null;
                           }
                           return DateTime.Parse(a);
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
                          else if (a[0] == '|' && a[^1] == '|')
                          {
                              return a.Remove(a.Length - 1, 1).Remove(0, 1).Split('|');
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
                       .ConvertUsing<string, ExpandoObject>(a => a != string.Empty ? a.Json<ExpandoObject>() : null)
                       .ConvertUsing<Dictionary<string, string>, string>(a => a.ToJson())
                       .ConvertUsing<Dictionary<string, object>, string>(a => a.ToJson())
                       .ConvertUsing<string, Dictionary<string, string>>(a => a.Json<Dictionary<string, string>>())
                       .ConvertUsing<string, Dictionary<string, object>>(a => a.Json<Dictionary<string, object>>());
        }
        private void _Update ()
        {
            this._VerNum += 1;
        }
        public IMapperConfig ConvertUsing<From, To> (Func<From, To> func)
        {
            this._Update();
            _ = this.Config.ConvertUsing(func);
            return this;
        }
        public IMapperConfig NullSubstitution<From, To> (Func<object, To> func)
        {
            this._Update();
            _ = this.Config.NullSubstitution<From, To>(func);
            return this;
        }
        public IMapperConfig MatchMembers (Func<string, string, bool> matcher)
        {
            this._Update();
            _ = this.Config.MatchMembers(matcher);
            return this;
        }
        public IMapperConfig IgnoreMembers<From, To> (params string[] pros)
        {
            if (!pros.IsNull())
            {
                this._Update();
                _ = this.Config.IgnoreMembers<From, To>(pros);
            }
            return this;
        }

        public IMapperConfig PostProcess<T> (Func<T, object, T> processor)
        {
            this._Update();
            _ = this.Config.PostProcess<T>((a, b) => processor(a, b));
            return this;
        }

        public IMappingOperation[] GetMappingOperations (Type from, Type to)
        {
            return this.Config.GetMappingOperations(from, to);
        }

        public IRootMappingOperation GetRootMappingOperation (Type from, Type to)
        {
            return this.Config.GetRootMappingOperation(from, to);
        }

        public string GetConfigurationName ()
        {
            return this.Config.GetConfigurationName();
        }

        public StaticConvertersManager GetStaticConvertersManager ()
        {
            return this.Config.GetStaticConvertersManager();
        }
    }
}
