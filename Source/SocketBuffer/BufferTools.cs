using SocketBuffer.Buffer;

namespace SocketBuffer
{
        internal class BufferTools
        {

                public static int GetBufferLen(int size)
                {
                        int len = size % 256;
                        if (len == 0)
                        {
                                return size;
                        }
                        return ((size / 256) + 1) * 256;
                }
                public static ISocketBuffer UsableBuffer(ISocketBuffer[] buffer, int size)
                {
                        foreach (ISocketBuffer i in buffer)
                        {
                                if (i == null)
                                {
                                        break;
                                }
                                else if (i.SourceSize >= size && i.UsableBuffer())
                                {
                                        return i;
                                }
                        }
                        return null;
                }

                public static ISocketBuffer UsableBuffer(ISocketBuffer[] buffer)
                {
                        foreach (ISocketBuffer i in buffer)
                        {
                                if (i.UsableBuffer())
                                {
                                        return i;
                                }
                        }
                        return null;
                }

                internal static ISocketBuffer AddBuffer(ISocketBuffer[] list, int index, int len)
                {
                        if (index < 0 || index >= list.Length)
                        {
                                return new TempBuffer(len);
                        }
                        else
                        {
                                ISocketBuffer buffer = new ExpansionBuffer(len);
                                list[index] = buffer;
                                return buffer;
                        }
                }
        }

}
