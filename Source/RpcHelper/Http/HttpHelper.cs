using System;
using System.IO;
using System.Net;

namespace RpcHelper
{
        public enum HttpReqType
        {
                basic = 0,
                Json = 1,
                image = 2,
                File = 3,
                Html = 4,
                XML = 5
        }
        public class HttpHelper
        {


                private static readonly HttpRequestSet _DownImgMethodSet = new HttpRequestSet("GET", HttpReqType.image)
                {
                        Timeout = 60000,
                        ReadWriteTimeout = 4000,
                        ContinueTimeout = 20000,
                        Referer = "http://mp.weixin.qq.com/",
                        IsResetFileName = true
                };
                private static readonly HttpRequestSet _DownImg = new HttpRequestSet("GET", HttpReqType.image)
                {
                        Timeout = 60000,
                        ReadWriteTimeout = 4000,
                        ContinueTimeout = 20000,
                        IsResetFileName = false
                };
                private static readonly HttpRequestSet _DownFileMethodSet = new HttpRequestSet(HttpReqType.File)
                {
                        Timeout = 130000,
                        ReadWriteTimeout = 120000,
                        ContinueTimeout = 10000
                };
                private const string _PostMethod = "POST";
                public static readonly HttpRequestSet GetMethodSet = new HttpRequestSet(HttpReqType.basic);
                public static readonly HttpRequestSet GetMethodJson = new HttpRequestSet(HttpReqType.Json);
                public static readonly HttpRequestSet PostMethodSet = new HttpRequestSet(_PostMethod, HttpReqType.basic);
                public static readonly HttpRequestSet PostJsonMethodSet = new HttpRequestSet(_PostMethod, HttpReqType.Json);
                private static readonly HttpRequestSet _PostLimitJsonMethodSet = new HttpRequestSet(_PostMethod, HttpReqType.Json) { Accept = "application/json" };
                private static readonly HttpRequestSet _UpFileMethodSet = new HttpRequestSet(HttpReqType.File)
                {
                        Timeout = 120000,
                        ReadWriteTimeout = 120000,
                        ContinueTimeout = 120000
                };
                static HttpHelper()
                {
                        GetMethodSet = new HttpRequestSet(HttpReqType.basic);
                }

                #region 下载图片文件

                public static bool DownImageFile(Uri uri, ref string savePath)
                {
                        return HttpTools.DownImageFile(uri, ref savePath, out _, HttpHelper._DownImgMethodSet);
                }
                public static bool DownImageFile(Uri uri, string savePath)
                {
                        return HttpTools.DownImageFile(uri, ref savePath, out _, HttpHelper._DownImg);
                }
                public static bool DownImageFile(Uri uri, ref string savePath, out HttpStatusCode status)
                {
                        return HttpTools.DownImageFile(uri, ref savePath, out status, HttpHelper._DownImgMethodSet);
                }
                public static bool DownImageFile(Uri uri, ref string savePath, HttpRequestSet reqSet)
                {
                        return HttpTools.DownImageFile(uri, ref savePath, out _, reqSet);
                }
                #endregion



                public static bool SendRequest(Uri uri, string post, out HttpResponseRes data, HttpRequestSet config)
                {
                        return HttpTools.SendRequest(uri, post, out data, config);
                }
                #region 提交GET请求

                public static bool SubmitGetData(Uri uri, out HttpResponseRes data, HttpRequestSet config)
                {
                        return HttpTools.SendRequest(uri, null, out data, config);
                }
                public static bool SubmitGetData(Uri uri, out HttpResponseRes data)
                {
                        return HttpTools.SendRequest(uri, null, out data, HttpHelper.GetMethodSet);
                }
                public static bool SubmitGetData(Uri uri, out string data)
                {
                        return HttpTools.SubmitData(uri, null, out data, HttpHelper.GetMethodSet);
                }
                public static bool SubmitGetData<T>(Uri uri, out T data)
                {
                        if (HttpTools.SubmitData(uri, null, out string json, HttpHelper.GetMethodJson))
                        {
                                //LogSystem.AddLog("HTTP", "Uri:" + uri.AbsoluteUri + "\r\njson:" + json);
                                data = Tools.Json<T>(json);
                                return true;
                        }
                        data = default;
                        return false;
                }
                public static bool SubmitGetData<T>(Uri uri, out T data, HttpRequestSet config)
                {
                        config.SubmitMethod = "GET";
                        if (HttpTools.SubmitData(uri, null, out string json, config))
                        {
                                //LogSystem.AddLog("HTTP", "Uri:" + uri.AbsoluteUri + "\r\njson:" + json);
                                data = Tools.Json<T>(json);
                                return true;
                        }
                        data = default;
                        return false;
                }
                #endregion


