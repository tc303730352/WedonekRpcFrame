using RpcSync.DAL;
using RpcSync.Model;

namespace RpcSync.Collect.Collect
{
    internal class SystemEventLogCollect : ISystemEventLogCollect
    {
        private ISystemEventLogDAL _LogDAL;

        public SystemEventLogCollect(ISystemEventLogDAL logDAL)
        {
            _LogDAL = logDAL;
        }

        public void Adds(SysEventAddLog[] logs)
        {
            _LogDAL.Adds(logs);
        }
    }
}
