using System.Data;

using RpcService.Model;

using SqlExecHelper;
namespace RpcService.DAL
{
        internal class RemoteServerTypeDAL : SqlBasicClass
        {
                public RemoteServerTypeDAL() : base("RemoteServerType")
                {

                }
                public bool GetSystemType(string typeVal, out SystemTypeDatum type)
                {
                        return this.GetRow(out type, new SqlWhere("TypeVal", SqlDbType.VarChar, 50)
                        {
                                Value = typeVal
                        });
                }
        }
}
