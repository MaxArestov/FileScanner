namespace FileScanner.Models
{
    public class MimeTypeGroup
    {
        public double Count { get; set; }
        public double Value { get; set; }
        public Dictionary<string, MimeTypeGroup> MimeTypes { get; set; } = new Dictionary<string, MimeTypeGroup>();
        public double TotalFilesCount { get; set; }

        public MimeTypeGroup() { }
    }
}
