using System;

namespace WeDonekRpc.HttpService.Interface
{
    public interface IHttpServer
    {
        Uri Uri { get; }

        void Close ();
        void Start ();
    }
}