using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface IIdgeneratorDAL
    {
        void Add (IdgeneratorModel add);
        int GetMaxWorkId ();
        int GetWorkId (string mac, int index, long serverTypeId);
    }
}