                #region 提交POST请求
                public static bool SubmitPostData(Uri uri, string post, out HttpResponseRes res)
                {
                        return HttpTools.SendRequest(uri, post, out res, HttpHelper.PostMethodSet);
                }
                public static bool SubmitPostData(Uri uri, string post, out HttpResponseRes data, HttpRequestSet reqSet)
                {
                        reqSet.SubmitMethod = _PostMethod;
                        return HttpTools.SendRequest(uri, post, out data, reqSet);
                }
                public static bool SubmitPostData(Uri uri, string post, out string data)
                {
                        return HttpTools.SubmitData(uri, post, out data, HttpHelper.PostMethodSet);
                }
                public static bool SubmitPostData<T>(Uri uri, string post, out T data)
                {
                        if (HttpTools.SubmitData(uri, post, out string json, HttpHelper.PostMethodSet))
                        {
                                //LogSystem.AddLog("请求接口!", string.Format("uri:{0}\r\npost:{1}\r\nres:{2}", uri.AbsoluteUri, post, json));
                                data = Tools.Json<T>(json);
                                return true;
                        }
                        data = default;
                        return false;
                }
                public static bool SubmitData<T>(Uri uri, string post, out T data, HttpRequestSet reqSet)
                {
                        if (HttpTools.SubmitData(uri, post, out string json, reqSet))
                        {
                                data = Tools.Json<T>(json);
                                return true;
                        }
                        data = default;
                        return false;
                }
                #endregion



                #region 提交JSON数据
                public static bool SubmitJson<T>(Uri uri, T data, out string res)
                {
                        string json = Tools.Json(data);
                        return HttpTools.SubmitData(uri, json, out res, HttpHelper.PostJsonMethodSet);
                }
                public static bool SubmitJson<T>(Uri uri, string json, out T res)
                {
                        if (HttpTools.SubmitData(uri, json, out string str, HttpHelper.PostJsonMethodSet))
                        {
                                res = Tools.Json<T>(str);
                                return true;
                        }
                        res = default;
                        return false;
                }
                public static bool SubmitJson<T>(Uri uri, T data, out HttpStatusCode status, out string res)
                {
                        string json = Tools.Json(data);
                        return HttpTools.SubmitData(uri, json, out status, out res, HttpHelper._PostLimitJsonMethodSet);
                }
                public static bool SubmitJson<T, Result>(Uri uri, T data, out Result res)
                {
                        return SubmitJson<T, Result>(uri, data, out _, out res);
                }
                public static bool SubmitJson<T, Result>(Uri uri, T data, out HttpStatusCode status, out Result res)
                {
                        string json = Tools.Json(data);
                        if (HttpTools.SubmitData(uri, json, out status, out string str, HttpHelper._PostLimitJsonMethodSet))
                        {
                                if (string.IsNullOrEmpty(str))
                                {
                                        res = default;
                                        return true;
                                }
                                res = Tools.Json<Result>(str);
                                return true;
                        }
                        res = default;
                        return false;
                }
                public static bool SubmitJson(Uri uri, string json, out HttpStatusCode status, out string res)
                {
                        return HttpTools.SubmitData(uri, json, out status, out res, HttpHelper.PostJsonMethodSet);
                }
                public static bool SubmitJson(Uri uri, string json, out string res)
                {
                        return HttpTools.SubmitData(uri, json, out res, HttpHelper.PostJsonMethodSet);
                }

                #endregion


                #region 下载文件

