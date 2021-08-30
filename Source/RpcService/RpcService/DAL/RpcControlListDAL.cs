using RpcModel.Model;

using SqlExecHelper;

namespace RpcService.DAL
{
        internal class RpcControlListDAL : SqlBasicClass
        {
                public RpcControlListDAL() : base("RpcControlList")
                {

                }
                public bool GetControlServer(out RpcControlServer[] servers)
                {
                        return this.Get(out servers, new SqlWhere("IsDrop", System.Data.SqlDbType.Bit) { Value = 0 });
                }
        }
}
