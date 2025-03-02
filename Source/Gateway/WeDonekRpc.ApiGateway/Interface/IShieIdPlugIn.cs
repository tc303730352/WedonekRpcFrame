namespace WeDonekRpc.ApiGateway.Interface
{
        public interface IShieIdPlugIn: IPlugIn
        {
                bool CheckIsShieId(string path);
        }
}