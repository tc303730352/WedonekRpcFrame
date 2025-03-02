using System.Collections;
using System.Linq;
using System.Text.Json;

namespace WeDonekRpc.Helper.Json
{
    public class JsonBodyValue : IEnumerable
    {
        private readonly JsonElement _Source;
        public JsonBodyValue (JsonElement root)
        {
            this._Source = root;
        }
        public int Length
        {
            get
            {
                if (this._Source.ValueKind == JsonValueKind.Array)
                {
                    return this._Source.GetArrayLength();
                }
                return this._Source.EnumerateObject().Count();
            }
        }
        public bool IsObject
        {
            get => this._Source.ValueKind == JsonValueKind.Object;
        }
        public bool IsNull
        {
            get => this._Source.ValueKind == JsonValueKind.Null || this._Source.ValueKind == JsonValueKind.Undefined;
        }
        public bool IsArray
        {
            get => this._Source.ValueKind == JsonValueKind.Array;
        }
        public string GetJsonText ()
        {
            return this._Source.GetRawText();
        }
        public JsonValueKind ValueType
        {
            get => this._Source.ValueKind;
        }
        public IEnumerator GetEnumerator ()
        {
            if (this._Source.ValueKind == JsonValueKind.Array)
            {
                return this._Source.EnumerateArray();
            }
            return this._Source.EnumerateObject();
        }
        public override string ToString ()
        {
            if (this._Source.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            return this._Source.ToString();
        }

    }
}
