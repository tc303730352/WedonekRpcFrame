using System;

namespace HttpService.Interface
{
        public interface IHttpServer
        {
                Uri Uri { get; }

                void Close();
                void Start();
        }
}