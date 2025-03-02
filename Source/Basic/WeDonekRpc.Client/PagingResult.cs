using System;

namespace WeDonekRpc.Client
{
    public class PagingResult<T>
    {
        public PagingResult()
        {

        }
        public PagingResult(int count)
        {
            this.Count = count;
        }
        public PagingResult(int count, T[] list)
        {
            this.Count = count;
            this.List = list;
        }
        public PagingResult(T[] list, int count)
        {
            this.Count = count;
            this.List = list;
        }
        public int Count
        {
            get;
            set;
        }
        public T[] List
        {
            get;
            set;
        }
    }
    public class PagingResultTo<T, To> : PagingResult<To> where T : class
    {
        public PagingResultTo()
        {

        }
        public PagingResultTo(int count, T[] list) : base(count, list.ConvertMap<T, To>())
        {
        }
        public PagingResultTo(int count, T[] list, Func<T, To, To> func) : base(count, list.ConvertMap<T, To>(func))
        {
        }
    }
    public class PagingResult<T, Extend> : PagingResult<T>
    {
        public PagingResult()
        {

        }
        public PagingResult(int count, T[] list) : base(count, list)
        {
        }
        public Extend Arg
        {
            get;
            set;
        }
    }
}
