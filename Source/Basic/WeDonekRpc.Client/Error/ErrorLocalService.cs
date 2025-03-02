using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Error;

namespace WeDonekRpc.Client.Error
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class ErrorLocalService : IErrorLocalService
    {
        public long GetErrorId(string code)
        {
            return LocalErrorManage.GetErrorId(code);
        }

        public string GetText(string code, string lang, string def)
        {
            return LocalErrorManage.GetText(code, lang, def);
        }
        public string GetText(string code, string def)
        {
            return LocalErrorManage.GetText(code, def);
        }
        public string GetCode(long errorId)
        {
            return LocalErrorManage.GetCode(errorId);
        }
    }
}
