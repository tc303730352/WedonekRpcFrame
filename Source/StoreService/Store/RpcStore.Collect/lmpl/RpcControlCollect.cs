
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Control.Model;

namespace RpcStore.Collect.lmpl
{
    internal class RpcControlCollect : IRpcControlCollect
    {
        private readonly IRpcControlDAL _BasicDAL;
        public RpcControlCollect (IRpcControlDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public void CheckIsRepeat (string ip, int port)
        {
            if (this._BasicDAL.CheckIsRepeat(ip, port))
            {
                throw new ErrorException("rpc.store.control.repeat");
            }
        }
        public int Add (RpcControlDatum datum)
        {
            this.CheckIsRepeat(datum.ServerIp, datum.Port);
            RpcControlModel add = datum.ConvertMap<RpcControlDatum, RpcControlModel>();
            return this._BasicDAL.Add(add);
        }
        public RpcControlModel Get (int id)
        {
            RpcControlModel control = this._BasicDAL.Get(id);
            if (control == null)
            {
                throw new ErrorException("rpc.store.control.not.find");
            }
            return control;
        }

        public RpcControlModel[] Query (IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(paging, out count);
        }
        public bool Set (RpcControlModel control, RpcControlDatum set)
        {
            if (set.IsEquals(control))
            {
                return false;
            }
            else if (control.ServerIp != set.ServerIp || control.Port != control.Port)
            {
                this.CheckIsRepeat(set.ServerIp, set.Port);
            }
            this._BasicDAL.Set(control.Id, set);
            return true;
        }
        public void Delete (RpcControlModel control)
        {
            this._BasicDAL.Delete(control.Id);
        }
    }
}
