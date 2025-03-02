using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace WeDonekRpc.Helper.Img
{
    public class ImageTools
    {
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Stream Resize (FileInfo file, int? width, int? height)
        {
            using (Image original = Image.Load(file.FullName))
            {
                IImageEncoder encoder = _GetEncoder(file);
                original.Mutate(x => x.Resize(width.GetValueOrDefault(), height.GetValueOrDefault()));
                MemoryStream stream = new MemoryStream();
                original.Save(stream, encoder);
                stream.Position = 0;
                return stream;
            }
        }
        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ratate"></param>
        /// <returns></returns>
        public static Stream Rotate (FileInfo file, int ratate)
        {
            using (Image original = Image.Load(file.FullName))
            {
                IImageEncoder encoder = _GetEncoder(file);
                original.Mutate(x => x.Rotate(ratate));
                MemoryStream stream = new MemoryStream();
                original.Save(stream, encoder);
                stream.Position = 0;
                return stream;
            }
        }
        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Stream Cut (FileInfo file, int x, int y, int width, int height)
        {
            using (Image original = Image.Load(file.FullName))
            {
                IImageEncoder encoder = _GetEncoder(file);
                original.Mutate(a =>
                {
                    Rectangle range = new Rectangle(x, y, width, height);
                    _ = a.Crop(range);
                });
                MemoryStream stream = new MemoryStream();
                original.Save(stream, encoder);
                stream.Position = 0;
                return stream;
            }
        }
        /// <summary>
        /// 图片操作
        /// </summary>
        /// <param name="file"></param>
        /// <param name="operate"></param>
        /// <returns></returns>
        public static Stream ImgOperate (FileInfo file, ImgOperate operate)
        {
            using (Image original = Image.Load(file.FullName))
            {
                IImageEncoder encoder = _GetEncoder(file);
                original.Mutate(x =>
                {
                    if (operate.Rotate.HasValue)
                    {
                        _ = x.Rotate(operate.Rotate.Value);
                    }
                    if (operate.Cut != null)
                    {
                        CutImg cut = operate.Cut;
                        Rectangle range = new Rectangle(cut.X, cut.Y, cut.Width, cut.Height);
                        _ = x.Crop(range);
                    }
                    if (operate.Width.HasValue || operate.Height.HasValue)
                    {
                        original.Mutate(x => x.Resize(operate.Width.GetValueOrDefault(), operate.Height.GetValueOrDefault()));
                    }
                });
                MemoryStream stream = new MemoryStream();
                original.Save(stream, encoder);
                stream.Position = 0;
                return stream;
            }
        }

        private static IImageEncoder _GetEncoder (FileInfo file)
        {
            switch (file.Extension)
            {
                case ".png":
                    return new PngEncoder();
                case ".jpg":
                    return new JpegEncoder();
                case ".jpeg":
                    return new JpegEncoder();
                case ".bmp":
                    return new BmpEncoder();
                case ".gif":
                    return new GifEncoder();
                default:
                    return new PngEncoder();
            }
        }
    }
}
