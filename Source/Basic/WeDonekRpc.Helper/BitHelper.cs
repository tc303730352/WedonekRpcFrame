using System;
using System.Numerics;

namespace WeDonekRpc.Helper
{
    public static class BitHelper
    {
        /// <summary>
        /// 写入Int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static unsafe int WriteByte(int value, byte[] stream, int index)
        {
            fixed (byte* ptr = &stream[index])
            {
                *(int*)ptr = value;
            }
            return 4;
        }
        public static int WriteByte(decimal value, byte[] stream, int index)
        {
            return Tools.ToByte(value, stream, index);
        }
        public static int WriteByte(BigInteger value, byte[] stream, int index)
        {
            byte[] bytes = value.ToByteArray();
            Buffer.BlockCopy(bytes, 0, stream, index, bytes.Length);
            return bytes.Length;
        }

        public static unsafe int WriteByte(uint value, byte[] stream, int index)
        {
            fixed (byte* ptr = &stream[index])
            {
                *(uint*)ptr = value;
            }
            return 4;
        }
        /// <summary>
        /// 写入long
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static unsafe int WriteByte(long value, byte[] stream, int index)
        {
            fixed (byte* ptr = &stream[index])
            {
                *(long*)ptr = value;
            }
            return 8;
        }
        public static unsafe int WriteByte(ulong value, byte[] stream, int index)
        {
            fixed (byte* ptr = &stream[index])
            {
                *(ulong*)ptr = value;
            }
            return 8;
        }
        public static unsafe int WriteByte(short value, byte[] stream, int index)
        {
            fixed (byte* ptr = &stream[index])
            {
                *(short*)ptr = value;
            }
            return 2;
        }
        public static unsafe int WriteByte(ushort value, byte[] stream, int index)
        {
            fixed (byte* ptr = &stream[index])
            {
                *(ushort*)ptr = value;
            }
            return 2;
        }
        public static unsafe int WriteByte(double value, byte[] stream, int index)
        {
            fixed (byte* ptr = &stream[index])
            {
                *(double*)ptr = value;
            }
            return 8;
        }
        public static unsafe int WriteByte(float value, byte[] stream, int index)
        {
            fixed (byte* ptr = &stream[index])
            {
                *(float*)ptr = value;
            }
            return 4;
        }
    }
}
