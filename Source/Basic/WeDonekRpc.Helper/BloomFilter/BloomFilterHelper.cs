using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;

namespace WeDonekRpc.Helper.BloomFilter
{
    public delegate string[] LoadData ();
    public class BloomFilterHelper
    {
        private const int _DefMinSize = 10000;
        public BloomFilterHelper ( LoadData load, bool isInit, int minSize = _DefMinSize ) : this(load, null, isInit, minSize)
        {

        }
        public BloomFilterHelper ( LoadData load, int minSize = _DefMinSize ) : this(load, null, false, minSize)
        {
        }
        public BloomFilterHelper ( LoadData load, Action<bool> complate, int minSize = _DefMinSize ) : this(load, complate, false, minSize)
        {

        }
        public BloomFilterHelper ( LoadData load, Action<bool> complate, bool isInit, int minSize )
        {
            this._MinSize = minSize;
            this._LoadAction = load;
            this._LoadComplate = complate;
            this._IsInit = isInit;
            if ( !this._IsInit )
            {
                this.LoadUser();
            }
        }
        private readonly Action<bool> _LoadComplate = null;
        private readonly HashSet<string> _AddData = new HashSet<string>();

        private readonly int accuracyRate = 1;

        private readonly int hashNum = 4;

        private readonly LockHelper _Lock = new LockHelper();

        private readonly LoadData _LoadAction = null;

        private BloomFilter _Filter = null;

        private int _MaxSize = 100000;
        private readonly int _MinSize = 10000;

        private int _Size = 0;
        private volatile bool _IsInit = false;
        private volatile bool _IsLoad = false;

        private volatile bool _IsLock = false;

        public int Size => this._Size;
        public bool IsLoad => this._IsLoad;

        public void LoadUser ()
        {
            if ( this._IsLock )
            {
                return;
            }
            this._IsLock = true;
            ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(( a ) =>
            {
                this._LoadUser();
            }), null);
        }
        private void _LoadUser ( int replyNum = 1 )
        {
            string[] datas = _LoadAction();
            this._LoadUser(datas);
            if ( this._LoadComplate != null && !this._IsInit )
            {
                this._IsInit = true;
                this._LoadComplate(true);
            }
        }
        private void _LoadUser ( string[] codes )
        {
            int size = (int)( codes.Length * 1.6 );
            if ( size < this._MinSize )
            {
                size = this._MinSize;
            }
            BloomFilter filter = new BloomFilter(size, this.accuracyRate, this.hashNum);
            if ( codes.Length > 0 )
            {
                Parallel.ForEach(codes, a => filter.Add(a));
            }
            this._MaxSize = size;
            Interlocked.Exchange(ref this._Size, codes.Length);
            this._Filter = filter;
            this._IsLoad = true;
            this._IsLock = false;
            if ( this._AddData.Count > 0 )
            {
                string[] vals = null;
                if ( this._Lock.GetLock() )
                {
                    vals = this._AddData.Distinct().ToArray();
                    this._AddData.Clear();
                    this._Lock.Exit();
                }
                Array.ForEach(vals, a =>
                {
                    this._Filter.Add(a);
                });
            }
        }

        public bool CheckIsFilter ( string val )
        {
            if ( this._IsLoad )
            {
                return this._Filter.Contains(val);
            }
            return true;
        }
        private void _AddItem ( string item )
        {
            if ( !this._IsLoad )
            {
                return;
            }
            if ( !this._Filter.Contains(item) )
            {
                this._Filter.Add(item);
                int size = Interlocked.Increment(ref this._Size);
                if ( size == this._MaxSize )
                {
                    this.LoadUser();
                }
            }
        }
        public void AddItem ( string item )
        {
            if ( this._IsLock )
            {
                if ( this._Lock.GetLock() )
                {
                    this._AddData.Add(item);
                    this._Lock.Exit();
                }
            }
            this._AddItem(item);
        }
    }
}
