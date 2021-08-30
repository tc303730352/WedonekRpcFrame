namespace SqlExecHelper
{
        internal struct RunParam
        {
                public RunParam(string type, string index)
                {
                        this.LockType = type;
                        this.Index = index;
                }
                public RunParam(string type)
                {
                        this.LockType = type;
                        this.Index = null;
                }
                public readonly string LockType;
                public readonly string Index;
                public override string ToString()
                {
                        if (this.Index == null)
                        {
                                return string.Format(" with({0})", this.LockType);
                        }
                        else if (this.LockType == null)
                        {
                                return string.Format(" with(INDEX({0})", this.Index);
                        }
                        return string.Format(" with({0},INDEX({1})", this.LockType, this.Index);
                }
        }
}
