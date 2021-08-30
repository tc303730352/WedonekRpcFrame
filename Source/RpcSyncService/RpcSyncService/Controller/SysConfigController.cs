using System;
using System.Linq;
using System.Text;

using RpcModel;

using RpcSyncService.Collect;
using RpcSyncService.Logic;
using RpcSyncService.Model;
using RpcSyncService.Model.DAL_Model;

using RpcHelper;
namespace RpcSyncService.Controller
{
        internal class SysConfigController : DataSyncClass
        {
                public SysConfigController(string type)
                {
                        this._SystemTypeVal = type;
                }

                public long SystemTypeId
                {
                        get;
                        private set;
                }

                private readonly string _SystemTypeVal = null;

                private SysConfigInfo[] _ConfigList = null;

                private DateTime _ToUpdateTime = DateTime.MinValue;
                public string ConfigMd5
                {
                        get;
                        private set;
                }

                protected override bool SyncData()
                {
                        if (!RemoteServerTypeCollect.GetSystemType(this._SystemTypeVal, out SystemTypeController systemType))
                        {
                                this.Error = systemType.Error;
                                return false;
                        }
                        else if (!new DAL.SysConfigDAL().GetSysConfig(systemType.Id, out SysConfigModel[] configs))
                        {
                                this.Error = "rpc.load.sys.config.error";
                                return false;
                        }
                        else
                        {
                                this.SystemTypeId = systemType.Id;
                                if (!configs.IsNull())
                                {
                                        this._ConfigList = configs.OrderByDescending(a => a.ToUpdateTime).Select(a => new SysConfigInfo(a)).Distinct().ToArray();
                                        this._ToUpdateTime = configs.Max(a => a.ToUpdateTime);
                                        this.ConfigMd5 = this._GetMD5(configs);
                                }
                                else
                                {
                                        this._ConfigList = new SysConfigInfo[0];
                                        this._ToUpdateTime = Tools.SqlMinTime;
                                        this.ConfigMd5 = string.Empty;
                                }
                                return true;
                        }
                }
                private string _GetMD5(SysConfigModel[] configs)
                {
                        StringBuilder str = new StringBuilder(configs.Length * 30 + 10);
                        configs.ForEach(a =>
                        {
                                str.Append(a.Name);
                                str.Append(",");
                        });
                        str.Append(this._ToUpdateTime.ToLong());
                        return str.ToString().GetMd5();
                }

                private void _Refresh()
                {
                        BroadcastMsg msg = new BroadcastMsg
                        {
                                IsExclude = true,
                                IsCrossGroup = true,
                                IsLimitOnly = false,
                                MsgKey = "Rpc_RefreshConfig",
                                MsgBody = new SysConfigRefresh { ConfigMd5 = this.ConfigMd5 }.ToJson(),
                                TypeVal = new string[]
                                {
                                        this._SystemTypeVal
                                }
                        };
                        BroadcastLogic.SendBroadcast(msg, RpcClient.RpcClient.CurrentSource);
                }
                public override void ResetLock()
                {
                        if (!this.IsInit)
                        {
                                return;
                        }
                        else if (!new DAL.SysConfigDAL().GetToUpdateTime(this.SystemTypeId, out DateTime time))
                        {
                                return;
                        }
                        else if (time > this._ToUpdateTime)
                        {
                                base.ResetLock();
                                if (base.Init())
                                {
                                        this._Refresh();
                                }
                        }
                }

                internal SysConfigData[] GetSysConfig(MsgSource source)
                {
                        SysConfigInfo[] configs = this._ConfigList.FindAll(a => a.CheckIsMate(source));
                        if (configs.Length > 0)
                        {
                                return configs.OrderByDescending(a => a.Weight).Select(a => new SysConfigData
                                {
                                        Name = a.Config.Name,
                                        IsJson = a.Config.ValueType,
                                        Value = a.Config.Value
                                }).Distinct().ToArray();
                        }
                        return null;
                }

                public string GetConfigVal(string name, MsgSource source)
                {
                        SysConfigInfo[] configs = this._ConfigList.FindAll(a => a.CheckIsMate(name, source));
                        if (configs.Length > 0)
                        {
                                return configs.OrderByDescending(a => a.Weight).Select(a => a.Config.Value).FirstOrDefault();
                        }
                        return null;
                }
        }
}
