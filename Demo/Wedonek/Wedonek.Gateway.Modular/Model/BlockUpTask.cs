using System;

namespace Wedonek.Gateway.Modular.Model
{
    public class BlockUpTask
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 文件MD5
        /// </summary>
        public string FileMd5
        {
            get;
            set;
        }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime Complete
        {
            get;
            set;
        }
    }
}
