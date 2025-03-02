using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.ApiGateway.Helper;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.IpBlack.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;
using WeDonekRpc.ModularModel.IpBlack.Model;

namespace WeDonekRpc.ApiGateway.IpBlack
{
    [IgnoreIoc]
    internal class RemoteIpBack : IIpBlack
    {
        private readonly ReadWriteLockHelper _ip4Lock = new ReadWriteLockHelper();
        private readonly ReadWriteLockHelper _ip6Lock = new ReadWriteLockHelper();

        private IpBlackRemote _Config;

        private IIpBackService _Service;

        private Timer _SyncTimer;

        private HashSet<long> _IpBack = null;

        private HashSet<BigInteger> _Ip6Back = null;

        private HashSet<RemoteIpBlackTo> _IpRanges = null;

        private volatile bool _IsInit = false;

        private int _IsLock = 0;

        private long _LocalVer = 0;

        public bool IsInit => this._IsInit;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        public void Init (IIpBlackConfig config)
        {
            IpBlackRemote remote = config.Remote;
            if (this._Config == null)
            {
                this._Service = RpcClient.Ioc.Resolve<IIpBackService>();
                this._Config = remote;
                _ = Task.Run(this._LoadFile);
                int time = this._Config.SyncVerTime * 1000;
                this._SyncTimer = new Timer(new TimerCallback(this._SyncVer), null, time, time);
            }
            else
            {
                if (this._Config.SyncVerTime != remote.SyncVerTime)
                {
                    int time = this._Config.SyncVerTime * 1000;
                    _ = this._SyncTimer.Change(time, time);
                }
                if (!this._Config.EnableCache && remote.EnableCache)
                {
                    this._Config = remote;
                    this._ResestCache();
                }
                else if (this._Config.EnableCache && !remote.EnableCache)
                {
                    string path = Path.Combine(AppContext.BaseDirectory, this._Config.CachePath, "backlist.data");
                    this._Config = remote;
                    this._DeleteCache(path);
                }
                else if (this._Config.CachePath != remote.CachePath && remote.EnableCache)
                {
                    string path = Path.Combine(AppContext.BaseDirectory, this._Config.CachePath, "backlist.data");
                    this._Config = remote;
                    this._MoveCache(path);
                }
            }

        }

