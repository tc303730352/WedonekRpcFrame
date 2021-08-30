using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace RpcHelper.Json
{
        internal class TypeCache
        {
                public Type Type;
                public IJsonFormatMate Source;
        }
        internal class InterfaceCache
        {
                private static readonly Type _FormatType = typeof(IJsonFormatMate);
                private readonly Type _Type;
                private bool _IsInit = false;
                private TypeCache[] _Class = null;

                public InterfaceCache(Type type)
                {
                        this._Type = type;
                }

                public bool IsCanConvert { get; private set; }

                internal void Init()
                {
                        if (this._IsInit)
                        {
                                return;
                        }
                        this._IsInit = true;
                        Type[] types = _FindClass(this._Type);
                        if (types.Length == 0)
                        {
                                return;
                        }
                        else if (types.Length == 1)
                        {
                                this._Class = new TypeCache[] {
                                        new TypeCache
                                        {
                                                Type = types[0]
                                        }
                                };
                                this.IsCanConvert = true;
                        }
                        else
                        {
                                types = types.FindAll(a => a.GetInterface(_FormatType.FullName) != null);
                                if (types.Length > 0)
                                {
                                        this._Class = types.ConvertAll(a => new TypeCache
                                        {
                                                Type = a,
                                                Source = (IJsonFormatMate)Activator.CreateInstance(a)
                                        });
                                        this.IsCanConvert = true;
                                }
                        }
                }
                private static Type[] _FindClass(Type type)
                {
                        Assembly assembly = type.Assembly;
                        Type[] types = assembly.GetTypes();
                        return types.FindAll(a => _CheckType(a, type.FullName));
                }
                private static bool _CheckType(Type a, string type)
                {
                        if (!a.IsClass || a.GetInterface(type) == null)
                        {
                                return false;
                        }
                        return a.GetConstructors().FindIndex(b => b.GetParameters().Length == 0) != -1;
                }
                private Type _FindType(JObject obj)
                {
                        if (this._Class.Length == 1)
                        {
                                return this._Class[0].Type;
                        }
                        return this._Class.Find(a => a.Source.FormatMate(obj), a => a.Type);
                }
                internal object Read(JsonReader reader, JsonSerializer serializer)
                {
                        JObject obj = JObject.Load(reader);
                        Type source = this._FindType(obj);
                        if (source == null)
                        {
                                return null;
                        }
                        JsonContract contract = serializer.ContractResolver.ResolveContract(source);
                        object val = contract.DefaultCreator();
                        using (JsonReader sr = obj.CreateReader())
                        {
                                serializer.Populate(sr, val);
                        }
                        return val;
                }
        }
}
