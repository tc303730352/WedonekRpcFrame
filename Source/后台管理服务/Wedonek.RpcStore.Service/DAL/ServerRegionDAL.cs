using SqlExecHelper;
using SqlExecHelper.SetColumn;
using SqlExecHelper.SqlValue;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        public class ServerRegionDAL : SqlBasicClass
        {
                public ServerRegionDAL() : base("ServerRegion")
                {

                }
                public bool CheckName(string name, out bool isExists)
                {
                        return this.CheckIsExists(out isExists, new ISqlWhere[] {
                                new SqlWhere("RegionName", System.Data.SqlDbType.NVarChar,50){Value=name},
                                new SqlWhere("IsDrop", System.Data.SqlDbType.Bit){Value=0}
                        });
                }

                public bool GetServerRegion(out ServerRegion[] list)
                {
                        return this.Get("IsDrop", 0, out list);
                }

                public bool AddRegion(string name, out int id)
                {
                        return this.Insert(new IInsertSqlValue[] {
                                new InsertSqlValue("RegionName", System.Data.SqlDbType.NVarChar,50){ Value=name}
                        }, out id);
                }
                public bool SetRegion(int id, string name)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("RegionName",  System.Data.SqlDbType.NVarChar,50){Value=name}
                        }, "Id", id);
                }
                public bool DropRegion(int id)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("IsDrop",  System.Data.SqlDbType.Bit){Value=1}
                        }, "Id", id);
                }
        }
}
