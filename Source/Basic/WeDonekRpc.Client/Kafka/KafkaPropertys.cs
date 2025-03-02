using System;
using System.Collections.Generic;
using System.Text;

using Confluent.Kafka;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Kafka
{
    public class KafkaPropertys
    {
        private Dictionary<string, byte[]> _Header = new Dictionary<string, byte[]>();

        public Acks? Ask
        {
            get;
            set;
        }

        public string TranId { get; set; }

        public int? TransactionTimeoutMs { get; set; }
        /// <summary>
        /// 事务初始化超时时间
        /// </summary>
        public int TranInitTimeOut { get; set; }
        public void SetHeader(string name, string data)
        {
            _Header.AddOrSet(name, Encoding.UTF8.GetBytes(data));
        }
        public void SetHeader(string name, DateTime time)
        {
            _Header.AddOrSet(name, BitConverter.GetBytes(time.ToLong()));
        }
        public void SetHeader(string name, byte[] data)
        {
            _Header.AddOrSet(name, data);
        }
        internal void InitHeader(Headers headers)
        {
            foreach (KeyValuePair<string, byte[]> i in this._Header)
            {
                headers.Add(i.Key, i.Value);
            }
        }
    }
}
