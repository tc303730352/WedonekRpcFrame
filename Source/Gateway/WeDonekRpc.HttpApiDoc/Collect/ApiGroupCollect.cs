using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeDonekRpc.HttpApiDoc.Collect
{
    internal class ApiGroupCollect
    {
        private static ApiGroup[] _GroupList = new Model.ApiGroup[0];
        internal static void AddGroup(ApiGroup group)
        {
            _GroupList = _GroupList.Add(group);
        }

        private static ApiGroupList[] _ApiGroup = null;

        internal static ApiGroupList[] ApiGroup
        {
            get
            {
                if (_ApiGroup == null || _ApiGroup.Length != _GroupList.Length)
                {
                    ApiModel[] list = ApiCollect.ApiList;
                    int id = 1;
                    Dictionary<int, ApiGroupList> dic = new Dictionary<int, ApiGroupList>();
                    list.ForEach(a =>
                    {
                        if (!dic.TryGetValue(a.GroupId, out ApiGroupList group))
                        {
                            if (_GroupList.Length > 0)
                            {
                                ApiGroup obj = _GroupList.Find(b => b.Id == a.GroupId);
                                group = new ApiGroupList
                                {
                                    Id = id++,
                                    Name = obj == null ? "默认" : obj.Name,
                                    Root = obj.RootUri
                                };
                                dic.Add(a.GroupId, group);
                            }
                        }
                        if (group != null)
                        {
                            group.ApiList.Add(a);
                        }
                    });
                    _ApiGroup = dic.Values.ToArray();
                }
                return _ApiGroup;
            }
        }

        public static ApiGroupList GetGroup(int id)
        {
            ApiGroupList[] groups = ApiGroup;
            if (groups == null)
            {
                return null;
            }
            return groups.Find(a => a.Id == id);
        }
        internal static int GetGroup(Type type)
        {
            if (_GroupList == null)
            {
                return 0;
            }
            string str = type.FullName.ToLower();
            ApiGroup group = _GroupList.Find(a => a.CheckIsGroup(str));
            return group != null ? group.Id : 0;
        }

    }
}
