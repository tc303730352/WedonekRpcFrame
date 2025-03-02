using System;
using System.Security.Cryptography;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpService.FileUp
{
    /// <summary>
    /// 上传文件参数
    /// </summary>
    public class UpFileParam : IDisposable
    {
        private readonly MD5 _Md5 = null;
        internal UpFileParam ( bool isMd5 )
        {
            if ( isMd5 )
            {
                this._Md5 = MD5.Create();
                this._Md5.Initialize();
            }
        }
        /// <summary>
        /// Disposition
        /// </summary>
        public string Disposition { get; private set; }
        /// <summary>
        ///上传类型
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; internal set; }

        public string FileMd5 { get; private set; }


        public void Dispose ()
        {
            this._Md5?.Dispose();
        }

        internal void CalculateMd5 ( byte[] stream, int len )
        {
            _ = this._Md5?.TransformBlock(stream, 0, len, stream, 0);
        }
        internal void End ( byte[] stream, int len )
        {
            if ( this._Md5 != null )
            {
                _ = this._Md5.TransformFinalBlock(stream, 0, len);
                this.FileMd5 = BitConverter.ToString(this._Md5.Hash).Replace("-", string.Empty);
            }
        }
        internal bool IsFile ()
        {
            return this.FileName != null && this.ContentType != null;
        }
        internal void Init ( string res )
        {
            string[] t = res.Split(';');
            int i = 0;
            t.ForEach(a =>
            {
                a = a.Trim();
                i = a.IndexOf('=');
                if ( a.StartsWith("Content-Disposition") )
                {
                    this.Disposition = a.Remove(0, 20).Trim();
                }
                else if ( a.StartsWith("name") )
                {
                    this.Name = a.Remove(0, i + 1).Replace("\"", string.Empty);
                }
                else if ( a.StartsWith("filename") )
                {
                    this.FileName = a.Remove(0, i + 1).Replace("\"", string.Empty);
                }
                else if ( a.StartsWith("Content-Type") )
                {
                    this.ContentType = a.Remove(0, i + 1).Replace("\"", string.Empty);
                }
            });
        }
    }
}
