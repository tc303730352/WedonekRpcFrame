using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using WeDonekRpc.ApiGateway.IpBlack.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.ApiGateway.Helper
{
    internal class IpBlackHelper
    {
        private readonly bool _IgnoreDrop = false;
        public IpBlackHelper ( bool ignoreDrop )
        {
            this._IgnoreDrop = ignoreDrop;
        }
        public IpBlackHelper ()
        {
        }
        public bool WriteCacheFile ( FileInfo file, HashSet<RemoteIpBlackTo> range, HashSet<long> ips, HashSet<BigInteger> ip6, long ver )
        {
            int size = 20 + ( ips.Count * 5 ) + ( range.Count * 9 ) + ( ip6.Count * 17 );
            byte[] stream = new byte[size];
            int i = BitHelper.WriteByte(ver, stream, 0);
            _ = BitHelper.WriteByte(ips.Count, stream, 8);
            _ = BitHelper.WriteByte(ip6.Count, stream, 12);
            _ = BitHelper.WriteByte(range.Count, stream, 16);
            i = 20;
            ips.ForEach(c =>
            {
                stream[i++] = 1;
                i += BitHelper.WriteByte((uint)c, stream, i);
            });
            ip6.ForEach(c =>
            {
                stream[i++] = 2;
                i += BitHelper.WriteByte(c, stream, i);
            });
            range.ForEach(a =>
            {
                stream[i++] = 3;
                i += BitHelper.WriteByte(a.BeginIp, stream, i);
                i += BitHelper.WriteByte(a.EndIp, stream, i);
            });
            try
            {
                ZipTools.CompressionFile(stream, file);
                return true;
            }
            catch ( Exception e )
            {
                new ErrorLog(e, "ApiGateway").Save();
                return false;
            }
        }
        public void LoadCacheFile ( FileInfo file, HashSet<RemoteIpBlackTo> range, HashSet<long> ips, HashSet<BigInteger> ip6, out long ver )
        {
            byte[] bytes = ZipTools.DecompressionFile(file);
            ver = BitConverter.ToInt64(bytes, 0);
            if ( bytes.Length == 20 )
            {
                return;
            }
            Span<byte> datas = bytes.AsSpan();
            int ip4n = Unsafe.ReadUnaligned<int>(ref datas[8]);
            int ip6n = Unsafe.ReadUnaligned<int>(ref datas[12]);
            int blackn = Unsafe.ReadUnaligned<int>(ref datas[16]);
            _ = range.EnsureCapacity(blackn);
            _ = ips.EnsureCapacity(ip4n);
            _ = ip6.EnsureCapacity(ip6n);
            int i = 20;
            while ( i < bytes.Length )
            {
                int val = bytes[i];
                i += 1;
                if ( val == 1 )
                {
                    uint ip = (uint)Unsafe.ReadUnaligned<int>(ref bytes[i]);
                    _ = ips.Add(ip);
                    i += 4;
                }
                else if ( val == 2 )
                {
                    _ = ip6.Add(new BigInteger(datas.Slice(i, 16)));
                    i += 16;
                }
                else if ( val == 3 )
                {
                    uint begin = (uint)Unsafe.ReadUnaligned<int>(ref bytes[i]);
                    i += 4;
                    uint end = (uint)Unsafe.ReadUnaligned<int>(ref bytes[i]);
                    i += 4;
                    _ = range.Add(new RemoteIpBlackTo(begin, end));
                }
            }
        }
        private FileBlack _ReadFile ( FileInfo file )
        {
            FileBlack back = new FileBlack
            {
                File = new FileCache(file)
            };
            Tools.ReadTextLine(file, Encoding.UTF8, line =>
           {
               line = line.Trim();
               if ( line != string.Empty && line.Length > 7 )
               {
                   if ( line.StartsWith('-') )
                   {
                       if ( this._IgnoreDrop )
                       {
                           return;
                       }
                       this._LoadText(line.Remove(0, 1), back, true);
                   }
                   else
                   {
                       this._LoadText(line, back, false);
                   }
               }
           });
            return back;
        }

        private void _LoadText ( string res, FileBlack list, bool isDrop )
        {
            int i = res.IndexOf(',');
            if ( i == -1 )
            {
                if ( !IPAddress.TryParse(res, out IPAddress ip) )
                {
                    return;
                }
                else if ( ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
                {
                    if ( isDrop )
                    {
                        list.dropIp4.Add(ip.Address);
                    }
                    else
                    {
                        list.ip4.Add(ip.Address);
                    }
                }
                else if ( ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 )
                {
                    if ( isDrop )
                    {
                        list.dropIp6.Add(new BigInteger(ip.GetAddressBytes()));
                    }
                    else
                    {
                        list.ip6.Add(new BigInteger(ip.GetAddressBytes()));
                    }
                }
            }
            else if ( IPAddress.TryParse(res.Substring(0, i), out IPAddress bIp)
                && IPAddress.TryParse(res.Substring(i + 1), out IPAddress eIp)
                && bIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                && eIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
            {
                list.black.Add(new IpBlackTo(bIp, eIp) { IsDrop = isDrop });
            }
        }
        internal FileBlack[] LoadDirFile ( DirectoryInfo dir )
        {
            FileInfo[] files = dir.GetFiles("*.black", SearchOption.TopDirectoryOnly);
            if ( files.Length == 0 )
            {
                return Array.Empty<FileBlack>();
            }
            return files.ParallelConvertAll(this._ReadFile);
        }

        internal FileBlack[] SyncDirFile ( DirectoryInfo dir, Dictionary<string, FileCache> fileCache )
        {
            FileInfo[] files = dir.GetFiles("*.black", SearchOption.TopDirectoryOnly);
            if ( files.Length == 0 )
            {
                return null;
            }
            FileCache[] caches = fileCache.Values.ToArray();
            List<FileInfo> blacks = new List<FileInfo>(files);
            foreach ( FileCache cache in caches )
            {
                FileState state = cache.CheckState(files, out FileInfo file);
                if ( state == FileState.已删除 )
                {
                    _ = fileCache.Remove(cache.Name);
                }
                else if ( state == FileState.无更改 )
                {
                    _ = blacks.Remove(file);
                }
            }
            if ( blacks.Count == 0 )
            {
                return null;
            }
            blacks.ForEach(a =>
            {
                if ( !fileCache.ContainsKey(a.Name) )
                {
                    fileCache.Add(a.Name, new FileCache(a));
                }
            });
            return blacks.ParallelToArray(this._ReadFile);
        }
    }
}
