using System.Transactions;

using RpcHelper;

using RpcModel;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;
namespace Wedonek.RpcStore.Service.Collect
{
        internal class ServerGroupCollect : BasicCollect<ServerGroupDAL>, IServerGroupCollect
        {
                private IServerCollect _Server => this.GetCollect<IServerCollect>();
                private IServerTypeCollect _ServerType => this.GetCollect<IServerTypeCollect>();
                public bool CheckIsRepeat(string typeVal)
                {
                        if (this.BasicDAL.CheckIsRepeat(typeVal, out bool isExists))
                        {
                                return isExists;
                        }
                        throw new ErrorException("rpc.server.group.check.error");
                }
                public long AddGroup(ServerGroupDatum add)
                {
                        if (this.CheckIsRepeat(add.TypeVal))
                        {
                                throw new ErrorException("rpc.server.group.type.repeat");
                        }
                        else if (this.BasicDAL.AddGroup(add, out long id))
                        {
                                return id;
                        }
                        throw new ErrorException("rpc.server.group.add.error");
                }

                public void SetGroup(long id, string name)
                {
                        if (!this.BasicDAL.SetGroupName(id, name))
                        {
                                throw new ErrorException("rpc.server.group.set.error");
                        }
                }
                public ServerGroup[] QueryGroup(string name, IBasicPage paging, out long count)
                {
                        if (this.BasicDAL.QueryGroup(name, paging, out ServerGroup[] groups, out count))
                        {
                                return groups;
                        }
                        throw new ErrorException("rpc.server.group.query.error");
                }
                public ServerGroup[] GetGroups()
                {
                        if (this.BasicDAL.GetGroups(out ServerGroup[] groups))
                        {
                                return groups;
                        }
                        throw new ErrorException("rpc.server.group.get.error");
                }
                public ServerGroup GetGroup(long id)
                {
                        if (!this.BasicDAL.GetGroup(id, out ServerGroup group))
                        {
                                throw new ErrorException("rpc.server.group.get.error");
                        }
                        else if (group == null)
                        {
                                throw new ErrorException("rpc.server.group.not.find");
                        }
                        return group;
                }

                public void DropGroup(long groupId)
                {
                        if (this._Server.CheckIsExistsByGroup(groupId))
                        {
                                throw new ErrorException("rpc.server.group.noallow.drop");
                        }
                        using (TransactionScope tran = new TransactionScope())
                        {
                                this._ServerType.Clear(groupId);
                                if (!this.BasicDAL.DropGroup(groupId))
                                {
                                        throw new ErrorException("rpc.server.group.drop.error");
                                }
                                tran.Complete();
                        }
                }

                public ServerGroup[] GetGroup(long[] ids)
                {
                        if (this.BasicDAL.GetServiceGroup(ids, out ServerGroup[] group))
                        {
                                return group;
                        }
                        throw new ErrorException("rpc.server.group.get.error");
                }
        }
}
