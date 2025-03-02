using System;
using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.Helper
{
    internal class ApiHelper
    {
        public static IUserPage GetPage (byte[] content, out byte[] value)
        {
            if (content.Length == 0)
            {
                value = null;
                return null;
            }
            int index = content.FindIndex(a => a == 10);
            if (index == -1)
            {
                value = null;
                return null;
            }
            int begin = index + 1;
            int len = 0;
            string pageId = null;
            if (content[begin] == 48)
            {
                begin += 2;
                len = content.Length - begin;
            }
            else
            {
                int end = content.FindIndex(begin, a => a == 10);
                if (end == -1)
                {
                    value = null;
                    return null;
                }
                pageId = Encoding.UTF8.GetString(content, begin, end - begin);
                begin = end + 1;
                len = content.Length - begin;
            }
            string direct = Encoding.UTF8.GetString(content, 0, index);
            value = new byte[len];
            if (len > 0)
            {
                Buffer.BlockCopy(content, begin, value, 0, len);
            }
            return new UserPage
            {
                Direct = direct,
                PageId = pageId
            };
        }
    }
}
