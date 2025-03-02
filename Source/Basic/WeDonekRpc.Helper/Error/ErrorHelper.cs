using System;
namespace WeDonekRpc.Helper.Error
{
    internal static class ErrorHelper
    {

        internal static string FormatError (string error)
        {
            int i = error.Length - 1;
            if (error[i] != ']')
            {
                return error;
            }
            int b = error.LastIndexOf('[');
            return error.Substring(0, b);
        }
        internal static string FormatError (string error, out string param)
        {
            int i = error.Length - 1;
            if (error[i] != ']')
            {
                param = null;
                return error;
            }
            int b = error.LastIndexOf('[') + 1;
            param = error.Substring(b, i - b);
            return error.Substring(0, b - 1);
        }
        internal static string TryGetCode (this Func<IErrorEvent> action, long errorId)
        {
            if (action == null)
            {
                return null;
            }
            return action().FindErrorCode(errorId);
        }
        internal static long TryGetId (this Func<IErrorEvent> action, string code)
        {
            if (action == null)
            {
                return 0;
            }
            return action().FindErrorId(code);
        }
        internal static void TriggerDrop (this Func<IErrorEvent> action, ErrorMsg msg)
        {
            if (action == null)
            {
                return;
            }
            IErrorEvent ev = action();
            ev.DropError(msg);
        }
        internal static ErrorMsg GetError (this Func<IErrorEvent> action, string code, string lang)
        {
            if (action == null)
            {
                return null;
            }
            IErrorEvent ev = action();
            return ev.GetError(code, lang);
        }
    }
}
