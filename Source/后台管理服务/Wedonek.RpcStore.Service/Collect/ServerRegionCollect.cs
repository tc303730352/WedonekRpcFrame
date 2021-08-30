
using RpcHelper;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class ServerRegionCollect : BasicCollect<ServerRegionDAL>, IServerRegionCollect
        {
                public ServerRegion[] GetServerRegion()
                {
                        if (this.BasicDAL.GetServerRegion(out ServerRegion[] list))
                        {
                                return list;
                        }
                        throw new ErrorException("rpc.region.get.error");
                }
                public void SetRegion(int id, string name)
                {
                        if (!this.BasicDAL.CheckName(name, out bool isExists))
                        {
                                throw new ErrorException("rpc.region.name.check.error");
                        }
                        else if (isExists)
                        {
                                throw new ErrorException("rpc.region.name.repeat");
                        }
                        else if (!this.BasicDAL.SetRegion(id, name))
                        {
                                throw new ErrorException("rpc.region.set.error");
                        }
                }

                public int AddRegion(string name)
                {
                        if (!this.BasicDAL.CheckName(name, out bool isExists))
                        {
                                throw new ErrorException("rpc.region.name.check.error");
                        }
                        else if (isExists)
                        {
                                throw new ErrorException("rpc.region.name.repeat");
                        }
                        else if (this.BasicDAL.AddRegion(name, out int id))
                        {
                                return id;
                        }
                        throw new ErrorException("rpc.region.add.error");
                }
                public void DropRegion(int id)
                {
                        if (!this.BasicDAL.DropRegion(id))
                        {
                                throw new ErrorException("rpc.region.drop.error");
                        }
                }
        }
}
