using WeDonekRpc.HttpService.Interface;
using System;
using System.IO;
using System.Text;

namespace WeDonekRpc.HttpService.FileUp
{
    internal class SyncUpFileHelper : ISyncUpFileHelper
    {
        public SyncUpFileHelper ( IHttpRequest request )
        {
            this._StreamLen = request.ContentLength;
            this._boundary = Encoding.UTF8.GetBytes("--" + request.ContentType.Substring(30).Trim());
        }

        private readonly byte[] _boundary = null;

        private bool _IsLoadHead = true;

        private readonly long _StreamLen = 0;

        public void LoadFile ( Stream stream, IUpFileRequest handler )
        {
            byte[] myByte = new byte[2048];
            int len = 0;
            UpFilePage page = null;
            do
            {
                int num = stream.Read(myByte, 0, 2048);
                if ( num > 0 )
                {
                    len += num;
                    this._SplictPage(handler, myByte, num, ref page);
                }
            } while ( len != this._StreamLen );

        }
        private void _SplictPage ( IUpFileRequest handler, byte[] myByte, int len, ref UpFilePage page )
        {
            int index = 0;
            try
            {
                this._SplictPage(handler, myByte, len, ref index, ref page);
            }
            catch ( Exception e )
            {
                page?.Delete(e);
                throw;
            }
        }
        private void _SplictPage ( IUpFileRequest handler, byte[] myByte, int len, ref int index, ref UpFilePage page )
        {
            if ( page == null )
            {
                page = new UpFilePage(this._boundary, handler, this._IsLoadHead);
                this._IsLoadHead = false;
            }
            if ( page.LoadData(myByte, len, ref index) )
            {
                page.Dispose();
                page = null;
                if ( index != len )
                {
                    this._SplictPage(handler, myByte, len, ref index, ref page);
                }
            }
        }


    }
}
