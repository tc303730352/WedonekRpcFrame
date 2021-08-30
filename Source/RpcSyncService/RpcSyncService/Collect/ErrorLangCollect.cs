using System;

using RpcSyncService.Model.DAL_Model;

namespace RpcSyncService.Collect
{
        internal class ErrorLangCollect
        {
                public static bool GetErrorLang(long id, out ErrorLang[] langs, out string error)
                {
                        string key = string.Concat("ErrorLang_", id);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out langs))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.ErrorDAL().GetErrorLang(id, out langs))
                        {
                                error = "rpc.sync.error.lang.get.error";
                                return false;
                        }
                        else
                        {
                                RpcClient.RpcClient.Cache.Set(key, langs, new TimeSpan(10, 0, 0));
                                error = null;
                                return true;
                        }
                }

                internal static void Clear(long errorId)
                {
                        string key = string.Concat("ErrorLang_", errorId);
                        RpcClient.RpcClient.Cache.Remove(key);
                }
        }
}
