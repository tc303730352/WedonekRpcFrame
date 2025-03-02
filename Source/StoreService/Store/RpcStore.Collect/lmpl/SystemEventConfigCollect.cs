using RpcStore.DAL;
using RpcStore.RemoteModel.SysEventConfig.Model;

namespace RpcStore.Collect.lmpl
{
    internal class SystemEventConfigCollect : ISystemEventConfigCollect
    {
        private readonly ISystemEventConfigDAL _BasicDAL;

        public SystemEventConfigCollect (ISystemEventConfigDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public SystemEventConfig Get (int id)
        {
            return this._BasicDAL.Get(id);
        }

        public string GetName (int id)
        {
            return this._BasicDAL.GetName(id);
        }

        public Dictionary<int, string> GetName (int[] ids)
        {
            return this._BasicDAL.GetName(ids);
        }

        public SystemEventItem[] Gets ()
        {
            return this._BasicDAL.Gets();
        }
    }
}
