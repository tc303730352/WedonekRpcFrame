namespace RpcClient.Interface
{
        public interface IErrorCollect
        {
                void ResetError(long errorId, string code);
        }
}