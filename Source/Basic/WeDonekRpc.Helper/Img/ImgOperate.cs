namespace WeDonekRpc.Helper.Img
{
    public class ImgOperate
    {
        /// <summary>
        /// 缩放宽度
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// 缩放高度
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// 剪切
        /// </summary>
        public CutImg Cut { get; set; }

        /// <summary>
        /// 旋转
        /// </summary>
        public int? Rotate { get; set; }
    }
    public class CutImg
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
