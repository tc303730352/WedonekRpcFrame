namespace HttpApiGateway.Interface
{
        /// <summary>
        /// HttpApi 
        /// </summary>
        internal interface IHttpApi
        {
                /// <summary>
                /// Api地址
                /// </summary>
                string ApiName { get; }


                /// <summary>
                /// Api地址
                /// </summary>
                string ApiUri { get; }

                bool VerificationApi();
                void ExecApi(IService service);
                void RegApi(IApiRoute route);
        }
}
