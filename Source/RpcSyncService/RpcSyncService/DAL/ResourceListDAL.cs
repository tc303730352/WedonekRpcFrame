using System;

using RpcModularModel.Resource.Model;

using RpcSyncService.Model;

using SqlExecHelper;

using RpcHelper;

namespace RpcSyncService.DAL
{
        internal class ResourceListDAL : SqlBasicClass
        {
                public ResourceListDAL() : base("ResourceList", "RpcExtendService")
                {

                }
                public void ClearResource()
                {
                        if (this.Drop(new ISqlWhere[] {
                                new SqlWhere("ResourceState", System.Data.SqlDbType.SmallInt){Value=2},
                                new SqlWhere("LastTime", System.Data.SqlDbType.Date,QueryType.小等){Value=HeartbeatTimeHelper.CurrentDate.AddDays(-10)}
                        }) == -2)
                        {
                                throw new ErrorException("resource.clear.error");
                        }
                }
                public InvalidResource[] GetInvalidResource()
                {
                        if (this.Group(new string[] { "ModularId", "VerNum" }, out InvalidResource[] invalid, new ISqlWhere[] {
                                new SqlWhere("ResourceState", System.Data.SqlDbType.SmallInt){Value=1}
                        }))
                        {
                                return invalid;
                        }
                        throw new ErrorException("resource.invalid.get.error");
                }
                public void Set(InvalidResource[] invalids)
                {
                        using (IBatchUpdate table = this.BatchUpdate(invalids.Length, 5))
                        {
                                table.Column = new SqlTableColumn[]
                                {
                                         new SqlTableColumn("ModularId", System.Data.SqlDbType.UniqueIdentifier){ ColType= SqlColType.查询},
                                        new SqlTableColumn("VerNum", System.Data.SqlDbType.VarChar,25){ ColType= SqlColType.查询},
                                         new SqlTableColumn("ResourceVer", System.Data.SqlDbType.Int){ ColType= SqlColType.查询},
                                         new SqlTableColumn("ResourceState", System.Data.SqlDbType.SmallInt){ColType= SqlColType.修改},
                                         new SqlTableColumn("LastTime", System.Data.SqlDbType.Date){ColType= SqlColType.修改}
                                };
                                invalids.ForEach(a =>
                                {
                                        table.AddRow(a.ModularId, a.VerNum, a.MinVer, 2, HeartbeatTimeHelper.CurrentDate);
                                });
                                if (!table.Update())
                                {
                                        throw new ErrorException("resource.set.error");
                                }
                        }
                }
                public void Sync(Guid modularId, string ver, ResourceDatum[] lists)
                {
                        using (IBatchMerge table = this.BatchMerge(lists.Length, 8))
                        {
                                table.Column = new SqlTableColumn[]
                                {
                                        new SqlTableColumn("Id", System.Data.SqlDbType.UniqueIdentifier){ColType= SqlColType.添加},
                                        new SqlTableColumn("ModularId", System.Data.SqlDbType.UniqueIdentifier){ColType= SqlColType.添加| SqlColType.查询},
                                        new SqlTableColumn("ResourcePath", System.Data.SqlDbType.VarChar,100){ColType= SqlColType.添加| SqlColType.查询},
                                        new SqlTableColumn("FullPath", System.Data.SqlDbType.VarChar,300){ColType= SqlColType.添加| SqlColType.修改},
                                        new SqlTableColumn("ResourceShow", System.Data.SqlDbType.NVarChar,100){ColType= SqlColType.添加 | SqlColType.修改},
                                        new SqlTableColumn("ResourceState", System.Data.SqlDbType.SmallInt){ColType= SqlColType.修改| SqlColType.添加},
                                        new SqlTableColumn("VerNum", System.Data.SqlDbType.VarChar,25){ColType= SqlColType.修改| SqlColType.添加},
                                        new SqlTableColumn("ResourceVer", System.Data.SqlDbType.Int){ColType= SqlColType.修改,SetType= SqlSetType.递加}
                                };
                                lists.ForEach(a =>
                                {
                                        table.AddRow(Tools.NewGuid(), modularId, a.ResourcePath, a.FullPath, a.ResourceShow, 1, ver, 1);
                                });
                                if (!table.InsertOrUpdate())
                                {
                                        throw new ErrorException("resource.sync.error");
                                }
                        }
                }
        }
}