                /// <summary>
                /// 下载文件
                /// </summary>
                /// <param name="uri"></param>
                /// <param name="post"></param>
                /// <param name="savePath"></param>
                /// <returns></returns>
                public static bool DownFile(Uri uri, string post, ref string savePath, out HttpStatusCode status)
                {
                        return HttpTools.DownFile(uri, post, ref savePath, out status, _DownFileMethodSet);
                }
                public static bool DownFile(Uri uri, string post, ref string savePath)
                {
                        return HttpTools.DownFile(uri, post, ref savePath, out _, _DownFileMethodSet);
                }
                public static bool DownFile(Uri uri, string post, ref string savePath, HttpRequestSet reqSet)
                {
                        return HttpTools.DownFile(uri, post, ref savePath, out _, reqSet);
                }
                public static bool DownFile(Uri uri, string post, ref string savePath, out HttpStatusCode status, HttpRequestSet reqSet)
                {
                        return HttpTools.DownFile(uri, post, ref savePath, out status, reqSet);
                }
                /// <summary>
                /// 获取请求头中的文件名
                /// </summary>
                /// <param name="head"></param>
                /// <param name="fileName"></param>
                /// <returns></returns>
                private static bool _GetHeaderFileName(string head, out string fileName)
                {
                        string[] temp = head.ToLower().Split(';');
                        head = Array.Find(temp, a => a.Trim().StartsWith("filename="));
                        if (head != null)
                        {
                                fileName = head.Replace("filename=", string.Empty).Replace("\"", string.Empty).Trim();
                                return true;
                        }
                        fileName = null;
                        return false;
                }
                public static string GetFileype(Uri uri, HttpStreamRes response)
                {
                        string fileType = Path.GetExtension(uri.LocalPath);
                        KeValue val = Array.Find(response.HeadList, a => a.Name == "Content-Disposition");
                        if (val.Name != null && _GetHeaderFileName(val.Value, out string newName))
                        {
                                fileType = Path.GetExtension(newName);
                        }
                        if (string.IsNullOrEmpty(fileType) && !string.IsNullOrEmpty(response.ContentType))
                        {
                                fileType = Tools.GetFileExtension(response.ContentType);
                        }
                        if (fileType == ".jpeg")
                        {
                                fileType = ".jpg";
                        }
                        return fileType;
                }
                /// <summary>
                /// 下载文件
                /// </summary>
                /// <param name="uri"></param>
                /// <param name="savePath"></param>
                /// <param name="reqSet"></param>
                /// <returns></returns>
                public static bool DownFile(Uri uri, ref string savePath)
                {
                        return HttpTools.DownFile(uri, null, ref savePath, out _, _DownFileMethodSet);
                }
                public static bool DownFile(Uri uri, ref string savePath, HttpRequestSet reqSet)
                {
                        return HttpTools.DownFile(uri, null, ref savePath, out _, reqSet);
                }
                public static bool DownFile(Uri uri, ref string savePath, out HttpStatusCode status, HttpRequestSet reqSet)
                {
                        return HttpTools.DownFile(uri, null, ref savePath, out status, reqSet);
                }
                #endregion
                #region 上传文件
                public static bool UploadFile(Uri uri, string postData, string filePath, out HttpStatusCode status, out string json)
                {
                        return HttpTools.UploadFile(uri, postData, filePath, out status, out json, HttpHelper._UpFileMethodSet);
                }

                public static bool UploadFile(Uri uri, string postData, string filePath, out string json)
                {
                        return HttpTools.UploadFile(uri, postData, filePath, out _, out json, HttpHelper._UpFileMethodSet);
                }
                #endregion
                #region 获取远程数据流
                public static bool GetRemoteStream(Uri uri, string postStr, out byte[] datas)
                {
                        return HttpTools.SubmitData(uri, postStr, out datas, HttpHelper.PostMethodSet);
                }
                public static bool GetRemoteStream(Uri uri, string postStr, out HttpStreamRes res)
                {
                        return HttpTools.GetStream(uri, postStr, out res, HttpHelper.PostMethodSet);
                }
                public static bool GetRemoteStream(Uri uri, out HttpStreamRes res)
                {
                        return HttpTools.GetStream(uri, null, out res, HttpHelper.GetMethodSet);
                }
                public static bool GetRemoteStream(Uri uri, out byte[] datas)
                {
                        return HttpTools.SubmitData(uri, null, out datas, HttpHelper.GetMethodSet);
                }
                public static bool GetRemoteStream(Uri uri, out HttpStreamRes res, HttpRequestSet reqSet)
                {
                        return HttpTools.GetStream(uri, null, out res, reqSet);
                }
                public static bool GetRemoteStream(Uri uri, string postStr, out byte[] datas, out HttpStatusCode status)
                {
                        return HttpTools.SubmitData(uri, postStr, out status, out datas, HttpHelper.PostMethodSet);
                }
                public static bool GetRemoteStream(Uri uri, string postStr, out byte[] datas, HttpRequestSet reqSet)
                {
                        return HttpTools.SubmitData(uri, postStr, out datas, reqSet);
                }
                public static bool GetRemoteStream(Uri uri, string postStr, out HttpStreamRes res, HttpRequestSet reqSet)
                {
                        return HttpTools.GetStream(uri, postStr, out res, reqSet);
                }
                #endregion
                public static bool SubmitData(Uri uri, string post, out HttpStatusCode status, out string data, HttpRequestSet reqSet)
                {
                        return HttpTools.SubmitData(uri, post, out status, out data, reqSet);
                }
                public static bool SubmitData(Uri uri, string post, out string data, HttpRequestSet reqSet)
                {
                        return HttpTools.SubmitData(uri, post, out data, reqSet);
                }
        }
}