        /// <summary>
        /// 移动黑名单本地缓存文件
        /// </summary>
        /// <param name="path"></param>
        private void _MoveCache (string path)
        {
            if (!this._LockData())
            {
                return;
            }
            if (File.Exists(path))
            {
                string to = Path.Combine(AppContext.BaseDirectory, this._Config.CachePath, "backlist.data");
                File.Move(path, to, true);
            }
            this._UnLockData();
        }
        /// <summary>
        /// 删除本地文件缓存
        /// </summary>
        /// <param name="path"></param>
        private void _DeleteCache (string path)
        {
            if (!this._LockData())
            {
                return;
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
            }
            this._UnLockData();
        }
        /// <summary>
        /// 重置本地缓存文件
        /// </summary>
        private void _ResestCache ()
        {
            if (!this._LockData())
            {
                return;
            }
            this._SaveFile();
            this._UnLockData();
        }
        /// <summary>
        /// 与远程服务器同步黑名单版本
        /// </summary>
        /// <param name="state"></param>
        private void _SyncVer (object state)
        {
            if (!this._LockData())
            {
                return;
            }
            this._SyncVer();
            this._UnLockData();
        }
        /// <summary>
        /// 锁定资源
        /// </summary>
        /// <returns></returns>
        private bool _LockData ()
        {
            return Interlocked.CompareExchange(ref this._IsLock, 1, 0) == 0;
        }
        /// <summary>
        /// 解锁
        /// </summary>
        private void _UnLockData ()
        {
            _ = Interlocked.Exchange(ref this._IsLock, 0);
        }
        /// <summary>
        ///同步远程名单版本
        /// </summary>
        /// <param name="state"></param>
        private void _SyncVer ()
        {
            IpBlackList black = this._Service.GetIpBlack(Interlocked.Read(ref this._LocalVer));
            if (black.Count == 0)
            {
                this._IsInit = this._IpBack.Count > 0 || this._IpRanges.Count > 0;
                return;
            }
            _ = Interlocked.Exchange(ref this._LocalVer, black.Ver);
            bool isEnable = false;
            if (this._LoadIp4(black))
            {
                isEnable = true;
            }
            if (this._LoadIp6(black))
            {
                isEnable = true;
            }
            this._IsInit = isEnable;
            if (this._Config.EnableCache && isEnable)
            {
                this._SaveFile();
            }
        }
        private bool _LoadIp4 (IpBlackList black)
        {
            bool isInit = false;
            if (this._ip4Lock.Write.GetLock())
            {
                using (this._ip4Lock.Write)
                {
                    black.DropIp4.ForEach(a =>
                    {
                        if (a.EndIp.HasValue)
                        {
                            _ = this._IpRanges.Remove(new RemoteIpBlackTo(a.Ip, a.EndIp.Value));
                            isInit = true;
                        }
                        else if (this._IpBack.Remove(a.Ip))
                        {
                            isInit = true;
                        }
                    });
                    black.Ip4.ForEach(a =>
                    {
                        if (this._IpBack.Add(a))
                        {
                            isInit = true;
                        }
                    });
                    black.Range.ForEach(a =>
                    {
                        _ = this._IpRanges.Add(new RemoteIpBlackTo(a.BeginIp, a.EndIp));
                        isInit = true;
                    });
                }
            }
            return isInit;
        }
        private bool _LoadIp6 (IpBlackList black)
        {
            decimal b = 1;
            BigInteger big = new BigInteger(b);
            bool isInit = false;
            if (this._ip6Lock.Write.GetLock())
            {
                using (this._ip6Lock.Write)
                {
                    black.DropIp6.ForEach(a =>
                    {
                        if (this._Ip6Back.Remove(a))
                        {
                            isInit = true;
                        }
                    });
                    black.Ip6.ForEach(a =>
                    {
                        if (this._Ip6Back.Add(a))
                        {
                            isInit = true;
                        }
                    });
                }
            }
            return isInit;
        }
        /// <summary>
        /// 保存缓存文件
        /// </summary>
        private void _SaveFile ()
        {
            string path = Path.Combine(AppContext.BaseDirectory, this._Config.CachePath, "backlist.data.lock");
            FileInfo file = new FileInfo(path);
            IpBlackHelper helper = new IpBlackHelper();
            if (helper.WriteCacheFile(file, this._IpRanges, this._IpBack, this._Ip6Back, this._LocalVer))
            {
                path = Path.Combine(AppContext.BaseDirectory, this._Config.CachePath, "backlist.data");
                file.Refresh();
                file.MoveTo(path);
            }
            else if (file.Exists)
            {
                file.Delete();
            }
        }
        /// <summary>
        /// 加载缓存文件
        /// </summary>
        private void _LoadFile ()
        {
            if (!this._LockData())
            {
                return;
            }
            if (this._Config.EnableCache)
            {
                string path = Path.Combine(AppContext.BaseDirectory, this._Config.CachePath, "backlist.data");
                FileInfo file = new FileInfo(path);
                HashSet<long> ips = [];
                HashSet<BigInteger> ip6 = [];
                HashSet<RemoteIpBlackTo> blacks = [];
                if (file.Exists)
                {
                    IpBlackHelper helper = new IpBlackHelper();
                    helper.LoadCacheFile(file, blacks, ips, ip6, out long ver);
                    _ = Interlocked.Exchange(ref this._LocalVer, ver);
                }
                if (this._ip4Lock.Write.GetLock())
                {
                    using (this._ip4Lock.Write)
                    {
                        this._IpRanges = blacks;
                        this._IpBack = ips;
                    }
                }
                if (this._ip6Lock.Write.GetLock())
                {
                    using (this._ip6Lock.Write)
                    {
                        this._Ip6Back = ip6;
                    }
                }
            }
            this._SyncVer();
            this._UnLockData();
        }

        public bool IsLimit (string ip)
        {
            IPAddress address = IPAddress.Parse(ip);
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return this._Ip4IsLimit(address.Address);
            }
            else
            {
                return this._Ip6IsLimit(new BigInteger(address.GetAddressBytes()));
            }
        }
        private bool _Ip6IsLimit (BigInteger ip)
        {
            if (this._ip6Lock.Read.GetLock())
            {
                using (this._ip6Lock.Read)
                {
                    return this._Ip6Back.Contains(ip);
                }
            }
            return false;
        }
        private bool _Ip4IsLimit (long ip)
        {
            if (this._ip4Lock.Read.GetLock())
            {
                using (this._ip4Lock.Read)
                {
                    if (this._IpBack.Contains(ip))
                    {
                        return true;
                    }
                    else if (this._IpRanges.Count > 0)
                    {
                        foreach (RemoteIpBlackTo i in this._IpRanges)
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
            this._SyncTimer?.Dispose();
            this._ip4Lock.Dispose();
        }
    }
}
