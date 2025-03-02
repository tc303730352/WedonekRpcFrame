using System;
using Wedonek.Gateway.Modular.Oom;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Mapper;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway;

namespace Wedonek.Gateway.Modular.Controller
{
    /// <summary>
    /// OOM实体映射使用演示
    /// </summary>
    internal class OomController : ApiController
    {
        private readonly IMapperCollect _Mapper;

        public OomController (IMapperCollect mapper)
        {
            this._Mapper = mapper;
        }

        /// <summary>
        /// 基本使用
        /// </summary>
        public OomDTO BasicMap ()
        {
            OomDB db = new OomDB
            {
                AddTime = DateTime.Now.ToLong(),
                Age = 18,
                Birthday = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "哈哈",
                Sex = 1,
                Tags = "[\"游戏\",\"游泳\"]"
            };
            return db.ConvertMap<OomDB, OomDTO>();
        }
        /// <summary>
        /// 覆盖实体中的值
        /// </summary>
        /// <returns></returns>
        public OomDTO IntoMap ()
        {
            OomDB db = new OomDB
            {
                Age = 18,
                Birthday = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "哈哈",
                Sex = 2,
                Tags = "[\"游戏\"]"
            };
            OomDTO dto = new OomDTO
            {
                AddTime = DateTime.Now,
                Age = 19,
                Tags = new string[] { "游戏", "游泳", "竞技" },
                Sex = UserSex.男
            };
            return dto.ConvertInto<OomDB, OomDTO>(db);
        }
        /// <summary>
        /// 基于转换方案
        /// </summary>
        /// <returns></returns>
        public OomDTO SchemeMap ()
        {
            OomSchemeDto db = new OomSchemeDto
            {
                AddTime = DateTime.Now.ToLong(),
                Age = "18岁",
                Birthday = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "哈哈",
                Sex = 1,
                Tags = "[\"游戏\",\"游泳\"]"
            };
            //使用现有指定方案转换
            OomDTO dto = db.ConvertMap<OomSchemeDto, OomDTO>("SchemeDemo");
            Console.WriteLine(dto.ToJson());
            //获取转换方案
            IMapperHandler scheme = this._Mapper.GetMapper("SchemeDemo");
            dto = scheme.Mapper<OomSchemeDto, OomDTO>(db);
            Console.WriteLine(dto.ToJson());
            return dto;
        }
        /// <summary>
        /// 基于配置的转换
        /// </summary>
        /// <returns></returns>
        public OomDTO ConfigMap ()
        {
            OomDB db = new OomDB
            {
                Age = 18,
                Birthday = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "哈哈",
                Sex = 2,
                Tags = "[\"游戏\"]",
                AddTime = DateTime.Now.ToLong()
            };
            MapperConfig config = new MapperConfig(true);
            return db.ConvertMap<OomDB, OomDTO>(config);
        }
    }
}
