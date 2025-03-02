using RpcSync.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace RpcSync.Service.Event
{
    internal class IdgeneratorEvent : IRpcApiService
    {
        private readonly IIdgeneratorCollect _Idgenerator;

        public IdgeneratorEvent (IIdgeneratorCollect idgenerator)
        {
            this._Idgenerator = idgenerator;
        }

        public int GetIdgeneratorWorkId (GetIdgeneratorWorkId arg, MsgSource source)
        {
            return this._Idgenerator.GetWorkId(arg.Mac, arg.Index, source.SystemTypeId);
        }
    }
}
