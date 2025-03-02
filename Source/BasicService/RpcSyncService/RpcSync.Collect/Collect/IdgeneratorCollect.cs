using RpcSync.DAL;
using RpcSync.Model.DB;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;

namespace RpcSync.Collect.Collect
{
    internal class IdgeneratorCollect : IIdgeneratorCollect
    {
        private readonly IIdgeneratorDAL _Idgenerator;
        public IdgeneratorCollect (IIdgeneratorDAL idgenerator)
        {
            this._Idgenerator = idgenerator;
        }

        public int GetWorkId (string mac, int index, long sysTypeId)
        {
            int workId = this._Idgenerator.GetWorkId(mac, index, sysTypeId);
            if (workId != 0)
            {
                return workId;
            }
            using (RemoteLock remoteLock = RemoteLock.ApplyLock("ApplyIdgenerator_" + sysTypeId))
            {
                if (remoteLock.GetLock())
                {
                    workId = this._Idgenerator.GetMaxWorkId() + 1;
                    if (workId < 10)
                    {
                        workId = 10;
                    }
                    this._Idgenerator.Add(new IdgeneratorModel
                    {
                        WorkId = workId,
                        Mac = mac,
                        ServerIndex = index,
                        SystemTypeId = sysTypeId
                    });
                    remoteLock.Exit(workId.ToString());
                    return workId;
                }
                else if (remoteLock.IsError)
                {
                    throw new ErrorException(remoteLock.Error);
                }
                else
                {
                    return int.Parse(remoteLock.Extend);
                }
            }
        }
    }
}
