using System.Collections.Generic;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;
namespace RpcSyncService.Controller
{
        internal class RpcMerController : DataSyncClass
        {
                public RpcMerController(long merId)
                {
                        this.MerId = merId;
                }

                public long MerId { get; }


                private MerServer[] _ServerList = null;

                protected override bool SyncData()
                {
                        if (!RemoteServerGroupCollect.GetAllServer(this.MerId, out this._ServerList, out string error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else
                        {
                                return true;
                        }
                }

                public string[] GetDictate(List<RootNode> nodes, int regionId)
                {
                        if (this._ServerList.Length == 0)
                        {
                                return null;
                        }
                        else if (regionId == 0)
                        {
                                return this._ServerList.Convert(a => nodes.Exists(b => b.Id == a.SystemType), a => a.TypeVal);
                        }
                        else
                        {
                                return this._ServerList.Convert(a => a.RegionId == regionId && nodes.Exists(b => b.Id == a.SystemType), a => a.TypeVal);
                        }
                }
                public long[] GetServer(List<RootNode> nodes, int regionId)
                {
                        if (this._ServerList.Length == 0)
                        {
                                return null;
                        }
                        else if (regionId == 0)
                        {
                                return this._ServerList.Convert(a => nodes.Exists(b => b.Id == a.SystemType), a => a.ServerId);
                        }
                        else
                        {
                                return this._ServerList.Convert(a => a.RegionId == regionId && nodes.Exists(b => b.Id == a.SystemType), a => a.ServerId);
                        }
                }
                internal string[] GetAllDictate(int regionId)
                {
                        if (regionId == 0)
                        {
                                return this._ServerList.ConvertAll(a => a.TypeVal);
                        }
                        else
                        {
                                return this._ServerList.Convert(a => a.RegionId == regionId, a => a.TypeVal);
                        }
                }

                internal long[] GetAllServer(int regionId)
                {
                        if (regionId == 0)
                        {
                                return this._ServerList.ConvertAll(a => a.ServerId);
                        }
                        else
                        {
                                return this._ServerList.Convert(a => a.RegionId == regionId, a => a.ServerId);
                        }
                }


        }
}
