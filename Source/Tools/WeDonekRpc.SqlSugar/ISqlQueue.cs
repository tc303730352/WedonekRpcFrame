using System.Linq.Expressions;
using SqlSugar;

namespace WeDonekRpc.SqlSugar
{
    public interface ISqlQueue<T> : ISqlQueue where T : class, new()
    {
        /// <summary>
        /// 只能修改单列
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="filter"></param>
        void Update (T data, Expression<Func<T, object>> columns);

        void Update (Expression<Func<T, T>> columns, Expression<Func<T, bool>> filter);

        void Delete (Expression<Func<T, bool>> filter);
        void Delete<Other> (Expression<Func<T, Other, bool>> filter) where Other : class, new();
        void Insert (T item);
        void Insert (T[] items);
        void Update (T data, Expression<Func<T, bool>> filter);
        IUpdateable<T> Update ();
        IDeleteable<T> Delete ();
        void DeleteAll ();
        void Update (T data, params string[] setColumn);
        void Update (T data, string[] setColumn, Expression<Func<T, bool>> filter);
        void UpdateOnly (T[] datas, Expression<Func<T, object>> setCol);
        bool Update (T source, T set);
        void Update (T[] datas, params string[] setColumn);
        void Update<Set> (Set set, Expression<Func<T, bool>> filter);
        bool Update<Set> (T source, Set set);
        void Update (T[] datas, Expression<Func<T, object>> filter);
        void UpdateOneColumn (Expression<Func<T, bool>> columns, Expression<Func<T, bool>> filter);
    }
    public interface ISqlQueue
    {
        bool IsNull { get; }
        /// <summary>
        /// 只能修改单列
        /// </summary>
        /// <typeparam name="Set"></typeparam>
        /// <param name="columns"></param>
        /// <param name="filter"></param>
        void Update<Set> (Expression<Func<Set, bool>> columns, Expression<Func<Set, bool>> filter) where Set : class, new();
        IUpdateable<T> Update<T> () where T : class, new();
        IDeleteable<T> Delete<T> () where T : class, new();
        void Delete<T> (Expression<Func<T, bool>> filter) where T : class, new();
        void Delete<T, Other> (Expression<Func<T, Other, bool>> filter) where Other : class, new() where T : class, new();
        void Insert<T> (T item) where T : class, new();
        void Insert<T> (T[] items) where T : class, new();
        void Insert<Table> (List<Table> items) where Table : class, new();
        void Update<T> (Expression<Func<T, T>> columns, Expression<Func<T, bool>> filter) where T : class, new();
        void UpdateOneColumn<T> (Expression<Func<T, bool>> columns, Expression<Func<T, bool>> filter) where T : class, new();
        void Update<T> (T data, Expression<Func<T, object>> columns) where T : class, new();
        void Update<T> (T data, string[] setColumn) where T : class, new();
        void Update<T> (T data, string[] setColumn, Expression<Func<T, bool>> filter) where T : class, new();
        bool Update<T> (T source, T set) where T : class, new();
        void DeleteAll<T> () where T : class, new();
        void Update<T> (T[] datas, params string[] setColumn) where T : class, new();
        void Update<T, Set> (Set set, Expression<Func<T, bool>> filter) where T : class, new();
        bool Update<T, Set> (T source, Set set) where T : class, new();


        int Submit (bool isTran = true);
    }
}