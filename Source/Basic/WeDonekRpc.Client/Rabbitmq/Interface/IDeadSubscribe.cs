namespace WeDonekRpc.Client.Rabbitmq.Interface
{
        internal interface IDeadSubscribe
        {
                void BindQueue (string queue);
                void Dispose ();
        }
}