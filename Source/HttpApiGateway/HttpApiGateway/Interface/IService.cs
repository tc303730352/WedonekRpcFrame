using System;

namespace HttpApiGateway.Interface
{
        internal interface IService : IApiService
        {
                bool IsError { get; }

                string LastError { get; }
                void InitService(IApiModular modular);
                void ReplyError(string error);
                void Reply(object result, object count);
                void Reply();
                void Reply(object result);
                void Reply(IResponse response);
                void ReplyError(string show, Exception e);
        }
}
