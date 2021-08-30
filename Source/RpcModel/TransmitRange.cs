namespace RpcModel
{
        [System.Serializable]
        public class TransmitRange
        {
                public long BeginRange
                {
                        get;
                        set;
                }

                public long EndRanage
                {
                        get;
                        set;
                }
                public bool IsFixed
                {
                        get;
                        set;
                }
                private string _Key = null;

                private string Key
                {
                        get
                        {
                                if (this._Key == null)
                                {
                                        this._Key = string.Join("_", this.IsFixed ? 1 : 0, this.BeginRange, this.EndRanage);
                                }
                                return this._Key;
                        }
                }
                public override bool Equals(object obj)
                {
                        if (obj is TransmitRange i)
                        {
                                return i.Key == this.Key;
                        }
                        return false;
                }
                public override int GetHashCode()
                {
                        return this.Key.GetHashCode();
                }
        }
}
