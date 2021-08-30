using RpcModel.Model;

using RpcService.Collect;

namespace RpcService.Logic
{
        internal class RpcControlLogic
        {
                internal static bool GetControls(out RpcControlServer[] result, out string error)
                {
                        return RpcControlCollect.GetControlServer(out result, out error);
                }
        }
}
