using RpcModel;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface ISysConfigCollect
        {
                long AddSysConfig(SysConfigAddParam add);
                void DropConfig(long id);
                SysConfigDatum GetSysConfig(long Id);
                SysConfigDatum[] Query(QuerySysParam query, IBasicPage paging, out long count);
                void SetSysConfig(long id, SysConfigSetParam config);
                long FindConfigId(long systemTypeId, string key);
        }
}