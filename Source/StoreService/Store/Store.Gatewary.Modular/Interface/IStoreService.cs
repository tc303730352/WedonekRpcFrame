using Store.Gatewary.Modular.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IStoreService
    {
        LoginResult Login (StoreLogin login);

        void LoginOut (string accreditId);
    }
}