using RpcModel;

using RpcSyncService.Model.DAL_Model;

namespace RpcSyncService.Model
{
        [System.Serializable]
        internal class SysConfigInfo
        {
                public SysConfigInfo()
                {
                }
                public SysConfigInfo(SysConfigModel config)
                {
                        this.Config = config;
                        if (config.RpcMerId != 0 || config.ServerId != 0)
                        {
                                this._IsPublic = true;
                                if (config.RpcMerId != 0 && config.ServerId != 0)
                                {
                                        this.Weight = 3;
                                }
                                else if (config.ServerId != 0)
                                {
                                        this.Weight = 2;
                                }
                                else
                                {
                                        this.Weight = 1;
                                }
                        }
                        this.Key = string.Join("_", config.ServerId, config.RpcMerId, config.Name.Trim().ToLower());
                }

                public string Key
                {
                        get;
                }
                private readonly bool _IsPublic = false;
                public int Weight
                {
                        get;
                }
                public SysConfigModel Config
                {
                        get;
                }
                public bool CheckIsMate(MsgSource source)
                {
                        if (this._IsPublic)
                        {
                                return true;
                        }
                        else if (this.Config.RpcMerId != 0 && this.Config.RpcMerId != source.RpcMerId)
                        {
                                return false;
                        }
                        else if (this.Config.ServerId != 0 && this.Config.ServerId != source.SourceServerId)
                        {
                                return false;
                        }
                        return true;
                }
                public bool CheckIsMate(string name, MsgSource source)
                {
                        if (name != this.Config.Name)
                        {
                                return false;
                        }
                        return this.CheckIsMate(source);
                }
                public override bool Equals(object obj)
                {
                        if (obj is SysConfigInfo config)
                        {
                                return this.Key == config.Key;
                        }
                        return false;
                }
                public override int GetHashCode()
                {
                        return this.Key.GetHashCode();
                }
        }
}
