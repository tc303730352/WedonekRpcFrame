using System;
using System.Threading;

using Confluent.Kafka;

using WeDonekRpc.Client.Kafka.Interface;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Queue.Model;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Kafka
{
    /// <summary>
    /// Kafka消费者
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal class KafkaConsumer<Key, T> : IKafkaConsumer where T : class
    {
        private readonly IConsumer<Key, T> _Consumer;
        private readonly Action<IKafkaEvent<Key, T>> _EventAction;
        private readonly KafkaSubConfig _Config;

        private Thread _Thread;
        private volatile bool _IsStop = false;
        private bool _IsInit = false;
        public KafkaConsumer (KafkaQueue kafka, KafkaSubConfig config, Action<IKafkaEvent<Key, T>> action)
        {
            this._Config = config;
            this._EventAction = action;
            this._Consumer = this._GetConsumer(kafka);
        }
        public event Action<string> CloseEvent;

        private IConsumer<Key, T> _GetConsumer (KafkaQueue kafka)
        {
            ConsumerConfig config = kafka.GetConsumerConfig(this._Config);
            ConsumerBuilder<Key, T> builder = new ConsumerBuilder<Key, T>(config);
            _ = builder.SetValueDeserializer(new KafkaJsonDeserializer<T>());
            _ = builder.SetErrorHandler(this._ErrorHandler);
            _ = builder.SetLogHandler(this._LogHandler);
            return builder.Build();
        }

        private void _LogHandler (IConsumer<Key, T> source, LogMessage log)
        {
            RpcLogSystem.AddKafkaLog(log);
        }

        private void _ErrorHandler (IConsumer<Key, T> source, Confluent.Kafka.Error error)
        {
            RpcLogSystem.AddKafkaErrorLog(error);
        }

        public void AddBind (string[] queue)
        {
            if (this._IsInit)
            {
                return;
            }
            this._Consumer.Subscribe(queue);
            this._IsInit = true;
        }
        public void Subscribe ()
        {
            if (this._Thread == null && this._IsInit)
            {
                this._Thread = new Thread(new ThreadStart(this._ConsumerHandler));
                this._Thread.Start();
            }

        }
        private ConsumeResult<Key, T> _GetConsumeResult ()
        {
            try
            {
                return this._Consumer.Consume(CancellationToken.None);
            }
            catch (ConsumeException e)
            {
                RpcLogSystem.AddKafkaErrorLog(e);
                if (e.Error.IsFatal)
                {
                    this._Close("exception");
                }
                return null;
            }
        }
        private void _Close (string name)
        {
            this._IsStop = true;
            if (this.CloseEvent != null)
            {
                this.CloseEvent(name);
            }
            this._Consumer.Close();
            this._Consumer.Dispose();
        }
        private void _ConsumerHandler ()
        {
            while (!this._IsStop)
            {
                ConsumeResult<Key, T> result = this._GetConsumeResult();
                if (result == null || result.IsPartitionEOF)
                {
                    continue;
                }
                try
                {
                    this._EventAction(new KafkaEvent<Key, T>(result.Message, result.Topic));
                }
                catch (Exception e)
                {
                    ErrorException error = ErrorException.FormatError(e);
                    error.Args.Add("key", result.Message.Key.ToString());
                    error.Args.Add("Value", result.Message.Value.ToJson());
                    error.Args.Add("Topic", result.Topic);
                    error.SaveLog("Kafka");
                }
            }
        }

        public void Dispose ()
        {
            this._Close("close");
        }
    }
}
