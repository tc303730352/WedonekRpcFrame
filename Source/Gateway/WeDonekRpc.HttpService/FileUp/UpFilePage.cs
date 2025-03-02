using System;
using System.IO;
using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.FileUp
{
    internal enum UpFileProgress
    {
        加载中 = 0,
        头部加载完成 = 1,
        包体加载完成 = 2,
        加载完成 = 3,
        加载错误 = 4
    }
    internal class UpFilePage : IDisposable
    {
        private readonly IUpFileRequest _Request = null;

        private readonly UpFileParam _UpParam = null;

        private Stream _Stream = null;
        public UpFilePage ( byte[] boundary, IUpFileRequest request, bool isHead )
        {
            this._Request = request;
            this._UpParam = new UpFileParam(this._Request.IsGenerateMd5);
            this._boundary = boundary;
            this._Head = new byte[boundary.Length + 2];
            this._LoadProgress = isHead ? UpFileProgress.加载中 : UpFileProgress.头部加载完成;
        }
        private volatile UpFileProgress _LoadProgress = UpFileProgress.加载中;

        private readonly byte[] _boundary = null;

        private readonly byte[] _Head = null;

        private int _HeadLen = 0;
        private readonly Encoding _Encoding = Encoding.UTF8;

        private bool _LoadHead ( byte[] data, int len, ref int index )
        {
            if ( this._LoadProgress != UpFileProgress.加载中 )
            {
                return true;
            }
            else
            {
                int num = len - index;
                if ( num > this._Head.Length - this._HeadLen )
                {
                    num = this._Head.Length - this._HeadLen;
                }
                Buffer.BlockCopy(data, index, this._Head, this._HeadLen, num);
                this._HeadLen += num;
                index += num;
                if ( this._HeadLen != this._Head.Length )
                {
                    return false;
                }
                if ( !this._CheckHed() )
                {
                    throw new ErrorException("");
                }
                else
                {
                    this._LoadProgress = UpFileProgress.头部加载完成;
                    return true;
                }
            }
        }

        private bool _CheckHed ()
        {
            if ( this._Head[this._HeadLen - 1] != 10 && this._Head[this._HeadLen - 2] != 13 )
            {
                return false;
            }
            else
            {
                for ( int i = 0 ; i < this._boundary.Length ; i++ )
                {
                    if ( this._boundary[i] != this._Head[i] )
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        internal void Delete ( Exception e )
        {
            this.Dispose();
            this._Request.UpFail(e);
        }

        private MemoryStream _RowStream = null;

        internal UpFileProgress LoadProgress => this._LoadProgress;

        private bool _LoadRow ( byte[] data, int len, out byte[] myByte, ref int index )
        {
            if ( index >= len )
            {
                myByte = null;
                return false;
            }
            else if ( this._RowStream == null )
            {
                this._RowStream = new MemoryStream();
            }
            int end = _GetRowLen(data, len, index);
            int num = end - index;
            if ( num == 0 )
            {
                index += 2;
                return this._LoadRow(data, len, out myByte, ref index);
            }
            else if ( end == -1 )
            {
                num = len - index;
                this._RowStream.Position = this._RowStream.Length;
                this._RowStream.Write(data, index, num);
                this._RowStream.Flush();
                index += num;
                if ( _CheckIsEnd(this._RowStream) )
                {
                    myByte = this._RowStream.ToArray();
                    this._RowStream.Close();
                    this._RowStream = null;
                    return true;
                }
                myByte = null;
                return false;
            }
            else if ( this._RowStream.Length == 0 )
            {
                myByte = new byte[num];
                Buffer.BlockCopy(data, index, myByte, 0, num);
                index += num;
                return true;
            }
            else
            {
                this._RowStream.Position = this._RowStream.Length;
                this._RowStream.Write(data, index, num);
                this._RowStream.Flush();
                index += num;
                myByte = this._RowStream.ToArray();
                this._RowStream.Close();
                this._RowStream = null;
                return true;
            }
        }
        private static bool _CheckIsEnd ( MemoryStream stream )
        {
            if ( stream.Length < 2 )
            {
                return false;
            }
            stream.Position = stream.Length - 2;
            return stream.ReadByte() == 13 && stream.ReadByte() == 10;
        }
        private static int _GetRowLen ( byte[] data, int len, int index )
        {
            int end = len - 1;
            for ( int i = index ; i < end ; i++ )
            {
                if ( data[i] == 13 && data[i + 1] == 10 )
                {
                    return i + 2;
                }
            }
            return -1;
        }
        internal void _InitPageBody ( byte[] myByte, ref int index )
        {
            string res = this._Encoding.GetString(myByte);
            if ( res.StartsWith("Content-Disposition") )
            {
                this._UpParam.Init(res);
                if ( this._UpParam.Name != "description" )
                {
                    if ( this._UpParam.IsFile() )
                    {
                        this._Request.CheckUpFile(this._UpParam);
                    }
                    this._Stream = this._Request.GetSaveStream(this._UpParam);
                    if ( this._Stream == null )
                    {
                        throw new ErrorException("http.file.save.stream.null");
                    }
                }
                else
                {
                    this._Stream = new MemoryStream();
                    this._LoadProgress = UpFileProgress.包体加载完成;
                    index += 2;
                }
            }
            else if ( res.StartsWith("Content-Type") )
            {
                this._UpParam.ContentType = res.Remove(0, 14).Replace("\r\n", string.Empty);
                this._LoadProgress = UpFileProgress.包体加载完成;
                index += 2;
            }
            else
            {
                this._LoadProgress = UpFileProgress.加载错误;
                throw new ErrorException("http.file.up.body.error");
            }
        }
        private bool _CheckIsEndRow ( byte[] myByte )
        {
            if ( myByte.Length < this._boundary.Length || myByte[0] != 45 || myByte[1] != 45 )
            {
                return false;
            }
            else
            {
                for ( int i = this._boundary.Length - 1 ; i > 1 ; i-- )
                {
                    if ( this._boundary[i] != myByte[i] )
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private void _VerFileSize ()
        {
            this._Request.VerificationFile(this._UpParam, this._Stream.Length); ;
        }
        private void _SaveFile ()
        {
            this._Request.SaveUpStream(this._UpParam, this._Stream);
        }
        private byte[] _upperRow = null;
        internal bool LoadData ( byte[] data, int len, ref int index )
        {
            if ( !this._LoadHead(data, len, ref index) )
            {
                return false;
            }
            do
            {
                if ( !this._LoadRow(data, len, out byte[] myByte, ref index) )
                {
                    return false;
                }
                else if ( this._LoadProgress == UpFileProgress.头部加载完成 )
                {
                    this._InitPageBody(myByte, ref index);
                }
                else if ( this._CheckIsEndRow(myByte) )
                {
                    if ( this._upperRow != null && this._LoadProgress == UpFileProgress.包体加载完成 )
                    {
                        int size = this._upperRow.Length - 2;
                        this._Stream.Write(this._upperRow, 0, size);
                        this._Stream.Flush();
                        this._UpParam.End(this._upperRow, size);
                    }
                    this._VerFileSize();
                    this._LoadProgress = UpFileProgress.加载完成;
                    this._SaveFile();
                    return true;
                }
                else
                {
                    if ( this._upperRow != null )
                    {
                        this._Stream.Write(this._upperRow, 0, this._upperRow.Length);
                        this._Stream.Flush();
                        this._UpParam.CalculateMd5(this._upperRow, this._upperRow.Length);
                    }
                    this._upperRow = myByte;
                }
            } while ( true );
        }

        public void Dispose ()
        {
            this._UpParam.Dispose();
            if ( this._Stream != null )
            {
                this._Stream.Close();
                this._Stream.Dispose();
            }
        }
    }
}
