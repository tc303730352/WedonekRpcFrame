namespace RpcModel.Server
{
        public class ServiceHeartbeat
        {

                public long ServerId
                {
                        get;
                        set;
                }
                public RunState RunState
                {
                        get;
                        set;
                }
                public RemoteState[] RemoteState { get; set; }
        }
}
