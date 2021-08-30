using RpcModel.Server;

namespace RpcService.Model.DAL_Model
{
        internal class ServiceState : RunState
        {
                public long ServerId
                {
                        get;
                        set;
                }
                public override bool Equals(object obj)
                {
                        if (obj is ServiceState i)
                        {
                                return i.ServerId == this.ServerId;
                        }
                        return false;
                }
                public override int GetHashCode()
                {
                        return this.ServerId.GetHashCode();
                }
        }
}
