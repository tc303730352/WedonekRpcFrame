namespace RpcClient.Interface
{
        public interface IServerLimit
        {
                bool IsUsable { get; }
                bool IsLimit();

                void Reset();
                void Refresh(int time);
        }
}
