namespace RpcSync.DAL
{
    public interface IErrorMsgDAL
    {
        string GetErrorMsg ( long errorId, string lang );
    }
}