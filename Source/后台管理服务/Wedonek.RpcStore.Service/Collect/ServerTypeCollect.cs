using RpcHelper;

using RpcModel;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class ServerTypeCollect : BasicCollect<RemoteServerTypeDAL>, IServerTypeCollect
        {

                public long AddServiceType(ServerTypeDatum add)
                {
                        if (this.CheckIsRepeat(add.TypeVal))
                        {
                                throw new ErrorException("rpc.server.group.type.repeat");
                        }
                        else if (this.BasicDAL.AddServiceType(add, out long id))
                        {
                                return id;
                        }
                        throw new ErrorException("rpc.server.type.add.error");
                }
                public bool CheckIsRepeat(string typeVal)
                {
                        if (this.BasicDAL.CheckIsRepeat(typeVal, out bool isExists))
                        {
                                return isExists;
                        }
                        throw new ErrorException("rpc.server.type.check.error");
                }

                public ServerType[] GetServiceTypes(long groupId)
                {
                        if (this.BasicDAL.GetServiceTypes(groupId, out ServerType[] types))
                        {
                                return types;
                        }
                        throw new ErrorException("rpc.server.type.get.error");
                }

                public void SetServiceType(long id, ServerTypeSetParam param)
                {
                        if (!this.BasicDAL.SetServiceType(id, param))
                        {
                                throw new ErrorException("rpc.server.type.set.error");
                        }
                }

                public void DropServiceType(long id)
                {
                        if (!this.BasicDAL.DropServiceType(id))
                        {
                                throw new ErrorException("rpc.server.type.drop.error");
                        }
                }

                public ServerType GetServiceType(long id)
                {
                        if (!this.BasicDAL.GetServiceType(id, out ServerType type))
                        {
                                throw new ErrorException("rpc.server.type.get.error");
                        }
                        else if (type == null)
                        {
                                throw new ErrorException("rpc.server.type.not.find");
                        }
                        return type;
                }

                public ServerType[] QuerySystemType(ServerTypeQueryParam query, IBasicPage paging, out long count)
                {
                        if (this.BasicDAL.QuerySystemType(query, paging, out ServerType[] datas, out count))
                        {
                                return datas;
                        }
                        throw new ErrorException("rpc.server.type.query.error");
                }

                public ServerType[] GetServiceTypes(long[] ids)
                {
                        if (ids.Length == 0)
                        {
                                return new ServerType[0];
                        }
                        else if (this.BasicDAL.GetServiceTypes(ids, out ServerType[] types))
                        {
                                return types;
                        }
                        throw new ErrorException("rpc.server.type.get.error");
                }

                public void Clear(long groupId)
                {
                        if (!this.BasicDAL.Clear(groupId))
                        {
                                throw new ErrorException("rpc.server.type.clear.error");
                        }
                }
        }
}
