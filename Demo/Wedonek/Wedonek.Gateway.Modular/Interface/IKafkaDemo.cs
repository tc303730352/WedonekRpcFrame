namespace Wedonek.Gateway.Modular.Interface
{
    public interface IKafkaDemo : System.IDisposable
    {
        void Producer ();
        void Subscribe ();
    }
}