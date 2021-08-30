using RpcModel;

using RpcService.Model;

using RpcHelper;
namespace RpcService.Controller
{
        public class SystemTypeController : DataSyncClass
        {
                public SystemTypeController(string val)
                {
                        this.TypeVal = val;
                }
                public long Id { get; private set; }

                public long GroupId { get; private set; }
                public string TypeVal { get; }
                /// <summary>
                /// 负载方式
                /// </summary>
                public BalancedType BalancedType { get; private set; }

                protected override bool SyncData()
                {
                        if (!new DAL.RemoteServerTypeDAL().GetSystemType(this.TypeVal, out SystemTypeDatum datum))
                        {
                                this.Error = "rpc.system.type.get.error";
                                return false;
                        }
                        else if (datum == null)
                        {
                                this.Error = "rpc.system.type.not.find";
                                return false;
                        }
                        else
                        {
                                this.BalancedType = datum.BalancedType;
                                this.GroupId = datum.GroupId;
                                this.Id = datum.Id;
                                return true;
                        }
                }
        }
}
