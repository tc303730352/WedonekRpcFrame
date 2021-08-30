using ApiGateway.Interface;
using ApiGateway.Model;

using RpcHelper;
namespace HttpApiGateway.Model
{
        /// <summary>
        /// 上传配置
        /// </summary>
        public class UpConfig : IUpCheck
        {
                /// <summary>
                /// 单文件上传大小
                /// </summary>
                public int SingleFileSize
                {
                        get;
                        set;
                } = 1024 * 1024 * 10;
                /// <summary>
                /// 允许的扩展
                /// </summary>
                public string[] FileExtension
                {
                        get;
                        set;
                }
                /// <summary>
                /// 限定一次请求上传文件数量（0 无限制）
                /// </summary>
                public int LimitFileNum
                {
                        get;
                        set;
                }
                private string _Format;


                private string _GetFormat()
                {
                        if (this._Format == null)
                        {
                                this._Format = this.FileExtension.Join(",");
                        }
                        return this._Format;
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
                                return this.FileExtension.IsExists(ext);
                        }
                }

                public void CheckUpFile(string fileName, int num)
                {
                        if (!this.FileExtension.IsNull() && !this._CheckFormat(fileName))
                        {
                                throw new ErrorException("file.up.format.error", "format:" + this._GetFormat());
                        }
                        else if (this.LimitFileNum != 0 && this.LimitFileNum < num)
                        {
                                throw new ErrorException("file.up.num.limit", "limit:" + this.LimitFileNum);
                        }
                }
                public void CheckFileSize(long size)
                {
                        if (this.SingleFileSize < size)
                        {
                                throw new ErrorException("file.up.size.error", "size:" + this.SingleFileSize);
                        }
                }
                public ApiUpSet ToUpSet()
                {
                        return new ApiUpSet
                        {
                                Extension = this.FileExtension,
                                LimitFileNum = this.LimitFileNum,
                                MaxSize = this.SingleFileSize
                        };
                }
        }
}
