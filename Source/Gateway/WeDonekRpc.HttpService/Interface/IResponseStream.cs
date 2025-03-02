using System;
using System.IO;
using System.Net;
using System.Text;

namespace WeDonekRpc.HttpService.Interface
{
    internal interface IResponseStream: IDisposable
    {
        void WriteText(string text, Encoding encoding);
        void WriteStream(Stream stream, string extension);
        void WriteFile(FileInfo file);
    }
}