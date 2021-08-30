using System;

namespace RpcSyncService.Collect
{
        internal class BasicCollect<T> where T : new()
        {
                [ThreadStatic]
                private static T _DAL;

                protected static T BasicDAL
                {
                        get
                        {
                                if (_DAL == null)
                                {
                                        _DAL = new T();
                                }
                                return _DAL;
                        }
                }
        }
}
