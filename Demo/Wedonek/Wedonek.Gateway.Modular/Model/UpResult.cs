using System;

namespace Wedonek.Gateway.Modular.Model
{
    /// <summary>
    /// 上传结果
    /// </summary>
    public class UpResult
    {
        public Uri FileUri
        {
            get;
            set;
        }

        public long FileSize
        {
            get;
            set;
        }
    }
}
