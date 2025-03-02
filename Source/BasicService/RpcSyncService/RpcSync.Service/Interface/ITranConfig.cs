namespace RpcSync.Service.Interface
{
    public interface ITranConfig
    {
        short TranCommitRetryNum { get;  }
        short TranRollbackRetryNum { get;  }
    }
}