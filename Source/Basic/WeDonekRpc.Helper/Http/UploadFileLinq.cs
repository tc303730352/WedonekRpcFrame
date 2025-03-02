using System;
using System.IO;
using System.Net.Http;

namespace WeDonekRpc.Helper.Http
{
    public static class UploadFileLinq
    {
        public static HttpResult UploadFile ( this HttpClient client, Uri uri, FileInfo file, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            return UploadFile(client, uri, file, HttpHelper.DefConfig, option);
        }
        public static HttpResult UploadFile<T> ( this HttpClient client, Uri uri, T post, FileInfo file, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead ) where T : class
        {
            return UploadFile(client, uri, post, file, HttpHelper.DefConfig, option);
        }
        public static HttpResult UploadFile ( this HttpClient client, Uri uri, FileInfo file, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, uri) )
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                using ( Stream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read) )
                {
                    content.Add(new StreamContent(stream, set.WriteBufferSize), "file", file.Name);
                    msg.Content = content;
                    set.Init(client, msg.Content, uri);
                    using ( HttpResponseMessage response = client.Send(msg, option) )
                    {
                        return HttpHelper.GetResponse(response, set, option);
                    }
                }
            }
        }
        public static HttpResult UploadFile<T> ( this HttpClient client, Uri uri, T post, FileInfo file, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead ) where T : class
        {
            return UploadFile(client, uri, new StringContent(post.ToJson(), set.RequestEncoding, "application/json"), file, set, option);
        }

        public static HttpResult UploadFile ( this HttpClient client, Uri uri, StringContent post, FileInfo file, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            return UploadFile(client, uri, post, file, HttpHelper.DefConfig, option);
        }

        public static HttpResult UploadFile ( this HttpClient client, Uri uri, StringContent post, FileInfo file, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead ) 
        {
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, uri) )
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                using ( Stream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read) )
                {
                    content.Add(new StreamContent(stream, set.WriteBufferSize), "file", file.Name);
                    if ( post != null )
                    {
                        content.Add(post, "description");
                    }
                    msg.Content = content;
                    set.Init(client, msg.Content, uri);
                    using ( HttpResponseMessage response = client.Send(msg, option) )
                    {
                        return HttpHelper.GetResponse(response, set, option);
                    }
                }
            }
        }
    }
}
