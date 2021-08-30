using RpcHelper;
namespace RpcSyncService.Controller
{
        public class SystemTypeController : DataSyncClass
        {
                public SystemTypeController(string val)
                {
                        this.TypeVal = val;
                }

                public string TypeVal { get; }
                public long Id { get; private set; }

                protected override bool SyncData()
                {
                        if (!new DAL.RemoteServerTypeDAL().GetSystemTypeId(this.TypeVal, out long id))
                        {
                                this.Error = "rpc.sync.system.type.get.error";
                                return false;
                        }
                        else if (id == 0)
                        {
                                this.Error = "rpc.sync.system.type.not.find";
                                return false;
                        }
                        else
                        {
                                this.Id = id;
                                return true;
                        }
                }
        }
}
