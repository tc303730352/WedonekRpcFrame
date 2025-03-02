using DataBasicCli.RpcService;

namespace DataBasicCli.Model
{
    internal class RpcServiceTemplate
    {
        public ServerRegionModel[] Region { get; set; }
        public RpcControlModel[] Control { get; set; }
        public DictCollect[] Dict { get; set; }
        public DictItemModel[] DictItem { get; set; }
        public RpcMerModel[] RpcMer { get; set; }
        public RemoteServerConfigModel[] Servers { get; set; }
        public RemoteServerGroupModel[] RemoteServerGroup { get; set; }
        public RemoteServerTypeModel[] ServerType { get; set; }
        public ServerGroupModel[] ServerGroup { get; set; }
        public SysConfigModel[] SysConfig { get; set; }
        public ServerTransmitSchemeModel[] TransmitScheme { get; set; }
        public ErrorCollectModel[] Errors { get; set; }
        public ErrorLangMsgModel[] ErrorLang { get; set; }
    }
}
