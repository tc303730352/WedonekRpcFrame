using System.Linq.Expressions;
using SqlSugar;
using WeDonekRpc.Model;

namespace WeDonekRpc.SqlSugar
{
    public interface IRepository<T> where T : class, new()
    {
        ILocalTransaction ApplyTran (bool isInherit = true);
        bool IsTable (string tableName);
        void CreateTable (string tableName);
        void CreateTable ();
        string GetKey ();
        string[] GetKeys ();
        bool Delete (T data);
        bool DeleteByKey<IdentityId> (IdentityId id);
        bool DeleteByKey<IdentityId> (IdentityId[] ids);
        bool UpdateOnly (T[] datas, Expression<Func<T, object>> setCol);
        Result[] Gets<Result> (Expression<Func<T, Result>> selector, IBasicPage paging);
        Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list, string selector, string orderBy) where Table : class, new();
        Result Get<Table, Result> (ISugarQueryable<Table>[] list, string selector, string orderBy) where Table : class, new();
        Result[] Gets<T, Result> (ISugarQueryable<T>[] list, string selector) where T : class, new();
        SqlSugarProvider Provider { get; }
        ISugarQueryable<T> Queryable { get; }
        ISqlQueue<T> BeginQueue ();
        Result Get<Result> (ISugarQueryable<Result>[] list, string orderBy) where Result : class, new();
        Result[] Gets<Result> (ISugarQueryable<Result>[] list, string orderBy) where Result : class, new();
        Result[] Gets<Result> (ISugarQueryable<Result>[] list) where Result : class, new();
        Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list, Expression<Func<Table, Result>> selector, string orderBy) where Table : class, new();
        Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list) where Table : class, new();
        Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list, Expression<Func<Table, Result>> selector) where Table : class, new();
        Result Get<Table, Result> (ISugarQueryable<Table>[] list, Expression<Func<Table, Result>> selector, string orderBy) where Table : class, new();
        Result Avg<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector);

        Result Max<Result> (Expression<Func<T, Result>> selector);
        Result Min<Result> (Expression<Func<T, Result>> selector);

        Result Avg<Result> (Expression<Func<T, Result>> selector);

        int Count ();
        int Count (Expression<Func<T, bool>> filter);
        int Count (string table, Expression<Func<T, bool>> filter);
        int Count (string table);
        bool Delete (Expression<Func<T, bool>> filter);
        bool Delete (Expression<Func<T, bool>> filter, Expression<Func<T, object>> selector, int num);
        IDeleteable<T> Delete ();
        bool Delete<Other> (Expression<Func<T, Other, bool>> filter) where Other : class, new();
        int ExecuteSql (string sql, params SugarParameter[] param);
        T Get (Expression<Func<T, bool>> filter);
        T GetById<IdentityId> (IdentityId id);
        Result GetById<IdentityId, Result> (IdentityId id) where Result : class;
        Result GetById<IdentityId, Result> (IdentityId id, Expression<Func<T, Result>> selector);

        T Get (Expression<Func<T, bool>> filter, string orderby);
        Result Get<Result> (Expression<Func<T, Result>> selector, string orderby);
        T Get (string sql, params SugarParameter[] param);
        Result Get<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector);
        Result Get<Result> (Expression<Func<T, bool>> filter) where Result : class;
        Result Get<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector, string orderby);
        T[] GetAll ();
        T[] GetAll (string orderby);
        Result[] GetAll<Result> (Expression<Func<T, Result>> selector);
        Result[] GetAll<Result> () where Result : class, new();

        Result[] GetAll<Result> (Expression<Func<T, Result>> selector, string orderby);

        Result[] GetAll<Result> (Expression<Func<T, Result>> selector, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc);
        T[] Gets (Expression<Func<T, bool>> filter);
        T[] Gets (Expression<Func<T, bool>> filter, string orderby);
        T[] Gets (Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc);

        Result[] Gets<Result> (Expression<Func<T, bool>> filter) where Result : class, new();

        Result[] Gets<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector);

        Result[] Gets<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector, string orderby);

        Result[] Gets<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc);

        Result[] Gets<Result> (Expression<Func<T, bool>> filter, string orderby) where Result : class, new();

        Result[] Gets<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc);
        Result[] Gets<Result> (string sql, params SugarParameter[] param) where Result : class, new();

        T[] GetsById<IdentityId> (IdentityId[] ids);

        Result[] GetsById<IdentityId, Result> (IdentityId[] ids) where Result : class;

        Result[] GetsById<IdentityId, Result> (IdentityId[] ids, Expression<Func<T, Result>> selector);

        Result[] GetsById<IdentityId, Result> (IdentityId[] ids, Expression<Func<T, Result>> selector, string orderBy);

        Result[] GetsById<IdentityId, Result> (IdentityId[] ids, Expression<Func<T, Result>> selector, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc);

        Result[] GroupByQuery<Result> ( Expression<Func<T, bool>> filter, Expression<Func<T, object>> group, Expression<Func<T, Result>> selector, IBasicPage pager, out int count );
        Result[] GroupBy<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, object>> group, Expression<Func<T, bool>> having, Expression<Func<T, Result>> selector);
        Result[] GroupBy<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, object>> group, Expression<Func<T, Result>> selector);

        Result[] GroupBy<Result> (Expression<Func<T, object>> group, Expression<Func<T, Result>> selector);
        void Insert (T item);
        int InsertTable (List<T> items);
        void Insert (T item, string tableName);
        void Insert (T[] items, string tableName);
        IdentityId Insert<IdentityId> (T item);
        long InsertReutrnIdentity (T item);
        void Insert (T[] items);
        void Insert<Table> (Table item) where Table : class, new();
        void Insert<Table> (Table[] items) where Table : class, new();

        void Insert (List<T> items);

        int Insert (List<T> datas, Expression<Func<T, object>> filter);

        bool IsExist (Expression<Func<T, bool>> filter);

        bool IsExist ();

        Result[] JoinGroupBy<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
       Expression<Func<T, TInner, object>> group,
       Expression<Func<T, TInner, Result>> selector) where TInner : class;

        Result[] JoinGroupBy<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
        Expression<Func<T, TInner, bool>> filter,
        Expression<Func<T, TInner, object>> group,
        Expression<Func<T, TInner, Result>> selector) where TInner : class;

        Result JoinGet<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression, Expression<Func<T, TInner, bool>> filter, Expression<Func<T, TInner, Result>> selector) where TInner : class;
        Result JoinGet<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression, Expression<Func<T, TInner, Result>> selector) where TInner : class;
        Result[] Join<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression, Expression<Func<T, TInner, Result>> selector) where TInner : class;
        Result[] Join<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression, Expression<Func<T, TInner, bool>> filter, Expression<Func<T, TInner, Result>> selector) where TInner : class;
        Result[] JoinGroupBy<TInner, TKey, TResult, Result, GroupKey> (Expression<Func<T, TInner, bool>> joinExpression, Expression<Func<T, TInner, bool>> filter, Expression<Func<T, TInner, object>> group, Expression<Func<T, TInner, Result>> selector) where TInner : class;
        Result[] JoinQuery<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression, Expression<Func<T, TInner, bool>> filter, Expression<Func<T, TInner, Result>> selector, IBasicPage pager, out int count) where TInner : class;
        Result[] JoinQuery<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression, Expression<Func<T, TInner, bool>> filter, Expression<Func<T, TInner, Result>> selector, int index, int size, Expression<Func<T, TInner, object>> orderby, OrderByType byType, out int count) where TInner : class;
        Result[] JoinQuery<TInner, Result> (Expression<Func<T, TInner, bool>> join, Expression<Func<T, TInner, Result>> selector, IBasicPage pager, out int count) where TInner : class;
        Result Max<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector);
        Result Min<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector);
        T[] Query (Expression<Func<T, bool>> filter, IBasicPage pager, out int count);
        T[] Query (IBasicPage pager, out int count);
        Result[] Query<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector, IBasicPage pager, out int count);
        Result[] Query<Result> (Expression<Func<T, bool>> filter, IBasicPage pager, out int count) where Result : class;
        Result Sum<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector);
        bool Update (T data, string[] setColumn);
        bool Update (T data, string[] setColumn, Expression<Func<T, bool>> filter);
        bool Update (T[] datas, params string[] setColumn);
        bool Update (string table, T data, params string[] setColumn);
        IUpdateable<T> Update (T data);
        IUpdateable<T> Update ();
        IUpdateable<T> Update<Set> (Set set);

        /// <summary>
        /// 只能修改单列
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="filter"></param>
        bool Update (Expression<Func<T, bool>> columns, Expression<Func<T, bool>> filter);
        bool Update<Set> (Set set, Expression<Func<T, bool>> filter);
        bool Update<Set> (Set set, Expression<Func<T, object>> filter);

        bool Update<Set> (Set[] set) where Set : class, new();
        bool Update (T[] datas, Expression<Func<T, object>> filter);

        bool Update (Expression<Func<T, T>> columns, Expression<Func<T, bool>> filter);
        int UpdateByRowIndex (Expression<Func<T, T>> columns, Expression<Func<T, bool>> filter);
        bool Update (Expression<Func<T, T>> columns, string[] whereColumn);
        bool Update (Expression<Func<T, T>> columns, Expression<Func<T, object>> filter);

        bool Update (T source, T set);
        bool Update<Set> (T source, Set set);

        bool AddOrUpdate (List<T> datas);
        IStorageable<T> Storageable (List<T> datas);
        bool AddOrUpdate (List<T> datas, Expression<Func<T, object>> filter);
        bool AddOrUpdate (List<T> datas, string table, Expression<Func<T, object>> filter);
        bool AddOrUpdate (List<T> datas, Func<StorageableInfo<T>, bool> insert, Func<StorageableInfo<T>, bool> update);

        bool AddOrUpdate (List<T> datas, Expression<Func<T, object>> filter, Expression<Func<T, object>> update);

    }
}