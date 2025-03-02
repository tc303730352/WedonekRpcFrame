using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.ApiGateway.Helper;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.IpBlack.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;

namespace WeDonekRpc.ApiGateway.IpBlack
{
    internal class LocalIpBlack : IIpBlack
    {
        private readonly ReadWriteLockHelper _Ip4Lock = new ReadWriteLockHelper();
        private readonly ReadWriteLockHelper _Ip6Lock = new ReadWriteLockHelper();

        private readonly Dictionary<string, FileCache> _FileTimeCache = [];
        private Timer _SyncTimer = null;
        private DirectoryInfo _Directory;
        private IpBlackLocal _Config;

        private readonly HashSet<long> _IpBack = [];
        private readonly HashSet<BigInteger> _Ip6Back = [];
        private readonly List<IpBlackTo> _IpRanges = [];
        //是否启用
        private volatile bool _IsEnable = false;
        //是否正在同步目录
        private volatile bool _IsSyncDir = false;
        private volatile int _VerNum = 0;
        public bool IsInit => this._IsEnable;

        public void Init (IIpBlackConfig config)
        {
            IpBlackLocal local = config.Local;
            if (this._Config == null)
            {
                this._Config = local;
                _ = Task.Run(this._LoadFile);
            }
            else if (local.DirPath != this._Config.DirPath)
            {
                this._Config = local;
                this._SyncTimer.Dispose();
                this._IsEnable = false;
                _ = Task.Run(this._LoadFile);
            }
            else if (local.SyncTime != this._Config.SyncTime)
            {
                this._Config = local;
                this._SyncTimer?.Dispose();
                int time = this._Config.SyncTime * 1000;
                this._SyncTimer = new Timer(this._SyncDir, null, time, time);
            }
        }
        private void _LoadFile ()
        {
            this._VerNum = Tools.GetRandom();
            string path = Path.Combine(AppContext.BaseDirectory, this._Config.DirPath);
            this._Directory = new DirectoryInfo(path);
            if (this._Directory.Exists)
            {
                IpBlackHelper helper = new IpBlackHelper();
                FileBlack[] files = helper.LoadDirFile(this._Directory);
                this._FileTimeCache.Clear();
                files.ForEach(a => this._FileTimeCache.Add(a.File.Name, a.File));
                bool isEnable = false;
                if (this._LoadIp4(files, true))
                {
                    isEnable = true;
                }
                if (this._LoadIp6(files, true))
                {
                    isEnable = true;
                }
                this._IsEnable = isEnable;
            }
            else
            {
                this._Directory.Create();
            }
            int time = this._Config.SyncTime * 1000;
            this._SyncTimer = new Timer(this._SyncDir, null, time, time);
        }
        private void _SyncDir (object state)
        {
            if (this._IsSyncDir)
            {
                return;
            }
            this._IsSyncDir = true;
            int ver = this._VerNum;
            IpBlackHelper helper = new IpBlackHelper();
            FileBlack[] blacks = helper.SyncDirFile(this._Directory, this._FileTimeCache);
            if (blacks.IsNull())
            {
                this._IsSyncDir = false;
                return;
            }
            else if (this._VerNum != ver)//配置已经更改 
            {
                return;
            }
            bool isEnable = false;
            if (this._LoadIp4(blacks))
            {
                isEnable = true;
            }
            if (this._LoadIp6(blacks))
            {
                isEnable = true;
            }
            this._IsEnable = isEnable;
            this._IsSyncDir = false;
        }
        private bool _LoadIp4 (FileBlack[] blacks, bool isReset = false)
        {
            int sum = blacks.Select(c => c.ip4.Count).Sum();
            int range = blacks.Select(c => c.black.Count).Sum();
            if (this._Ip4Lock.Write.GetLock())
            {
                using (this._Ip4Lock.Write)
                {
                    if (isReset)
                    {
                        this._IpRanges.Clear();
                        this._IpBack.Clear();
                        _ = this._IpBack.EnsureCapacity(sum);
                        _ = this._IpRanges.EnsureCapacity(range);
                        blacks.ForEach(a =>
                        {
                            a.ip4.ForEach(c => this._IpBack.Add(c));
                            a.black.ForEach(c =>
                            {
                                if (!c.IsDrop)
                                {
                                    this._IpRanges.Add(c);
                                }
                            });
                        });
                    }
                    else
                    {
                        blacks.ForEach(a =>
                        {
                            a.dropIp4.ForEach(c => this._IpBack.Remove(c));
                            a.ip4.ForEach(c => this._IpBack.Add(c));
                            a.black.ForEach(c =>
                            {
                                int index = this._IpRanges.FindIndex(e => e == c);
                                if (index == -1 && !c.IsDrop)
                                {
                                    this._IpRanges.Add(c);
                                }
                                else if (c.IsDrop && index != -1)
                                {
                                    this._IpRanges.RemoveAt(index);
                                }
                            });
                        });
                    }
                    return this._IpBack.Count > 0 || this._IpRanges.Count > 0;
                }
            }
            return false;
        }
        private bool _LoadIp6 (FileBlack[] blacks, bool isReset = false)
        {
            int sum = blacks.Select(c => c.ip6.Count).Sum();
            if (this._Ip6Lock.Write.GetLock())
            {
                using (this._Ip6Lock.Write)
                {
                    if (isReset)
                    {
                        this._Ip6Back.Clear();
                        _ = this._Ip6Back.EnsureCapacity(sum);
                        blacks.ForEach(a =>
                        {
                            a.ip6.ForEach(c => this._Ip6Back.Add(c));
                        });
                    }
                    else
                    {
                        blacks.ForEach(a =>
                        {
                            a.dropIp6.ForEach(c => this._Ip6Back.Remove(c));
                            a.ip6.ForEach(c => this._Ip6Back.Add(c));
                        });
                    }
                    return this._Ip6Back.Count > 0;
                }
            }
            return false;
        }
        public bool IsLimit (string ip)
        {
            IPAddress address = IPAddress.Parse(ip);
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return this._Ip4IsLimit(address.Address);
            }
            return this._Ip6IsLimit(new BigInteger(address.GetAddressBytes()));
        }
        private bool _Ip6IsLimit (BigInteger ip)
        {
            if (this._Ip6Lock.GetReadLock())
            {
                using (this._Ip6Lock.Read)
                {
                    if (this._Ip6Back.Contains(ip))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool _Ip4IsLimit (long ip)
        {
            if (this._Ip4Lock.GetReadLock())
            {
                using (this._Ip4Lock.Read)
                {
                    if (this._IpBack.Contains(ip))
                    {
                        return true;
                    }
                    else if (this._IpRanges.Count > 0)
                    {
                        foreach (IpBlackTo i in this._IpRanges)
                        {
                            if (i.IsLimit(ip))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public void Dispose ()
        {
            this._Ip4Lock.Dispose();
            this._Ip6Lock.Dispose();

        }
    }
}
