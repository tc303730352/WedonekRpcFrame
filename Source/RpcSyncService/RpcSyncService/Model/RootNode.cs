namespace RpcSyncService.Model
{
        public class RootNode
        {
                public long Id
                {
                        get;
                        set;
                }

                public string Dictate
                {
                        get;
                        set;
                }
                public override bool Equals(object obj)
                {
                        if (obj is RootNode i)
                        {
                                return i.Id == this.Id;
                        }
                        return false;
                }
                public override int GetHashCode()
                {
                        return this.Id.GetHashCode();
                }
        }
}
