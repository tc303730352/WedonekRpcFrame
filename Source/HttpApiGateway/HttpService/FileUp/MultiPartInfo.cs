namespace HttpService
{
        internal class MultiPartInfo
        {
                public int BeginIndex { get; set; }
                public int DataLen { get; internal set; }
                public string Disposition { get; set; }
                public string Name { get; set; }

                public string FileName { get; set; }

                public string ContentType { get; set; }
        }
}
