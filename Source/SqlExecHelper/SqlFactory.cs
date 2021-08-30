
namespace SqlExecHelper
{
        public class SqlFactory : ISqlFactory
        {
                [System.ThreadStatic]
                private IDAL _CurrentDAL = null;

                public virtual IDAL GetDAL(string conName)
                {
                        if (this._CurrentDAL != null && this._CurrentDAL.ConName == conName)
                        {
                                return this._CurrentDAL;
                        }
                        MyDAL myDAL = new MyDAL(conName);
                        myDAL.TranBegin += this.MyDAL_TranBegin;
                        myDAL.TranEnd += this.MyDAL_TranEnd;
                        return myDAL;
                }

                private void MyDAL_TranEnd(IDAL dal)
                {
                        this._CurrentDAL = null;
                }

                private void MyDAL_TranBegin(IDAL dal)
                {
                        this._CurrentDAL = dal;
                }

                public virtual IDAL GetDAL()
                {
                        if (this._CurrentDAL != null && this._CurrentDAL.ConName == "defSqlCon")
                        {
                                return this._CurrentDAL;
                        }
                        MyDAL myDAL = new MyDAL();
                        myDAL.TranBegin += this.MyDAL_TranBegin;
                        myDAL.TranEnd += this.MyDAL_TranEnd;
                        return myDAL;
                }
        }
}
