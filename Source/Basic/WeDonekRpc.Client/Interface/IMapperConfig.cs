using System;
using EmitMapper.MappingConfiguration;

namespace WeDonekRpc.Client.Interface
{
    public interface IMapperConfig
    {
        IMapperConfig ConvertUsing<From, To>(Func<From, To> func);
        IMapperConfig IgnoreMembers<From, To>(params string[] pros);
        IMapperConfig MatchMembers(Func<string, string, bool> matcher);
        IMapperConfig NullSubstitution<From, To>(Func<object, To> func);
        IMapperConfig PostProcess<T>(Func<T, object, T> processor);
    }
    public interface ITempMapperConfig : IMapperConfig, IDisposable
    {

    }
    internal interface IEmitConfig : IMapperConfig
    {
        int VerNum { get; }
        string ConfigId { get; }
        DefaultMapConfig Config { get; }
    }
}