using System;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;

namespace WeDonekRpc.Client.Mapper
{
    public class MapperBuffer
    {
        private readonly IocBuffer _Ioc;

        internal MapperBuffer (IocBuffer ioc)
        {
            this._Ioc = ioc;
        }

        public void Add (Type type)
        {
            if (type.GetInterface(ConfigDic.IMapper.FullName) != null)
            {
                _ = this._Ioc.Register(ConfigDic.IMapper, type);
            }
        }
        public void Add<T> () where T : ISchemeMapper
        {
            _ = this._Ioc.Register(ConfigDic.IMapper, typeof(T));
        }
    }
}
