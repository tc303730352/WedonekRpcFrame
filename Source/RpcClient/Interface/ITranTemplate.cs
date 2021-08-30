namespace RpcClient.Interface
{
        internal interface ITranTemplate
        {
                string TranName { get; }

                void Rollback(string submitJson, string extend);
        }
}