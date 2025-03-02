using RpcStore.Collect.Model;
using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.Helper
{
    internal class TransmitGenerateHelper
    {
        private static readonly long _HashCodeSum = 4294967295;
        private static readonly int _ZoneIndexSum = 79;

        private static readonly float _TransmitScale = 2.5f;

        private string[] _ServerId;
        private Dictionary<string, Transmit> _Transmits;

        public TransmitDatum[] InitTransmits (Transmit[] list, TransmitType type)
        {
            this._Transmits = list.ToDictionary(c => c.ServerCode, c => c);
            this._ServerId = this._Transmits.Keys.ToArray();
            this._AutoGenerate(type);
            return this._Transmits.Values.ConvertAll(c =>
            {
                return new TransmitDatum
                {
                    ServerCode = c.ServerCode,
                    TransmitConfig = new TransmitConfig[]
                    {
                        new TransmitConfig
                        {
                            Range = c.Range.ToArray()
                        }
                    }
                };
            });
        }
        private void _AutoGenerate (TransmitType type)
        {
            if (type == TransmitType.HashCode)
            {
                this._InitHashCode();
            }
            else if (type == TransmitType.ZoneIndex)
            {
                this._InitZoneIndex();
            }
        }
        private void _InitHashCode ()
        {
            if (this._ServerId.Length == 1)
            {
                this._Transmits[this._ServerId[0]].Range.Add(new TransmitRange
                {
                    BeginRange = int.MinValue,
                    EndRange = int.MaxValue,
                    IsFixed = false
                });
            }
            else
            {
                this._InitTransmit(_HashCodeSum, int.MinValue, (long)int.MaxValue + 1);
            }
        }

        private void _InitZoneIndex ()
        {
            if (this._ServerId.Length == 1)
            {
                this._Transmits[this._ServerId[0]].Range.Add(new TransmitRange
                {
                    BeginRange = 48,
                    EndRange = 128,
                    IsFixed = false
                });
            }
            else
            {
                this._InitTransmit(_ZoneIndexSum, 48, 128);
            }
        }

        private void _InitTransmit (long sum, long begin, long max)
        {
            int num = (int)( this._ServerId.Length * _TransmitScale );
            if (num == 0)
            {
                return;
            }
            long size = sum / num;
            int len = this._ServerId.Length;
            string endId = this._ServerId[this._ServerId.Length - 1];
            for (int k = 0; k < num; k++)
            {
                string i = this._ServerId[k % len];
                long end = i == endId ? max : begin + size;
                Transmit transmit = this._Transmits[i];
                transmit.Range.Add(new TransmitRange
                {
                    BeginRange = begin,
                    EndRange = end
                });
                begin = end;
            }
        }
    }
}
