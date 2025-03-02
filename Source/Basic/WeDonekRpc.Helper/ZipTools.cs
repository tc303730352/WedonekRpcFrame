using System.IO;
using System.IO.Compression;
using System.Text;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper
{
    public class ZipTools
    {
        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompression (byte[] data)
        {
            using (MemoryStream fs = new MemoryStream(data))
            {
                using (MemoryStream original = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress, true))//创建压缩对象
                    {
                        gzipStream.CopyTo(original);
                        gzipStream.Flush();
                    }
                    return original.ToArray();
                }
            }
        }

        public static byte[] Decompression (byte[] data, int offset, int size)
        {
            using (MemoryStream fs = new MemoryStream(data, offset, size))
            {
                using (MemoryStream original = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress, true))//创建压缩对象
                    {
                        gzipStream.CopyTo(original);
                        gzipStream.Flush();
                    }
                    return original.ToArray();
                }
            }
        }
        public static T DecompressionObject<T> (byte[] data)
        {
            string json = DecompressionString(data);
            return json.Json<T>();
        }
        public static string DecompressionString (byte[] data, int offset, int size)
        {
            byte[] sour = Decompression(data, offset, size);
            return Encoding.UTF8.GetString(sour);
        }
        /// <summary>
        /// 解压字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DecompressionString (byte[] data)
        {
            byte[] sour = Decompression(data);
            return Encoding.UTF8.GetString(sour);
        }
        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="str">压缩的字符串</param>
        /// <param name="len">返回原始字节长度</param>
        /// <returns></returns>
        public static byte[] CompressionString (string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return Compression(data);
        }
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="data">数据流</param>
        /// <returns>压缩的数据</returns>
        public static byte[] Compression (byte[] data)
        {
            using (MemoryStream fs = new MemoryStream())//创建文件
            {
                using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress, true))//创建压缩对象
                {
                    gzipStream.Write(data, 0, data.Length);
                    gzipStream.Flush();
                }
                return fs.ToArray();
            }
        }
        public static byte[] CompressionObject<T> (T data)
        {
            string json = JsonTools.Json(data);
            return CompressionString(json);
        }
        /// <summary>
        /// 压缩数据流
        /// </summary>
        /// <param name="input"></param>
        /// <param name="stream"></param>
        public static void CompressionStream (Stream input, Stream stream)
        {
            using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Compress, true))//创建压缩对象
            {
                input.CopyTo(gzipStream);
                gzipStream.Flush();
            }
        }
        /// <summary>
        /// 压缩保存成文件
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="file">文件</param>
        public static void CompressionFile (byte[] stream, FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
            else if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            using (Stream fs = file.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                _ = fs.Seek(0, SeekOrigin.Begin);
                using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress, true))//创建压缩对象
                {
                    gzipStream.Write(stream, 0, stream.Length);
                    gzipStream.Flush();
                }
            }
        }

        public static byte[] DecompressionFile (FileInfo file)
        {
            using (Stream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _ = fs.Seek(0, SeekOrigin.Begin);
                using (MemoryStream ms = new MemoryStream((int)( fs.Length * 1.5 )))
                {
                    using (GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress, true))//创建压缩对象
                    {
                        gzipStream.CopyTo(fs);
                        gzipStream.Flush();
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}
