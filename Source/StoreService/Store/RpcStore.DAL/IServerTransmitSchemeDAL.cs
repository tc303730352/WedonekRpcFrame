using RpcStore.Model.DB;
using RpcStore.Model.TransmitScheme;
using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL
{
    public interface IServerTransmitSchemeDAL
    {
        void SyncItem (long schemeId, TransmitDatum[] transmits);
        void CheckIsRepeat (long rpcMerId, TransmitSchemeSet add);
        long Add (TransmitSchemeAdd scheme);
        void Adds (TransmitAdd[] data);
        void Delete (long schemeId);
        ServerTransmitSchemeModel Get (long id);
        ServerTransmitScheme GetScheme (long id);

        ServerTransmitSchemeModel[] Query (TransmitSchemeQuery query, IBasicPage paging, out int count);

        void SetScheme (ServerTransmitSchemeModel scheme, TransmitSchemeSet set);

        void SetIsEnable (long id, bool isEnable);

        void Clear (long rpcMerId);
        bool CheckIsNull (long schemeId);
        void CheckIsRepeatEnable (ServerTransmitSchemeModel scheme);
    }
}