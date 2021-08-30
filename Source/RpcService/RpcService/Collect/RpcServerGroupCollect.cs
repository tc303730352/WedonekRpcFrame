namespace RpcService.Collect
{
        internal class RpcServerGroupCollect
        {
                public static bool GetGroupTypeVal(long groupId, out string typeVal)
                {
                        string key = string.Concat("SysGTypeVal_", groupId);
                        if (RpcService.Cache.TryGet(key, out typeVal))
                        {
                                return true;
                        }
                        else if (!new DAL.ServerGroupDAL().GetTypeVal(groupId, out typeVal))
                        {
                                return false;
                        }
                        else
                        {
                                RpcService.Cache.Set(key, typeVal, new System.TimeSpan(1, 0, 0, 0));
                                return true;
                        }
                }
        }
}
