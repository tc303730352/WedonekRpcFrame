namespace WeDonekRpc.Client.Kafka.Interface
{
        public interface IkafkaTransaction : System.IDisposable
        {
                void Commit();
                bool Producer(string msg, params string[] topic);
                bool Producer<T>(T msg, params string[] topic) where T : class;
        }
        public interface IkafkaTransaction<Key> : System.IDisposable
        {
                void Commit();
                bool Producer(string msg, Key key, params string[] topic);
                bool Producer<T>(T msg, Key key, params string[] topic) where T : class;
        }
}