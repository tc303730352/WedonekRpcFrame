using System;
using System.IO;
using System.Net;
using System.Text;

using HttpService.Model;

using RpcHelper;

namespace HttpService.Helper
{
        internal class FileWriteHelper
        {
                /// <summary>
                /// 最小文件大小
                /// </summary>
                private static readonly long _MinFileSize = 10485760;

                public static void WriteStream(HttpListenerResponse response, Stream stream, string type, string range)
                {
                        if (range != null && range.StartsWith("bytes="))
                        {
                                range = range.Remove(0, 6);
                                if (range.IndexOf(',') == -1)
                                {
                                        FileWriteHelper._WriteStream(response, stream, type, range);
                                }
                                else
                                {
                                        FileWriteHelper._WriteStream(response, stream, type, range.Split(','));
                                }
                        }
                        else
                        {
                                _WriteStream(response, type, stream);
                        }
                }
                private static void _WriteStream(HttpListenerResponse response, string type, Stream stream)
                {
                        response.ContentType = type;
                        response.ContentLength64 = stream.Length;
                        response.ContentEncoding = Encoding.UTF8;
                        using (Stream output = response.OutputStream)
                        {
                                stream.CopyTo(output);
                                output.Flush();
                        }
                        response.Close();
                }
                private static void _WriteStream(HttpListenerResponse response, string type, Stream stream, long begin, long end)
                {
                        long len = end - begin;
                        response.ContentType = type;
                        response.ContentLength64 = len;
                        response.ContentEncoding = Encoding.UTF8;
                        using (stream)
                        {
                                stream.Position = begin;
                                using (Stream output = response.OutputStream)
                                {
                                        FileWriteHelper._Write(output, stream, len, _MinFileSize);
                                        output.Flush();
                                }
                        }
                }
                private static void _WriteStream(HttpListenerResponse response, Stream stream, string type, string range)
                {
                        long end = stream.Length;
                        string[] i = range.Split('-');
                        if (i[1] != string.Empty)
                        {
                                end = long.Parse(i[1]) + 1;
                        }
                        else
                        {
                                i[1] = (stream.Length - 1).ToString();
                        }
                        long begin = i[0] == string.Empty ? stream.Length - end : long.Parse(i[0]);
                        response.StatusCode = 206;
                        response.Headers.Add("Accept-Ranges", "bytes");
                        response.Headers.Add("Content-Range", string.Format("bytes {0}-{1}/{2}", i[0], i[1], stream.Length));
                        FileWriteHelper._WriteStream(response, type, stream, begin, end);
                }
                private static void _WriteStream(HttpListenerResponse response, Stream stream, string type, string[] ranges)
                {
                        FilePage page = FileWriteHelper._GetFilePage(stream, type, ranges);
                        response.StatusCode = 206;
                        response.Headers.Add("Accept-Ranges", "bytes");
                        response.Headers.Add("Content-Type", string.Concat("multipart/byteranges; boundary=", page.boundary));
                        response.ContentLength64 = page.Size;
                        response.ContentEncoding = Encoding.UTF8;
                        using (Stream output = response.OutputStream)
                        {
                                page.pages.ForEach(a =>
                                {
                                        stream.Position = a.Begin;
                                        output.Write(a.Head, 0, a.Head.Length);
                                        FileWriteHelper._Write(output, stream, a.Len, _MinFileSize);
                                });
                                output.Write(page.end, 0, page.end.Length);
                                output.Flush();
                        }
                }
                private static FilePage _GetFilePage(Stream stream, string type, string[] ranges)
                {
                        string boundary = Guid.NewGuid().ToString("N").Substring(0, 17).ToLower();
                        FilePage page = new FilePage
                        {
                                len = stream.Length,
                                boundary = boundary,
                                type = type
                        };
                        byte[] end = Encoding.UTF8.GetBytes(string.Concat("--", boundary, "--"));
                        long sum = end.Length;
                        page.pages = ranges.ConvertAll(a => _GetBranchPage(a, page, ref sum));
                        page.Size = sum;
                        page.end = end;
                        return page;
                }
                private static BranchPage _GetBranchPage(string range, FilePage page, ref long sum)
                {
                        long end = page.len;
                        string[] i = range.Split('-');
                        if (i[1] != string.Empty)
                        {
                                end = long.Parse(i[1]) + 1;
                        }
                        else
                        {
                                i[1] = (page.len - 1).ToString();
                        }
                        long begin = i[0] == string.Empty ? page.len - end : long.Parse(i[0]);
                        long size = end - begin;
                        byte[] head = Encoding.UTF8.GetBytes(string.Format("--{0}\r\nContent-Type:{1}\r\nContent-Range:bytes {2}-{3}/{4}\r\n", page.boundary, page.type, i[0], i[1], page.len));
                        sum += size + head.Length;
                        return new BranchPage
                        {
                                Head = head,
                                Begin = begin,
                                Len = size,
                                End = end
                        };
                }
                private static void _Write(Stream stream, Stream file, long total, long size)
                {
                        int num = (int)Math.Ceiling((decimal)total / size);
                        if (num == 1)
                        {
                                byte[] myByte = new byte[total];
                                file.Read(myByte, 0, myByte.Length);
                                stream.Write(myByte, 0, myByte.Length);
                                return;
                        }
                        num -= 1;
                        for (int i = 0; i <= num; i++)
                        {
                                if (i == num)
                                {
                                        size = total - (i * size);
                                }
                                byte[] myByte = new byte[size];
                                file.Read(myByte, 0, myByte.Length);
                                stream.Write(myByte, 0, myByte.Length);
                        }
                }


                internal static void WriteFile(HttpListenerResponse response, FileInfo file, string range)
                {
                        string type = RequestHeader.GetHeader(file.Extension);
                        using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                                WriteStream(response, stream, type, range);
                        }
                        response.Close();
                }

        }
}
