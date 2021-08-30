using System;

using ApiGateway.Interface;
using ApiGateway.Model;

using RpcHelper;
namespace ApiGateway.Attr
{

        /// <summary>
        /// 上传配置
        /// </summary>
        [AttributeUsage(AttributeTargets.Method, Inherited = true)]
        public class ApiUpConfig : Attribute, IUpCheck
        {
                /// <summary>
                /// 最大大小
                /// </summary>
                private readonly int _MaxSize = 10 * 1024 * 1024;
                /// <summary>
                /// 允许的扩展
                /// </summary>
                private readonly string[] _Extension = null;
                /// <summary>
                /// 限定一次请求上传文件数量（0 无限制）
                /// </summary>
                public int LimitFileNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 文件扩展
                /// </summary>
                public string[] Extension => this._Extension;

                /// <summary>
                /// 最大文件大小
                /// </summary>
                public int MaxSize => this._MaxSize;


                public ApiUpConfig(int maxLen, string[] extensions)
                {
                        this._MaxSize = maxLen;
                        this._Extension = extensions;
                        this.LimitFileNum = 1;
                }
                public ApiUpConfig(UpFileFormat format, params string[] extensions)
                {
                        this._MaxSize = 10 * 1024 * 1024;
                        this.LimitFileNum = 1;
                        if ((UpFileFormat.图片 & format) == UpFileFormat.图片)
                        {
                                this._Extension = this._Extension.Add(PublicDataDic.MainImgFormat);
                        }
                        if ((UpFileFormat.Excel & format) == UpFileFormat.Excel)
                        {
                                this._Extension = this._Extension.Add(PublicDataDic.ExcelFormat);
                        }
                        if ((UpFileFormat.Word & format) == UpFileFormat.Word)
                        {
                                this._Extension = this._Extension.Add(PublicDataDic.WordFormat);
                        }
                        if ((UpFileFormat.Pdf & format) == UpFileFormat.Pdf)
                        {
                                this._Extension = this._Extension.Add(".pdf");
                        }
                        if ((UpFileFormat.PPT & format) == UpFileFormat.PPT)
                        {
                                this._Extension = this._Extension.Add(new string[] {
                                        ".ppt",
                                        ".pptx"
                                });
                        }
                        if (!extensions.IsNull())
                        {
                                this._Extension = this._Extension.Add(extensions);
                        }
                }
                public ApiUpConfig(int maxLen, UpFileFormat format, params string[] extensions)
                {
                        this.LimitFileNum = 1;
                        this._MaxSize = maxLen;
                        if ((UpFileFormat.图片 & format) == UpFileFormat.图片)
                        {
                                this._Extension = this._Extension.Add(PublicDataDic.MainImgFormat);
                        }
                        if ((UpFileFormat.Excel & format) == UpFileFormat.Excel)
                        {
                                this._Extension = this._Extension.Add(PublicDataDic.ExcelFormat);
                        }
                        if ((UpFileFormat.Word & format) == UpFileFormat.Word)
                        {
                                this._Extension = this._Extension.Add(PublicDataDic.WordFormat);
                        }
                        if ((UpFileFormat.Pdf & format) == UpFileFormat.Pdf)
                        {
                                this._Extension = this._Extension.Add(".pdf");
                        }
                        if ((UpFileFormat.PPT & format) == UpFileFormat.PPT)
                        {
                                this._Extension = this._Extension.Add(new string[] {
                                        ".ppt",
                                        ".pptx"
                                });
                        }
                        if (!extensions.IsNull())
                        {
                                this._Extension = this._Extension.Add(extensions);
                        }
                        this._Format = this._Extension.Join(",");
                }
                private readonly string _Format = null;


                public void CheckUpFile(string fileName, int num)
                {
                        if (!this._CheckFormat(fileName))
                        {
                                throw new ErrorException("file.up.format.error", "format:" + this._Format);
                        }
                        else if (this.LimitFileNum != 0 && this.LimitFileNum < num)
                        {
                                throw new ErrorException("file.up.num.limit", "limit:" + this.LimitFileNum);
                        }
                }
                public void CheckFileSize(long size)
                {
                        if (this._MaxSize < size)
                        {
                                throw new ErrorException("file.up.size.error", "size:" + this._MaxSize);
                        }
                }
                private bool _CheckFormat(string fileName)
                {
                        if (fileName.IsNull())
                        {
                                return false;
                        }
                        else
                        {
                                int index = fileName.LastIndexOf(".");
                                if (index == -1)
                                {
                                        return false;
                                }
                                string ext = fileName.Substring(index).ToLower();
                                return this._Extension.IsExists(ext);
                        }
                }

                public ApiUpSet ToUpSet()
                {
                        return new ApiUpSet
                        {
                                Extension=this.Extension,
                                LimitFileNum=this.LimitFileNum,
                                MaxSize=this.MaxSize
                        };
                }
        }
}
