using System;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Helper;
namespace WeDonekRpc.ApiGateway.Attr
{

    /// <summary>
    /// 上传配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class ApiUpConfig : Attribute
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

        /// <summary>
        /// 分块上传大小
        /// </summary>
        public int BlockUpSize
        {
            get;
            set;
        }
        /// <summary>
        /// 上传文件是否计算MD5
        /// </summary>
        public bool IsGenerateMd5
        {
            get;
            set;
        }
        /// <summary>
        /// 临时文件保存目录
        /// </summary>
        public string TempFileSaveDir
        {
            get;
            set;
        }
        public Type ConfigType { get; }
        public ApiUpConfig (Type upConfigType)
        {
            this.ConfigType = upConfigType;
        }
        public ApiUpConfig (int maxSize, int blockUpSize, string[] extensions)
        {
            this._MaxSize = maxSize;
            this.BlockUpSize = blockUpSize;
            this._Extension = extensions;
            this.LimitFileNum = 1;
        }
        public ApiUpConfig (int maxSize, int blockUpSize, string tempFileSaveDir, string[] extensions)
        {
            this._MaxSize = maxSize;
            this.TempFileSaveDir = tempFileSaveDir;
            this.BlockUpSize = blockUpSize;
            this._Extension = extensions;
            this.LimitFileNum = 1;
        }
        public ApiUpConfig (int maxSize, string[] extensions)
        {
            this._MaxSize = maxSize;
            this._Extension = extensions;
            this.LimitFileNum = 1;
        }
        public ApiUpConfig (UpFileFormat format, params string[] extensions)
        {
            this._MaxSize = 10 * 1024 * 1024;
            this.LimitFileNum = 1;
            if (( UpFileFormat.图片 & format ) == UpFileFormat.图片)
            {
                this._Extension = this._Extension.Add(PublicDataDic.MainImgFormat);
            }
            if (( UpFileFormat.Excel & format ) == UpFileFormat.Excel)
            {
                this._Extension = this._Extension.Add(PublicDataDic.ExcelFormat);
            }
            if (( UpFileFormat.Word & format ) == UpFileFormat.Word)
            {
                this._Extension = this._Extension.Add(PublicDataDic.WordFormat);
            }
            if (( UpFileFormat.Pdf & format ) == UpFileFormat.Pdf)
            {
                this._Extension = this._Extension.Add(".pdf");
            }
            if (( UpFileFormat.PPT & format ) == UpFileFormat.PPT)
            {
                this._Extension = this._Extension.Add(new string[] { ".ppt", ".pptx" });
            }
            if (!extensions.IsNull())
            {
                this._Extension = this._Extension.Add(extensions);
            }
        }
        public ApiUpConfig (int maxSize, UpFileFormat format, params string[] extensions)
        {
            this.LimitFileNum = 1;
            this._MaxSize = maxSize;
            if (( UpFileFormat.图片 & format ) == UpFileFormat.图片)
            {
                this._Extension = this._Extension.Add(PublicDataDic.MainImgFormat);
            }
            if (( UpFileFormat.Excel & format ) == UpFileFormat.Excel)
            {
                this._Extension = this._Extension.Add(PublicDataDic.ExcelFormat);
            }
            if (( UpFileFormat.Word & format ) == UpFileFormat.Word)
            {
                this._Extension = this._Extension.Add(PublicDataDic.WordFormat);
            }
            if (( UpFileFormat.Pdf & format ) == UpFileFormat.Pdf)
            {
                this._Extension = this._Extension.Add(".pdf");
            }
            if (( UpFileFormat.PPT & format ) == UpFileFormat.PPT)
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

        public ApiUpSet ToUpSet ()
        {
            return new ApiUpSet
            {
                Extension = this.Extension,
                LimitFileNum = this.LimitFileNum,
                MaxSize = this.MaxSize,
                BlockUpSize = this.BlockUpSize,
                TempFileSaveDir = this.TempFileSaveDir,
                IsGenerateMd5 = this.IsGenerateMd5
            };
        }
    }
}
