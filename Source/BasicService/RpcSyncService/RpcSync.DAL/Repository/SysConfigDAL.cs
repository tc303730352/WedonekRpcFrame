using RpcSync.Model;
using RpcSync.Model.DB;
using SqlSugar;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class SysConfigDAL : ISysConfigDAL
    {
        private readonly IRepository<SysConfigModel> _BasicDAL;
        public SysConfigDAL (IRepository<SysConfigModel> dal)
        {
            this._BasicDAL = dal;
        }

        public SysConfig[] GetSysConfig (string type)
        {
            string[] types = new string[] { type, string.Empty };
            return this._BasicDAL.Gets<SysConfig>(c => types.Contains(c.SystemType) && c.IsEnable);
        }

        public ConfigItemToUpdateTime[] GetToUpdateTime ()
        {
            return this._BasicDAL.GroupBy(a => a.IsEnable, a => new
            {
                a.RpcMerId,
                a.ServerId,
                a.RegionId,
                a.ContainerGroup,
                a.VerNum,
                a.SystemType
            }, a => new ConfigItemToUpdateTime
            {
                RpcMerId = a.RpcMerId,
                RegionId = a.RegionId,
                ServerId = a.ServerId,
                ContainerGroup = a.ContainerGroup,
                VerNum = a.VerNum,
                SystemType = a.SystemType,
                ToUpdateTime = SqlFunc.AggregateMax(a.ToUpdateTime)
            });
        }
    }
}
