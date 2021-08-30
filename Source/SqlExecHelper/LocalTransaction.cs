namespace SqlExecHelper
{
        public class LocalTransaction : ITransaction
        {
                private readonly IDAL _DAL = null;
                public LocalTransaction()
                {
                        this._DAL = Config.SqlConfig.SqlFactory.GetDAL().BeginTrans();

                }
                internal LocalTransaction(IDAL dal)
                {
                        this._DAL = dal;

                }
                public LocalTransaction(string conName)
                {
                        this._DAL = Config.SqlConfig.SqlFactory.GetDAL(conName).BeginTrans();
                }
                public IDAL Source => this._DAL;

                public void Commit()
                {
                        this._DAL.CommitTrans();
                }

                public void Dispose()
                {
                        this._DAL.Dispose();
                }
        }
}
