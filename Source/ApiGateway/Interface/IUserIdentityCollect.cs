namespace ApiGateway.Interface
{
        public interface IUserIdentityCollect
        {
                bool IsEnableIdentity { get; }
                void InitIdentity(string identity, string path);
                void CheckIdentity(string identityId);
        }
}