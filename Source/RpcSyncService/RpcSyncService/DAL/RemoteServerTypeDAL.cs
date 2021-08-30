using System.Data;

using RpcSyncService.Model;

using SqlExecHelper;
namespace RpcSyncService.DAL
{
        internal class RemoteServerTypeDAL : SqlBasicClass
        {
                public RemoteServerTypeDAL() : base("RemoteServerType")
                {

                }

                public bool GetSystemTypeId(string sysType, out long id)
                {
                        return this.ExecuteScalar("Id", out id, new SqlWhere("TypeVal", SqlDbType.VarChar, 50) { Value = sysType });
                }
                public bool GetSystemType(out SystemType[] sysType)
                {
                        return this.Get(out sysType);
                }
        }
}
