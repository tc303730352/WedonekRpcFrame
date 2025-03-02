using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway
{
    public static class LingHelper
    {
        public static void CheckAccredit (this IApiService service, ApiAccreditSet accredit)
        {
            if (!accredit.IsAccredit)
            {
                return;
            }
            else if (service.UserState == null)
            {
                throw new ErrorException("accredit.unauthorized");
            }
            else if (!accredit.Prower.IsNull() && !service.UserState.CheckPrower(accredit.Prower))
            {
                throw new ErrorException("accredit.no.prower");
            }
        }
        public static void CheckAccredit (this IApiService service)
        {
            if (service.UserState == null)
            {
                throw new ErrorException("accredit.unauthorized");
            }
        }
        public static void CheckAccredit (this IApiService service, string[] prowers)
        {
            if (service.UserState == null)
            {
                throw new ErrorException("accredit.unauthorized");
            }
            else if (!prowers.IsNull() && !service.UserState.CheckPrower(prowers))
            {
                throw new ErrorException("accredit.no.prower");
            }
        }
    }
}
