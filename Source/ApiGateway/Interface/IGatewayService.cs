namespace ApiGateway.Interface
{
        public interface IGatewayService
        {
                void RegDoc(IApiDocModular doc);
                void RegModular(IModular modular);
        }
}