using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WeDonekRpc.Helper.Http
{
    public static class HttpLinqHelper
    {
        
        public static HttpResult SendRequest ( this HttpClient client, HttpRequestMessage msg, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            return SendRequest(client, msg, HttpHelper.DefConfig, option);
        }
        public static HttpResult SendRequest ( this HttpClient client, HttpRequestMessage msg, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            set.Init(client, msg.Content, msg.RequestUri);
            using ( HttpResponseMessage response = client.Send(msg, option) )
            {
                return HttpHelper.GetResponse(response, set, option);
            }
        }
        public static HttpResult SendGet ( this HttpClient client, Uri uri, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            return SendGet(client, uri, HttpHelper.DefConfig, option);
        }
        public static HttpResult SendJson<T> ( this HttpClient client, Uri uri, T data, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead ) where T : class
        {
            return SendJson(client, uri, data.ToJson(), HttpHelper.DefConfig, option);
        }
        public static HttpResult SendJson<T> ( this HttpClient client, Uri uri, T data, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead ) where T : class
        {
            return SendJson(client, uri, data.ToJson(), set, option);
        }
        public static HttpResult SendJson ( this HttpClient client, Uri uri, string json, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            return SendJson(client, uri, json, HttpHelper.DefConfig, option);
        }
        public static HttpResult SendGet ( this HttpClient client, Uri uri, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, uri) )
            {
                set.Init(client, msg.Content, uri);
                using ( HttpResponseMessage response = client.Send(msg, option) )
                {
                    return HttpHelper.GetResponse(response, set, option);
                }
            }
        }
        public static HttpResult SendJson ( this HttpClient client, Uri uri, string json, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, uri) )
            {
                msg.Content = new StringContent(json, set.RequestEncoding, "application/json");
                set.Init(client, msg.Content, uri);
                using ( HttpResponseMessage response = client.Send(msg, option) )
                {
                    return HttpHelper.GetResponse(response, set, option);
                }
            }
        }
        public static HttpResult SendPostForm ( this HttpClient client, Uri uri, Dictionary<string, string> form, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            return SendPostForm(client, uri, form, HttpHelper.DefConfig, option);
        }
        public static HttpResult SendPostForm ( this HttpClient client, Uri uri, Dictionary<string, string> form, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            StringBuilder str = new StringBuilder();
            form.ForEach(( a, b ) =>
            {
                str.Append('&');
                str.Append(a);
                str.Append('=');
                str.Append(b);
            });
            str.Remove(0, 1);
            return SendPostForm(client, uri, str.ToString(), set, option);
        }
        public static HttpResult SendPostForm ( this HttpClient client, Uri uri, string form, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            return SendPostForm(client, uri, form, HttpHelper.DefConfig, option);
        }
        public static HttpResult SendPostForm ( this HttpClient client, Uri uri, string form, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, uri) )
            {
                msg.Content = new StringContent(form, set.RequestEncoding, "application/x-www-form-urlencoded");
                set.Init(client, msg.Content, uri);
                using ( HttpResponseMessage response = client.Send(msg, option) )
                {
                    return HttpHelper.GetResponse(response, set, option);
                }
            }
        }
    }
}
