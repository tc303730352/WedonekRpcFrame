using System.Linq.Expressions;
using SqlSugar;
using WeDonekRpc.Helper;

namespace WeDonekRpc.SqlSugar.Queue
{
    internal class LocalQueue<T> : LocalQueue, ISqlQueue<T> where T : class, new()
    {
        public LocalQueue (SqlSugarProvider client) : base(client)
        {
        }


        public void Insert (T item)
        {
            this._DbClient.Insertable(item).AddQueue();
        }

        public void Insert (T[] items)
        {
            this._DbClient.Insertable(items).AddQueue();
        }
        public void Delete (Expression<Func<T, bool>> filter)
        {
            this._DbClient.Deleteable<T>().Where(filter).AddQueue();
        }
        public void DeleteAll ()
        {
            this._DbClient.Deleteable<T>().AddQueue();
        }
        public void Delete<Other> (Expression<Func<T, Other, bool>> filter) where Other : class, new()
        {
            this._DbClient.Deleteable<T>().Where(p => SqlFunc.Subqueryable<Other>().Where(filter).Any()).AddQueue();
        }
        public void UpdateByKey (T data, params string[] setColumn)
        {
            if (setColumn.IsNull())
            {
                this._DbClient.Updateable(data).AddQueue();
                return;
            }
            this._DbClient.Updateable(data).UpdateColumns(setColumn).AddQueue();
        }
        public void Update (T[] datas, params string[] setColumn)
        {
            if (setColumn.IsNull())
            {
                this._DbClient.Updateable(datas).AddQueue();
                return;
            }
            this._DbClient.Updateable(datas).UpdateColumns(setColumn).AddQueue();
        }
        public void Update<Set> (Set set, Expression<Func<T, bool>> filter)
        {
            this._DbClient.Updateable<T>(set).Where(filter).AddQueue();
        }
        public void UpdateOnly (T[] datas, Expression<Func<T, object>> setCol)
        {
            this._DbClient.Updateable(datas).UpdateColumns(setCol).AddQueue();
        }

        public void Update (T data, string[] setColumn, Expression<Func<T, bool>> filter)
        {
            this._DbClient.Updateable(data).UpdateColumns(setColumn).Where(filter).AddQueue();
        }
        public void Update (T data, params string[] setColumn)
        {
            if (setColumn.IsNull())
            {
                this._DbClient.Updateable(data).AddQueue();
                return;
            }
            this._DbClient.Updateable(data).UpdateColumns(setColumn).AddQueue();
        }
        public void Update (T data, Expression<Func<T, object>> columns)
        {
            this._DbClient.Updateable<T>(data).UpdateColumns(columns).AddQueue();
        }
        public void Update (Expression<Func<T, T>> columns, Expression<Func<T, bool>> filter)
        {
            this._DbClient.Updateable<T>().SetColumns(columns).Where(filter).AddQueue();
        }
        public void UpdateOneColumn (Expression<Func<T, bool>> columns, Expression<Func<T, bool>> filter)
        {
            this._DbClient.Updateable<T>().SetColumns(columns).Where(filter).AddQueue();
        }
        public IUpdateable<T> Update ()
        {
            return this._DbClient.Updateable<T>();
        }
        public IDeleteable<T> Delete ()
        {
            return this._DbClient.Deleteable<T>();
        }

        public bool Update<Set> (T source, Set set)
        {
            string[] cols = source.Merge(set);
            if (cols.Length == 0)
            {
                return false;
            }
            this.Update(source, cols);
            return true;
        }

        public bool Update (T source, T set)
        {
            string[] cols = source.Merge<T>(set);
            if (cols.Length == 0)
            {
                return false;
            }
            this.Update(source, cols);
            return true;
        }

        public void Update (T[] datas, Expression<Func<T, object>> filter)
        {
            this._DbClient.Updateable(datas).WhereColumns(filter).AddQueue();
        }

        public void Update (T data, Expression<Func<T, bool>> filter)
        {
            this._DbClient.Updateable<T>(data).Where(filter).AddQueue();
        }
    }
}
