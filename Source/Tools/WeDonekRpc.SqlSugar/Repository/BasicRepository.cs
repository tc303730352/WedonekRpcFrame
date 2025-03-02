using System.Linq.Expressions;
using SqlSugar;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar.Queue;
using WeDonekRpc.SqlSugar.Tran;

namespace WeDonekRpc.SqlSugar.Repository
{
    public class BasicRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly ISqlClientFactory _Factory;
        private readonly string _ConfigId;
        protected BasicRepository (ISqlClientFactory factory, string configId)
        {
            this._Factory = factory;
            this._ConfigId = configId;
        }
        private SqlSugarProvider _Provider;

        public SqlSugarProvider Provider
        {
            get
            {
                if (this._Provider == null)
                {
                    this._Provider = this._Factory.GetProvider(this._ConfigId);
                }
                return this._Provider;
            }
        }
        private Type _Type;
        private Type ModelType
        {
            get
            {
                if (this._Type == null)
                {
                    this._Type = typeof(T);
                }
                return this._Type;
            }
        }
        private EntityColumnInfo[] _KeyColumn;
        private EntityColumnInfo[] _GetKeyColumn ()
        {
            if (this._KeyColumn == null)
            {
                this._KeyColumn = this.Provider.Context.EntityMaintenance.GetEntityInfo(this.ModelType).Columns.ToArray(c => c.IsPrimarykey);
            }
            if (this._KeyColumn.Length == 0)
            {
                throw new ErrorException("db.model.not.find.key");
            }
            return this._KeyColumn;
        }

