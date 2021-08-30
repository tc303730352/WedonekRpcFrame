
using RpcModular.Domain;

namespace RpcModular
{
        public interface IIdentityService
        {
                string IdentityId { get; set; }
                bool IsEnableIdentity
                {
                        get;
                }
                IdentityDomain GetIdentity();
                bool Check();
                void CheckState();
        }
}