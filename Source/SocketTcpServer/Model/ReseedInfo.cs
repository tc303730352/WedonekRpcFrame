using System;

namespace SocketTcpServer.Model
{
        [Serializable]
        internal class ReseedInfo
        {
                public ReseedInfo()
                {
                }

                public ReseedInfo(string md5, string path)
                {
                        this.FileMD5 = md5;
                        this.SaveFilePath = path;
                }
                /// <summary>
                /// 文件的MD5
                /// </summary>
                public string FileMD5
                {
                        get;
                        set;
                }

                /// <summary>
                /// 保存的文件信息路径
                /// </summary>
                public string SaveFilePath
                {
                        get;
                        set;
                }

                /// <summary>
                /// 是否正在续传
                /// </summary>
                public bool IsUse
                {
                        get;
                        set;
                }
        }
}
