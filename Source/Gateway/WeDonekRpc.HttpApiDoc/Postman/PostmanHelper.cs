using System;
using System.Linq;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiDoc.Collect;
using WeDonekRpc.HttpApiDoc.Helper;
using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.HttpApiDoc.Postman.Model;

namespace WeDonekRpc.HttpApiDoc.Postman
{
    internal class PostmanHelper
    {
        public static Postman_Config CreatePostman (ApiGroupList[] groups)
        {
            Postman_Info info = new Postman_Info
            {
                name = "所有API",
                _postman_id = Guid.NewGuid()
            };

            Postman_item[] items = groups.ConvertAll(a => new Postman_item
            {
                name = a.Name,
                item = a.ApiList.ToArray(b => _GetPostmanItem(b))
            });
            return new Postman_Config
            {
                info = info,
                item = items
            };
        }
        public static Postman_Config CreatePostman (ApiGroupList group)
        {
            Postman_Info info = new Postman_Info
            {
                name = group.Name,
                _postman_id = Guid.NewGuid()
            };
            Postman_item[] items = group.ApiList.ToArray(a => _GetPostmanItem(a));
            return new Postman_Config
            {
                info = info,
                item = items
            };
        }

        private static Postman_item _GetPostmanItem (ApiModel obj)
        {
            ApiInfo api = ApiCollect.GetApiInfo(obj.Id);
            Uri uri = api.BasicSet.Uri;
            Postman_body body = null;
            Postman_query[] query = null;
            if (!api.GetParam.IsNull())
            {
                query = api.GetParam.ConvertAll(a => new Postman_query
                {
                    key = a.ParamName,
                    value = a.DefValue == "Null" ? string.Empty : a.DefValue
                });
            }
            if (api.SubmitBody != null && !api.SubmitBody.ProList.IsNull())
            {
                body = new Postman_body
                {
                    raw = ApiDataHelper.GetPostFormat(api.SubmitBody)
                };
            }
            Postman_header[] header = null;
            if (!api.BasicSet.Header.IsNull())
            {
                header = api.BasicSet.Header.ConvertAll(a => new Postman_header
                {
                    key = a.key,
                    value = a.value
                });
            }
            return new Postman_item
            {
                name = api.BasicSet.Show,
                request = new Postman_request
                {
                    url = new Postman_url
                    {
                        raw = uri.AbsoluteUri,
                        host = uri.Host.Split('.'),
                        path = uri.AbsolutePath.Remove(0, 1).Split('/'),
                        port = uri.IsDefaultPort ? null : uri.Port.ToString(),
                        protocol = uri.Scheme,
                        query = query
                    },
                    header = header,
                    method = api.BasicSet.RequestMethod,
                    body = body,
                }
            };
        }
    }
}
