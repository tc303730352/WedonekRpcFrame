using RpcStore.Collect.LocalEvent.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.Model.TransmitScheme;
using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerTransmitSchemeCollect : IServerTransmitSchemeCollect
    {
        private readonly IServerTransmitSchemeDAL _Scheme;

        public ServerTransmitSchemeCollect (IServerTransmitSchemeDAL scheme)
        {
            this._Scheme = scheme;
        }

        public long Add (TransmitSchemeAdd scheme)
        {
            this._Scheme.CheckIsRepeat(scheme.RpcMerId, scheme);
            return this._Scheme.Add(scheme);
        }


        public void Delete (ServerTransmitSchemeModel scheme)
        {
            if (scheme.IsEnable)
            {
                throw new ErrorException("rpc.store.transmit.already.enable");
            }
            this._Scheme.Delete(scheme.Id);
            new DeleteTransmitScheme
            {
                Scheme = scheme
            }.AsyncPublic();
        }
        public ServerTransmitScheme GetScheme (long id)
        {
            return this._Scheme.GetScheme(id);
        }
        public ServerTransmitSchemeModel Get (long id)
        {
            return this._Scheme.Get(id);
        }

        public ServerTransmitSchemeModel[] Query (TransmitSchemeQuery query, IBasicPage paging, out int count)
        {
            return this._Scheme.Query(query, paging, out count);
        }

        public bool SetIsEnable (ServerTransmitSchemeModel scheme, bool isEnable)
        {
            if (scheme.IsEnable == isEnable)
            {
                return false;
            }
            else if (isEnable)
            {
                if (this._Scheme.CheckIsNull(scheme.Id))
                {
                    throw new ErrorException("rpc.store.transmit.null");
                }
                this._Scheme.CheckIsRepeatEnable(scheme);
            }
            this._Scheme.SetIsEnable(scheme.Id, isEnable);
            return true;
        }

        public void SetScheme (ServerTransmitSchemeModel scheme, TransmitSchemeSet set)
        {
            if (set.IsEquals(scheme))
            {
                return;
            }
            else if (scheme.SystemTypeId != set.SystemTypeId || scheme.Scheme != set.Scheme)
            {
                this._Scheme.CheckIsRepeat(scheme.RpcMerId, set);
            }
            this._Scheme.SetScheme(scheme, set);
        }

        public void SyncItem (long schemeId, TransmitDatum[] transmits)
        {
            this._Scheme.SyncItem(schemeId, transmits);
        }
    }
}
