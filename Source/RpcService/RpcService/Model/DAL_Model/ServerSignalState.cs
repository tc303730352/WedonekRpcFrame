using RpcModel;

namespace RpcService.Model.DAL_Model
{
        public class ServerSignalState
        {
                public long ServerId
                {
                        get;
                        set;
                }
                public long RemoteId
                {
                        get;
                        set;
                }
                public int ConNum
                {
                        get;
                        set;
                }

                public int AvgTime
                {
                        get;
                        set;
                }
                public int SendNum
                {
                        get;
                        set;
                }
                public UsableState UsableState { get; set; }
                public int ErrorNum { get; internal set; }

                public override bool Equals(object obj)
                {
                        if (obj is ServerSignalState i)
                        {
                                return i.ServerId == this.ServerId && i.RemoteId == this.RemoteId;
                        }
                        return false;
                }
                public override int GetHashCode()
                {
                        return string.Concat(this.ServerId, "_", this.RemoteId).GetHashCode();
                }
        }
}
