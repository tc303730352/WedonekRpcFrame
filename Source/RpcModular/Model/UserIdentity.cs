using System;

using RpcModularModel.Identity;
using RpcModularModel.Identity.Model;

using RpcHelper;

namespace RpcModular.Model
{
        internal class UserIdentity : DataSyncClass
        {
                internal UserIdentity(string identityId)
                {
                        this.IdentityId = identityId;
                }
                public string IdentityId
                {
                        get;
                        private set;
                }
                /// <summary>
                /// 应用名称
                /// </summary>
                public string AppName
                {
                        get;
                        private set;
                }
                public bool IsValid
                {
                        get;
                        private set;
                }
                private bool _IsAccredit = false;

                private string[] _LocalPath = null;

                private bool _IsCheckRoute = false;

                private string[] _RouteList = null;



                protected override bool SyncData()
                {
                        IdentityDatum datum = new GetIdentity
                        {
                                IdentityId = new Guid(this.IdentityId)
                        }.Send();
                        this.AppName = datum.AppName;
                        this._LocalPath = datum.Path;
                        this._RouteList = datum.Routes;
                        this.IsValid = datum.IsValid;
                        if (datum.Path.IsNull())
                        {
                                this._IsAccredit = true;
                        }
                        this._IsCheckRoute = !datum.Routes.IsNull();
                        return true;
                }

                internal bool CheckRoute(string msgKey)
                {
                        if (!this._IsCheckRoute)
                        {
                                return true;
                        }
                        else
                        {
                                return this._RouteList.IsExists(msgKey);
                        }
                }

                public bool CheckGateway(string path)
                {
                        if (this._IsAccredit)
                        {
                                return true;
                        }
                        else
                        {
                                return this._LocalPath.StartsWith(path);
                        }
                }
             
        }
}
