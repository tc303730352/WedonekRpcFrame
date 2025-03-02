using System.Linq.Expressions;
using SqlSugar;
using WeDonekRpc.Helper;

namespace WeDonekRpc.SqlSugar.Queue
{
    internal class LocalQueue : ISqlQueue
    {
        protected SqlSugarProvider _DbClient;
        public LocalQueue (SqlSugarProvider client)
        {
            this._DbClient = client;
        }
        public bool IsNull
        {
            get => this._DbClient.Queues.Count == 0;
        }

        public void Update<Set> (Set data, string[] setColumn, Expression<Func<Set, bool>> filter) where Set : class, new()
        {
            this._DbClient.Updateable(data).UpdateColumns(setColumn).Where(filter).AddQueue();
        }
        public void Update<Set> (Expression<Func<Set, bool>> columns, Expression<Func<Set, bool>> filter) where Set : class, new()
        {
            this._DbClient.Updateable<Set>().SetColumns(columns).Where(filter).AddQueue();
        }

        public int Submit (bool isTran = true)
        {
            return this._DbClient.SaveQueues(isTran);
        }
        public void DeleteAll<Table> () where Table : class, new()
        {
            this._DbClient.Deleteable<Table>().AddQueue();
        }
        public void Delete<Table> (Expression<Func<Table, bool>> filter) where Table : class, new()
        {
            this._DbClient.Deleteable<Table>().Where(filter).AddQueue();
        }

        public void Delete<Table, Other> (Expression<Func<Table, Other, bool>> filter)
            where Table : class, new()
            where Other : class, new()
        {
            this._DbClient.Deleteable<Table>().Where(p => SqlFunc.Subqueryable<Other>().Where(filter).Any()).AddQueue();
        }

        public void Insert<Table> (Table item) where Table : class, new()
        {
            this._DbClient.Insertable(item).AddQueue();
        }

        public void Insert<Table> (Table[] items) where Table : class, new()
        {
            this._DbClient.Insertable(items).AddQueue();
        }
        public void Insert<Table> (List<Table> items) where Table : class, new()
        {
            this._DbClient.Insertable(items).AddQueue();
        }

        public void Update<Table> (Table[] datas, params string[] setColumn) where Table : class, new()
        {
            if (setColumn.IsNull())
            {
                this._DbClient.Updateable(datas).AddQueue();
                return;
            }
            this._DbClient.Updateable(datas).UpdateColumns(setColumn).AddQueue();
        }
        public void UpdateOneColumn<Table> (Expression<Func<Table, bool>> columns, Expression<Func<Table, bool>> filter) where Table : class, new()
        {
            this._DbClient.Updateable<Table>().SetColumns(columns).Where(filter).AddQueue();
        }
        public void Update<Table, Set> (Set set, Expression<Func<Table, bool>> filter) where Table : class, new()
        {
            this._DbClient.Updateable<Table>(set).Where(filter).AddQueue();
        }
        public void Update<Table> (Table data, params string[] setColumn) where Table : class, new()
        {
            this._DbClient.Updateable(data).UpdateColumns(setColumn).AddQueue();
        }
        public void Update<Table> (Expression<Func<Table, Table>> columns, Expression<Func<Table, bool>> filter) where Table : class, new()
        {
            this._DbClient.Updateable<Table>().SetColumns(columns).Where(filter).AddQueue();
        }
        public void Update<Table> (Table data, Expression<Func<Table, object>> columns, Expression<Func<Table, bool>> filter) where Table : class, new()
        {
            this._DbClient.Updateable<Table>(data).UpdateColumns(columns).Where(filter).AddQueue();
        }
        public bool Update<Table, Set> (Table source, Set set) where Table : class, new()
        {
            string[] cols = source.Merge(set);
            if (cols.Length == 0)
            {
                return false;
            }
            this.Update(source, cols);
            return true;
        }

        public bool Update<Table> (Table source, Table set) where Table : class, new()
        {
            string[] cols = source.Merge<Table>(set);
            if (cols.Length == 0)
            {
                return false;
            }
            this.Update(source, cols);
            return true;
        }

        public IUpdateable<Table> Update<Table> () where Table : class, new()
        {
            return this._DbClient.Updateable<Table>();
        }
        public IDeleteable<Table> Delete<Table> () where Table : class, new()
        {
            return this._DbClient.Deleteable<Table>();
        }

        public void Update<Table> (Table data, Expression<Func<Table, object>> columns) where Table : class, new()
        {
            this._DbClient.Updateable<Table>(data).UpdateColumns(columns).AddQueue();
        }
    }
}
