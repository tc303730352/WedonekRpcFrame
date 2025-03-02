using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.Model.SysConfig;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.DAL
{
    public interface ISysConfigDAL
    {
        void Add (SysConfigModel add);
        bool CheckIsExists (SysConfigModel config);
        void Clear (long serverId);
        void Delete (long id);
        SysConfigModel FindConfig (string systemType, string name);

        BasicSysConfig FindBasicConfig (long rpcMerId, string name);
        SysConfigModel Get (long Id);
        SysConfigBasic[] Query (QuerySysParam query, IBasicPage paging, out int count);
        void Set (long id, SysConfigSetParam config);
        void SetIsEnable (long id, bool isEnable);
    }
}