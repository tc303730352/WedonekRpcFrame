namespace WeDonekRpc.Helper.Reflection
{
    public class ObjectBody
    {
        public object Value { get; }
        private readonly IReflectionBody _Body;
        internal ObjectBody (object obj, IReflectionBody body)
        {
            this.Value = obj;
            this._Body = body;
        }

        public T GetValue<T> (string name)
        {
            return (T)this._Body.GetValue(this.Value, name);
        }
        public T GetValue<T> (int index)
        {
            return (T)this._Body.GetValue(this.Value, index);
        }
        public object GetValue (int index)
        {
            return this._Body.GetValue(this.Value, index);
        }
        public object GetValue (string name)
        {
            return this._Body.GetValue(this.Value, name);
        }
        public void SetValue (string name, object value)
        {
            this._Body.SetValue(this.Value, value, name);
        }
        public void SetValue (int index, object value)
        {
            this._Body.SetValue(this.Value, index, value);
        }
        public object GetDicValue (object key)
        {
            return this._Body.GetDicValue(this.Value, key);
        }
        public void SetDicValue (object source, object key, object obj)
        {
            this._Body.SetDicValue(source, key, obj);
        }
    }
}
