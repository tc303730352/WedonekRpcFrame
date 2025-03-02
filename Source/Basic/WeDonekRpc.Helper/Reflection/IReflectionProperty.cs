using System.Text;

namespace WeDonekRpc.Helper.Reflection
{
    public interface IReflectionProperty : IFastGetProperty
    {
        bool IsObject
        {
            get;
        }
        bool IsWrite { get; }
        bool IsChange (object source, object other);
        bool IsChange (IReflectionProperty otherPro, object source, object other);
        void SetValue (object source, object obj);

        bool IsEquals (IReflectionProperty otherPro, object source, object other);
        bool IsEquals (object source, object other);

        void ToString (object source, StringBuilder str);
    }
}