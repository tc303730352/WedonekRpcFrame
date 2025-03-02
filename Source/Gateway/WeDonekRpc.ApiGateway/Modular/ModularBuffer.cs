using WeDonekRpc.ApiGateway.Interface;

namespace WeDonekRpc.ApiGateway.Modular
{
    internal class ModularBuffer
    {
        private IGatewayOption _Option;

        public ModularBuffer(IGatewayOption option)
        {
            _Option = option;
        }

        internal void Reg(IModular modular)
        {
            modular = ModularService.AddModular(modular);
            modular.Load(_Option);
        }
        internal void Init()
        {
            ModularService.ModularInit();
        }
    }
}
