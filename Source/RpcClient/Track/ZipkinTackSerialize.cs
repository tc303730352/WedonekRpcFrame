using System;
using System.Text;

using RpcClient.Interface;
using RpcClient.Track.Model;

using RpcModel.Model;

using RpcHelper;

namespace RpcClient.Track
{
        internal class ZipkinTackSerialize : ITackSerialize
        {
                private const char openBrace = '{';
                private const char closeBrace = '}';
                private const char comma = ',';
                private const string _Endpoint = "endpoint";
                private const string _Value = "value";
                private const string _BinaryAnnotations = "binaryAnnotations";
                private const string _Timestamp = "timestamp";
                private const string _ServiceName = "serviceName";
                private const string _Duration = "duration";
                private const string _Id = "id";
                private const string _Key = "key";
                private const string _LocalEndpoint = "localEndpoint";
                private const string _Name = "name";
                private const string _Debug = "debug";
                private const string _TraceId = "traceId";
                private const string _ParentId = "parentId";
                private const string _Ipv4 = "ipv4";
                private const string _Port = "port";

                private readonly StringBuilder _Json = new StringBuilder(1024 * 10);
                private static string _ServerName;
                public ZipkinTackSerialize(string serverName)
                {
                        _ServerName = serverName;
                }
                public byte[] Serialize(TrackBody[] tracks)
                {
                        this._Json.Append('[');
                        tracks.ForEach(a =>
                        {
                                TrackSpan track = a.Trace;
                                this._Json.Append(openBrace);
                                _WriteField(this._Json, _Id, track.SpanId.ToString("x16"));
                                _AppendField(this._Json, _Name, a.Dictate);
                                _AppendField(this._Json, _Debug, false);
                                _AppendField(this._Json, _TraceId, track.ToTraceId());
                                _AppendField(this._Json, _Timestamp, a.Time);
                                _AppendField(this._Json, _Duration, a.Duration);
                                if (track.ParentId.HasValue)
                                {
                                        _AppendField(this._Json, _ParentId, track.ParentId.Value.ToString("x16"));
                                }
                                this._WriteAnnotation(a);
                                this._WriteArg(a);
                                this._Json.Append(closeBrace);
                                this._Json.Append(comma);
                        });
                        this._Json.Remove(this._Json.Length - 1, 1);
                        this._Json.Append(']');
                        return Encoding.UTF8.GetBytes(this._Json.ToString());
                }
                private void _WriteArg(TrackBody task)
                {
                        if (task.Args.IsNull())
                        {
                                return;
                        }
                        this._Json.Append(comma);
                        _WriteArray(this._Json, _BinaryAnnotations, task.Args, a =>
                        {
                                if ((a.Key == "Param" || a.Key == "Return"))
                                {
                                        a.Value = _FormatValue(a.Value);
                                }
                                else if (a.Value == null)
                                {
                                        a.Value = string.Empty;
                                }
                                _WriteArg(this._Json, task, a);
                        });
                }
                private static string _FormatValue(string str)
                {
                        if (str == null)
                        {
                                return string.Empty;
                        }
                        else if (str.IndexOf("\"") == -1)
                        {
                                return str;
                        }
                        StringBuilder json = new StringBuilder(str);
                        json.Replace("\"", "\\\"");
                        json.Replace("\\\\\"", "\\\"");
                        return json.ToString();
                }
                private void _WriteAnnotation(TrackBody task, TrackAnnotation an)
                {
                        _WriteField(this._Json, _Timestamp, an.Time);
                        _AppendField(this._Json, _Value, an.Stage.ToString());
                        _AppendIp(this._Json, _Endpoint, task);
                }
                private void _WriteAnnotation(TrackBody track)
                {
                        if (track.Annotations.Length > 0)
                        {
                                this._Json.Append(comma);
                                _WriteArray(this._Json, "annotations", track.Annotations, a =>
                                {
                                        this._WriteAnnotation(track, a);
                                });
                        }
                }
                #region 静态方法
                private static void _WriteArg(StringBuilder stream, TrackBody task, TrackArg an)
                {
                        _WriteField(stream, _Key, an.Key);
                        _AppendField(stream, _Value, an.Value);
                        _AppendIp(stream, _Endpoint, task);
                }
                private static void _AppendIp(StringBuilder stream, string name, TrackBody task)
                {
                        stream.Append(comma);
                        _WriteName(stream, name);
                        stream.Append(openBrace);
                        _WriteField(stream, _Ipv4, task.RemoteIp);
                        _AppendField(stream, _Port, task.Port);
                        _AppendField(stream, _ServiceName, task.ServerName ?? _ServerName);
                        stream.Append(closeBrace);
                }

                private static void _WriteArray<T>(StringBuilder stream, string name, T[] array, Action<T> action)
                {
                        _WriteName(stream, name);
                        stream.Append('[');
                        array.ForEach(a =>
                        {
                                stream.Append(openBrace);
                                action(a);
                                stream.Append(closeBrace);
                                stream.Append(comma);
                        });
                        stream.Remove(stream.Length - 1, 1);
                        stream.Append("]");
                }
                private static void _WriteField(StringBuilder stream, string name, DateTime time)
                {
                        _WriteField(stream, name, time.ToMilliseconds());
                }
                private static void _AppendField(StringBuilder stream, string name, DateTime time)
                {
                        stream.Append(comma);
                        _WriteField(stream, name, time.ToMilliseconds());
                }
                private static void _AppendField(StringBuilder stream, string name, bool val)
                {
                        stream.Append(comma);
                        _WriteField(stream, name, val);
                }
                private static void _WriteField(StringBuilder stream, string name, long value)
                {
                        _WriteName(stream, name);
                        stream.Append(value);
                }
                private static void _WriteField(StringBuilder stream, string name, bool value)
                {
                        _WriteName(stream, name);
                        stream.Append(value ? "true" : "false");
                }
                private static void _AppendField(StringBuilder stream, string name, long value)
                {
                        stream.Append(comma);
                        _WriteName(stream, name);
                        stream.Append(value);

                }
                private static void _WriteValue(StringBuilder stream, string str)
                {
                        stream.Append('\"');
                        stream.Append(str);
                        stream.Append('\"');
                }

                private static void _WriteName(StringBuilder stream, string name)
                {
                        stream.Append('\"');
                        stream.Append(name);
                        stream.Append('\"');
                        stream.Append(':');
                }
                private static void _AppendField(StringBuilder stream, string name, string value)
                {
                        stream.Append(comma);
                        _WriteName(stream, name);
                        _WriteValue(stream, value);

                }
                private static void _WriteField(StringBuilder stream, string name, string value)
                {
                        _WriteName(stream, name);
                        _WriteValue(stream, value);

                }
                #endregion
        }
}