        private string[] _CurKey;
        public string GetKey ()
        {
            string[] keys = this.GetKeys();
            if (keys.Length > 1)
            {
                throw new ErrorException("db.model.have.multiple.key");
            }
            return keys[0];
        }
        public string[] GetKeys ()
        {
            if (this._CurKey == null)
            {
                EntityColumnInfo[] cols = this._GetKeyColumn();
                this._CurKey = cols.Convert(c => c.IsPrimarykey, a => a.PropertyName);
            }
            return this._CurKey;
        }
        public ILocalTransaction ApplyTran (bool isInherit = true)
        {
            return new LocalTransaction(this._Factory, isInherit);
        }
        public bool IsTable (string tableName)
        {
            return this.Provider.DbMaintenance.IsAnyTable(tableName);
        }
        public void CreateTable ()
        {
            this.Provider.CodeFirst.InitTables<T>();
        }
        public void CreateTable (string tableName)
        {
            this.Provider.CodeFirst.As<T>(tableName).InitTables<T>();
        }
        public bool Delete (T data)
        {
            return this.Provider.Deleteable(data).ExecuteCommandHasChange();
        }
        public bool DeleteByKey<IdentityId> (IdentityId data)
        {
            return this.Provider.Deleteable<T>().In(data).ExecuteCommandHasChange();
        }
        public bool DeleteByKey<IdentityId> (IdentityId[] ids)
        {
            return this.Provider.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }
        public ISqlQueue<T> BeginQueue ()
        {
            return new LocalQueue<T>(this.Provider);
        }
        public ISugarQueryable<T> Queryable
        {
            get => this._Factory.GetQueryable<T>(this._ConfigId);
        }
        private ISugarQueryable<T> _GetQueryable (Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                return this.Queryable;
            }
            return this.Queryable.Where(filter);
        }

        public void Insert (T item)
        {
            _ = this.Provider.Insertable(item).ExecuteCommand();
        }
        public void Insert (T item, string tableName)
        {
            _ = this.Provider.Insertable(item).AS(tableName).ExecuteCommand();
        }
        public void Insert (T[] items, string tableName)
        {
            _ = this.Provider.Insertable(items).AS(tableName).ExecuteCommand();
        }
        public IdentityId Insert<IdentityId> (T item)
        {
            EntityColumnInfo key = this._GetKeyColumn()[0];
            if (key.IsIdentity)
            {
                object val;
                if (key.PropertyInfo.PropertyType == PublicDataDic.LongType)
                {
                    val = this.Provider.Insertable(item).ExecuteReturnBigIdentity();
                }
                else
                {
                    val = this.Provider.Insertable(item).ExecuteReturnIdentity();
                }
                return (IdentityId)val;
            }
            _ = this.Provider.Insertable(item).ExecuteCommand();
            return item.GetObjectValue<IdentityId>(key.DbColumnName);
        }
        public long InsertReutrnIdentity (T item)
        {
            return this.Provider.Insertable(item).ExecuteReturnBigIdentity();
        }
        public int InsertTable (List<T> items)
        {
            return this.Provider.Fastest<T>().BulkCopy(items);
        }
        public void Insert (T[] items)
        {
            IInsertable<T> insert = this.Provider.Insertable(items);
            if (items.Length < 500)
            {
                _ = insert.UseParameter().ExecuteCommand();
                return;
            }
            _ = insert.ExecuteCommand();
        }
        public void Insert<Table> (Table item) where Table : class, new()
        {
            _ = this.Provider.Insertable(item).ExecuteCommand();
        }
        public void Insert<Table> (Table[] items) where Table : class, new()
        {
            IInsertable<Table> insert = this.Provider.Insertable(items);
            if (items.Length < 500)
            {
                _ = insert.UseParameter().ExecuteCommand();
                return;
            }
            _ = insert.ExecuteCommand();
        }
        public void Insert (List<T> items)
        {
            IInsertable<T> insert = this.Provider.Insertable(items);
            if (items.Count < 500)
            {
                _ = insert.UseParameter().ExecuteCommand();
                return;
            }
            _ = insert.ExecuteCommand();
        }

        public int Count ()
        {
            return this.Queryable.Count();
        }
        public int Count (string table, Expression<Func<T, bool>> filter)
        {
            return this._GetQueryable(filter).AS(table).Count();
        }
        public int Count (string table)
        {
            return this.Queryable.AS(table).Count();
        }
        public int Count (Expression<Func<T, bool>> filter)
        {
            return this._GetQueryable(filter).Count();
        }

        public bool Delete (Expression<Func<T, bool>> filter)
        {
            return this.Provider.Deleteable<T>().Where(filter).ExecuteCommandHasChange();
        }
        public bool Delete (Expression<Func<T, bool>> filter, Expression<Func<T, object>> selector, int num)
        {
            return this.Provider.Deleteable<T>().In(selector, this.Provider.Queryable<T>().Where(filter).Take(num).Select(selector)).ExecuteCommandHasChange();
        }
        public IDeleteable<T> Delete ()
        {
            return this.Provider.Deleteable<T>();
        }
        public bool Delete<Other> (Expression<Func<T, Other, bool>> filter) where Other : class, new()
        {
            return this.Provider.Deleteable<T>().Where(p => SqlFunc.Subqueryable<Other>().Where(filter).Any()).ExecuteCommandHasChange();
        }


        public int ExecuteSql (string sql, params SugarParameter[] param)
        {
            return this.Provider.Ado.ExecuteCommand(sql, param);
        }

        public T Get (string sql, params SugarParameter[] param)
        {
            return this.Provider.Ado.SqlQuerySingle<T>(sql, param);
        }
        public Result GetById<IdentityId, Result> (IdentityId id) where Result : class
        {
            return this.Queryable.In(id).Select<Result>().First();
        }
        public Result GetById<IdentityId, Result> (IdentityId id, Expression<Func<T, Result>> selector)
        {
            return this.Queryable.In(id).Select(selector).First();
        }

        public T GetById<IdentityId> (IdentityId id)
        {
            return this.Queryable.In(id).First();
        }
        public T Get (Expression<Func<T, bool>> filter)
        {
            return this._GetQueryable(filter).First();
        }

        public T Get (Expression<Func<T, bool>> filter, string orderby)
        {
            return this._GetQueryable(filter).OrderBy(orderby).First();
        }
        public Result Get<Result> (Expression<Func<T, bool>> filter) where Result : class
        {
            return this._GetQueryable(filter).Select<Result>().First();
        }
        public Result Get<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector)
        {
            return this._GetQueryable(filter).Select(selector).First();
        }
        public Result Get<Result> (Expression<Func<T, Result>> selector, string orderby)
        {
            return this.Queryable.OrderBy(orderby).Select(selector).First();
        }
        public Result Get<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector, string orderby)
        {
            return this._GetQueryable(filter).OrderBy(orderby).Select(selector).First();
        }
        public Result Get<Result> (ISugarQueryable<Result>[] list, string orderBy) where Result : class, new()
        {
            return this.Provider.UnionAll(list).OrderBy(orderBy).First();
        }
        public Result[] Gets<Result> (ISugarQueryable<Result>[] list, string orderBy) where Result : class, new()
        {
            return this.Provider.UnionAll(list).OrderBy(orderBy).ToArray();
        }
        public Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list, string selector, string orderBy) where Table : class, new()
        {
            return this.Provider.UnionAll(list).Select<Result>(selector).OrderBy(orderBy).ToArray();
        }
        public Result Get<Table, Result> (ISugarQueryable<Table>[] list, string selector, string orderBy) where Table : class, new()
        {
            return this.Provider.UnionAll(list).Select<Result>(selector).OrderBy(orderBy).First();
        }
        public Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list, string selector) where Table : class, new()
        {
            return this.Provider.UnionAll(list).Select<Result>(selector).ToArray();
        }
        public Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list, Expression<Func<Table, Result>> selector, string orderBy) where Table : class, new()
        {
            return this.Provider.UnionAll(list).Select(selector).OrderBy(orderBy).ToArray();
        }
        public Result Get<Table, Result> (ISugarQueryable<Table>[] list, Expression<Func<Table, Result>> selector, string orderBy) where Table : class, new()
        {
            return this.Provider.UnionAll(list).Select(selector).OrderBy(orderBy).First();
        }
        public Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list) where Table : class, new()
        {
            return this.Provider.UnionAll(list).Select<Result>().ToArray();
        }
        public Result[] Gets<Table, Result> (ISugarQueryable<Table>[] list, Expression<Func<Table, Result>> selector) where Table : class, new()
        {
            return this.Provider.UnionAll(list).Select(selector).ToArray();
        }
        public Result[] Gets<Result> (ISugarQueryable<Result>[] list) where Result : class, new()
        {
            return this.Provider.UnionAll(list).ToArray();
        }

        public T[] GetAll ()
        {
            return this.Queryable.ToArray();
        }

        public T[] GetAll (string orderby)
        {
            return this.Queryable.OrderBy(orderby).ToArray();
        }
        public Result[] GetAll<Result> () where Result : class, new()
        {
            return this.Queryable.Select<Result>().ToArray();
        }
        public Result[] GetAll<Result> (Expression<Func<T, Result>> selector)
        {
            return this.Queryable.Select(selector).ToArray();
        }

        public Result[] GetAll<Result> (Expression<Func<T, Result>> selector, string orderby)
        {
            return this.Queryable.Select(selector).OrderBy(orderby).ToArray();
        }
        public Result[] GetAll<Result> (Expression<Func<T, Result>> selector, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc)
        {
            return this.Queryable.OrderBy(orderBy, type).Select(selector).ToArray();
        }
        public T[] GetsById<IdentityId> (IdentityId[] ids)
        {
            return this.Queryable.In(ids).ToArray();
        }

        public Result[] GetsById<IdentityId, Result> (IdentityId[] ids) where Result : class
        {
            return this.Queryable.In(ids).Select<Result>().ToArray();
        }

        public Result[] GetsById<IdentityId, Result> (IdentityId[] ids, Expression<Func<T, Result>> selector)
        {
            return this.Queryable.In(ids).Select(selector).ToArray();
        }
        public Result[] GetsById<IdentityId, Result> (IdentityId[] ids, Expression<Func<T, Result>> selector, string orderBy)
        {
            return this.Queryable.In(ids).OrderBy(orderBy).Select(selector).ToArray();
        }

        public Result[] GetsById<IdentityId, Result> (IdentityId[] ids, Expression<Func<T, Result>> selector, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc)
        {
            return this.Queryable.In(ids).OrderBy(orderBy, type).Select(selector).ToArray();
        }
        public Result[] Gets<Result> (string sql, params SugarParameter[] param) where Result : class, new()
        {
            return this.Provider.Ado.SqlQuery<Result>(sql, param).ToArray();
        }

        public T[] Gets (Expression<Func<T, bool>> filter)
        {
            return this._GetQueryable(filter).ToArray();
        }

        public T[] Gets (Expression<Func<T, bool>> filter, string orderby)
        {
            return this._GetQueryable(filter).OrderBy(orderby).ToArray();
        }
        public T[] Gets (Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc)
        {
            return this._GetQueryable(filter).OrderBy(orderBy, type).ToArray();
        }
        public Result[] Gets<Result> (Expression<Func<T, bool>> filter, string orderby) where Result : class, new()
        {
            return this._GetQueryable(filter).OrderBy(orderby).Select<Result>().ToArray();
        }
        public Result[] Gets<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc)
        {
            return this._GetQueryable(filter).OrderBy(orderBy, type).Select<Result>().ToArray();
        }
        public Result[] Gets<Result> (Expression<Func<T, bool>> filter) where Result : class, new()
        {
            return this._GetQueryable(filter).Select<Result>().ToArray();
        }
        public Result[] Gets<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector)
        {
            return this._GetQueryable(filter).Select(selector).ToArray();
        }
        public Result[] Gets<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector, Expression<Func<T, object>> orderBy, OrderByType type = OrderByType.Asc)
        {
            return this._GetQueryable(filter).OrderBy(orderBy, type).Select(selector).ToArray();
        }
        public Result[] Gets<Result> (Expression<Func<T, Result>> selector, IBasicPage paging)
        {
            return this.Queryable.OrderBy(paging.OrderBy).Select(selector).Skip(( paging.Index - 1 ) * paging.Size).Take(paging.Size).ToArray();
        }
        public Result[] Gets<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector, string orderby)
        {
            return this._GetQueryable(filter).OrderBy(orderby).Select(selector).ToArray();
        }

        public Result[] GroupBy<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, object>> group, Expression<Func<T, Result>> selector)
        {
            return this._GetQueryable(filter).GroupBy(group).Select(selector).ToArray();
        }
        public Result[] GroupBy<Result> (Expression<Func<T, object>> group, Expression<Func<T, Result>> selector)
        {
            return this.Queryable.GroupBy(group).Select(selector).ToArray();
        }
        public Result[] GroupBy<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, object>> group, Expression<Func<T, bool>> having, Expression<Func<T, Result>> selector)
        {
            return this._GetQueryable(filter).GroupBy(group).Having(having).Select(selector).ToArray();
        }
        public bool IsExist ()
        {
            return this.Provider.Queryable<T>().Any();
        }
        public bool IsExist (Expression<Func<T, bool>> filter)
        {
            return this._GetQueryable(filter).Any();
        }
        public Result[] Join<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
         Expression<Func<T, TInner, Result>> selector) where TInner : class
        {
            return this.Queryable.InnerJoin(joinExpression).Select(selector).ToArray();
        }

        public Result[] JoinGroupBy<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
         Expression<Func<T, TInner, object>> group,
         Expression<Func<T, TInner, Result>> selector) where TInner : class
        {
            return this.Queryable.InnerJoin(joinExpression).GroupBy(group).Select(selector).ToArray();
        }
        public Result[] JoinGroupBy<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
            Expression<Func<T, TInner, bool>> filter,
             Expression<Func<T, TInner, object>> group,
            Expression<Func<T, TInner, Result>> selector) where TInner : class
        {
            if (filter == null)
            {
                return this.JoinGroupBy(joinExpression, group, selector);
            }
            return this.Queryable.InnerJoin(joinExpression).Where(filter).GroupBy(group).Select(selector).ToArray();
        }
        public Result JoinGet<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
      Expression<Func<T, TInner, Result>> selector) where TInner : class
        {
            return this.Queryable.InnerJoin(joinExpression).Select(selector).First();
        }
        public Result JoinGet<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
           Expression<Func<T, TInner, bool>> filter,
           Expression<Func<T, TInner, Result>> selector) where TInner : class
        {
            if (filter == null)
            {
                return this.JoinGet(joinExpression, selector);
            }
            return this.Queryable.InnerJoin(joinExpression).Where(filter).Select(selector).First();
        }

        public Result[] Join<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
            Expression<Func<T, TInner, bool>> filter,
            Expression<Func<T, TInner, Result>> selector) where TInner : class
        {
            if (filter == null)
            {
                return this.Join(joinExpression, selector);
            }
            return this.Queryable.InnerJoin(joinExpression).Where(filter).Select(selector).ToArray();
        }
        public Result[] JoinQuery<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
          Expression<Func<T, TInner, bool>> filter,
          Expression<Func<T, TInner, Result>> selector,
          IBasicPage pager,
          out int count) where TInner : class
        {
            ISugarQueryable<T, TInner> table = this.Queryable.InnerJoin(joinExpression);
            if (filter != null)
            {
                table = table.Where(filter);
            }
            count = table.Clone().Count();
            return table.OrderBy(pager.OrderBy).Select(selector).Skip(( pager.Index - 1 ) * pager.Size).Take(pager.Size).ToArray();
        }
        public Result[] JoinQuery<TInner, Result> (Expression<Func<T, TInner, bool>> join,
          Expression<Func<T, TInner, Result>> selector,
          IBasicPage pager,
          out int count) where TInner : class
        {
            ISugarQueryable<T, TInner> table = this.Queryable.InnerJoin(join);
            count = table.Clone().Count();
            return table.OrderBy(pager.OrderBy).Select(selector).Skip(( pager.Index - 1 ) * pager.Size).Take(pager.Size).ToArray();
        }
        public Result[] JoinQuery<TInner, Result> (Expression<Func<T, TInner, bool>> joinExpression,
          Expression<Func<T, TInner, bool>> filter,
          Expression<Func<T, TInner, Result>> selector,
          int index,
          int size,
           Expression<Func<T, TInner, object>> orderby,
           OrderByType byType,
          out int count) where TInner : class
        {
            ISugarQueryable<T, TInner> table = this.Queryable.InnerJoin(joinExpression);
            if (filter != null)
            {
                table = table.Where(filter);
            }
            count = table.Clone().Count();
            return table.OrderBy(orderby, byType).Select(selector).Skip(( index - 1 ) * size).Take(size).ToArray();
        }

        public Result[] JoinGroupBy<TInner, TKey, TResult, Result, GroupKey> (Expression<Func<T, TInner, bool>> joinExpression,
            Expression<Func<T, TInner, bool>> filter,
            Expression<Func<T, TInner, object>> group,
            Expression<Func<T, TInner, Result>> selector) where TInner : class
        {
            ISugarQueryable<T, TInner> table = this.Queryable.InnerJoin(joinExpression);
            if (filter != null)
            {
                table = table.Where(filter);
            }
            return table.GroupBy(group).Select(selector).ToArray();
        }

        public Result Max<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector)
        {
            return this._GetQueryable(filter).Max(selector);
        }

        public Result Min<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector)
        {
            return this._GetQueryable(filter).Min(selector);
        }
        public Result Avg<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector)
        {
            return this._GetQueryable(filter).Avg(selector);
        }
        public Result Max<Result> (Expression<Func<T, Result>> selector)
        {
            return this.Queryable.Max(selector);
        }
        public Result Min<Result> (Expression<Func<T, Result>> selector)
        {
            return this.Queryable.Min(selector);
        }

        public Result Avg<Result> (Expression<Func<T, Result>> selector)
        {
            return this.Queryable.Avg(selector);
        }
        public T[] Query (Expression<Func<T, bool>> filter, IBasicPage pager, out int count)
        {
            this._InitOrderBy(pager);
            ISugarQueryable<T> table = this._GetQueryable(filter);
            count = table.Clone().Count();
            return table.OrderBy(pager.OrderBy).Skip(( pager.Index - 1 ) * pager.Size).Take(pager.Size).ToArray();
        }
        public Result[] GroupByQuery<Result> ( Expression<Func<T, bool>> filter, Expression<Func<T, object>> group, Expression<Func<T, Result>> selector,IBasicPage pager, out int count ) 
        {
            this._InitOrderBy(pager);
            ISugarQueryable<T> table = this._GetQueryable(filter);
            count = table.Clone().GroupBy(group).Select<Result>(selector).Count();
            return table.GroupBy(group).OrderBy(pager.OrderBy).Select<Result>(selector).Skip(( pager.Index - 1 ) * pager.Size).Take(pager.Size).ToArray();
        }
        public T[] Query (IBasicPage pager, out int count)
        {
            this._InitOrderBy(pager);
            ISugarQueryable<T> table = this.Queryable;
            count = table.Clone().Count();
            return table.OrderBy(pager.OrderBy).Skip(( pager.Index - 1 ) * pager.Size).Take(pager.Size).ToArray();
        }
        private void _InitOrderBy (IBasicPage pager)
        {
            if (pager.SortName.IsNull())
            {
                string key = this.GetKeys().Join(",");
                pager.InitOrderBy(key, false);
            }
        }
        public Result[] Query<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector, IBasicPage pager, out int count)
        {
            this._InitOrderBy(pager);
            ISugarQueryable<T> table = this._GetQueryable(filter);
            count = table.Clone().Count();
            return table.OrderBy(pager.OrderBy).Select(selector).Skip(( pager.Index - 1 ) * pager.Size).Take(pager.Size).ToArray();
        }
        public Result[] Query<Result> (Expression<Func<T, bool>> filter, IBasicPage pager, out int count) where Result : class
        {
            this._InitOrderBy(pager);
            ISugarQueryable<T> table = this._GetQueryable(filter);
            count = table.Clone().Count();
            return table.OrderBy(pager.OrderBy).Select<Result>().Skip(( pager.Index - 1 ) * pager.Size).Take(pager.Size).ToArray();
        }
        public bool Update (T[] datas, params string[] setColumn)
        {
            if (setColumn.IsNull())
            {
                return this.Provider.Updateable(datas).ExecuteCommandHasChange();
            }
            return this.Provider.Updateable(datas).UpdateColumns(setColumn).ExecuteCommandHasChange();
        }
        public bool UpdateOnly (T[] datas, Expression<Func<T, object>> setCol)
        {
            return this.Provider.Updateable(datas).UpdateColumns(setCol).ExecuteCommandHasChange();
        }
        public bool Update (T[] datas, Expression<Func<T, object>> filter)
        {
            return this.Provider.Updateable(datas).WhereColumns(filter).ExecuteCommandHasChange();
        }
        public bool Update<Set> (Set set, Expression<Func<T, bool>> filter)
        {
            return this.Provider.Updateable<T>(set).Where(filter).ExecuteCommandHasChange();
        }
        public bool Update<Set> (Set set, Expression<Func<T, object>> filter)
        {
            return this.Provider.Updateable<T>(set).WhereColumns(filter).ExecuteCommandHasChange();
        }
        public bool Update<Set> (Set[] set) where Set : class, new()
        {
            T[] datas = set.ToConvertAll<Set, T>(out string[] column);
            return this.Provider.Updateable(datas).UpdateColumns(column).ExecuteCommandHasChange();
        }
        public bool Update (string table, T data, params string[] column)
        {
            if (column.IsNull())
            {
                return this.Provider.Updateable(data).AS(table).ExecuteCommandHasChange();
            }
            return this.Provider.Updateable(data).AS(table).UpdateColumns(column).ExecuteCommandHasChange();
        }
        public bool Update (T data, string[] column)
        {
            if (column.IsNull())
            {
                return this.Provider.Updateable(data).ExecuteCommandHasChange();
            }
            return this.Provider.Updateable(data).UpdateColumns(column).ExecuteCommandHasChange();
        }

        public bool Update (T data, string[] column, Expression<Func<T, bool>> filter)
        {
            if (column.IsNull())
            {
                return this.Provider.Updateable(data).Where(filter).ExecuteCommandHasChange();
            }
            return this.Provider.Updateable(data).UpdateColumns(column).Where(filter).ExecuteCommandHasChange();
        }
        public bool Update (Expression<Func<T, T>> columns, Expression<Func<T, bool>> filter)
        {
            return this.Provider.Updateable<T>().SetColumns(columns).Where(filter).ExecuteCommandHasChange();
        }

        public int UpdateByRowIndex (Expression<Func<T, T>> columns, Expression<Func<T, bool>> filter)
        {
            return this.Provider.Updateable<T>().SetColumns(columns).Where(filter).ExecuteCommand();
        }
        public bool Update (Expression<Func<T, T>> columns, string[] whereColumn)
        {
            return this.Provider.Updateable<T>().SetColumns(columns).WhereColumns(whereColumn).ExecuteCommandHasChange();
        }
        public bool UpdateAll (Expression<Func<T, T>> columns)
        {
            return this.Provider.Updateable<T>().SetColumns(columns).ExecuteCommandHasChange();
        }
        public bool Update (Expression<Func<T, T>> columns, Expression<Func<T, object>> filter)
        {
            return this.Provider.Updateable<T>().SetColumns(columns).WhereColumns(filter).ExecuteCommandHasChange();
        }
        public bool Update (Expression<Func<T, bool>> columns, Expression<Func<T, bool>> filter)
        {
            return this.Provider.Updateable<T>().SetColumns(columns).Where(filter).ExecuteCommandHasChange();
        }
        public bool Update (Expression<Func<T, T>> columns)
        {
            return this.Provider.Updateable<T>().SetColumns(columns).ExecuteCommandHasChange();
        }
        public IUpdateable<T> Update (T data)
        {
            return this.Provider.Updateable(data);
        }
        public IUpdateable<T> Update ()
        {
            return this.Provider.Updateable<T>();
        }
        public IUpdateable<T> Update<Set> (Set set)
        {
            return this.Provider.Updateable<T>(set);
        }
        public bool Update<Set> (T source, Set set)
        {
            string[] cols = source.Merge(set);
            if (cols.Length == 0)
            {
                return false;
            }
            else if (!this.Update(source, cols))
            {
                throw new ErrorException("db.update.fail");
            }
            return true;
        }

        public bool Update (T source, T set)
        {
            string[] cols = source.Merge<T>(set);
            if (cols.Length == 0)
            {
                return false;
            }
            return this.Update(source, cols);
        }

        public Result Sum<Result> (Expression<Func<T, bool>> filter, Expression<Func<T, Result>> selector)
        {
            return this._GetQueryable(filter).Sum(selector);
        }
        public bool AddOrUpdate (List<T> datas)
        {
            return this.Provider.Storageable(datas).ExecuteCommand() > 0;
        }
        public IStorageable<T> Storageable (List<T> datas)
        {
            return this.Provider.Storageable(datas);
        }
        public bool AddOrUpdate (List<T> datas, Func<StorageableInfo<T>, bool> insert, Func<StorageableInfo<T>, bool> update)
        {
            return this.Provider.Storageable(datas).SplitInsert(insert).SplitUpdate(update).ExecuteCommand() > 0;
        }
        public int Insert (List<T> datas, Expression<Func<T, object>> filter)
        {
            StorageableResult<T> table = this.Provider.Storageable(datas).WhereColumns(filter).ToStorage();
            return table.AsInsertable.ExecuteCommand();
        }
        public bool AddOrUpdate (List<T> datas, Expression<Func<T, object>> filter)
        {
            IStorageable<T> table = this.Provider.Storageable(datas).WhereColumns(filter);
            string[] cols = this._GetKeyColumn().Convert(c => c.IsPrimarykey || c.IsIdentity, c => c.DbColumnName);
            if (cols.Length == 0)
            {
                return table.ExecuteCommand() > 0;
            }
            else
            {
                StorageableResult<T> result = table.ToStorage();
                int row = result.AsInsertable.ExecuteCommand();
                row += result.AsUpdateable.IgnoreColumns(cols).ExecuteCommand();
                return row > 0;
            }
        }
        public bool AddOrUpdate (List<T> datas, string table, Expression<Func<T, object>> filter)
        {
            IStorageable<T> storageable = this.Provider.Storageable(datas).As(table).WhereColumns(filter);
            string[] cols = this._GetKeyColumn().Convert(c => c.IsPrimarykey || c.IsIdentity, c => c.DbColumnName);
            if (cols.Length == 0)
            {
                return storageable.ExecuteCommand() > 0;
            }
            else
            {
                StorageableResult<T> result = storageable.ToStorage();
                int row = result.AsInsertable.ExecuteCommand();
                row += result.AsUpdateable.IgnoreColumns(cols).ExecuteCommand();
                return row > 0;
            }
        }
        public bool AddOrUpdate (List<T> datas, Expression<Func<T, object>> filter, Expression<Func<T, object>> update)
        {
            StorageableResult<T> table = this.Provider.Storageable(datas).WhereColumns(filter).ToStorage();
            int row = table.AsInsertable.ExecuteCommand();
            row += table.AsUpdateable.UpdateColumns(update).ExecuteCommand();
            return row > 0;
        }

    }
}
