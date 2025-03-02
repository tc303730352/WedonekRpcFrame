using System.Collections.Generic;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.HttpService.Config
{
    internal class DefHttpFileConfig : HttpFileConfig
    {
        private readonly ICrossConfig _Cross;
        internal DefHttpFileConfig (ICrossConfig cross)
        {
            this._Cross = cross;
            IConfigSection section = LocalConfig.Local.GetSection("gateway:http:file");
            section.AddRefreshEvent(this._Refresh);
        }
        private bool _IsInit = false;
        private void _Refresh (IConfigSection section, string name)
        {
            if (this._IsInit)
            {
                return;
            }
            this._IsInit = true;
            base.TempDirPath = section.GetValue("TempDirPath", "TempFile");
            base.DirConfig = section.GetValue<FileDirConfig[]>("DirConfig");
            base.ContentType = section.GetValue<Dictionary<string, string>>("ContentType");
            base.Init(this._Cross);
        }
    }
}
