namespace FileScanner.Models
{
    public class Directories
    {
        public string DirectoryName { get; set; }
        public FileInfo[] Files { get; set; }
        public List<Directories> DirectoryInf { get; set; } = new List<Directories>();
    }
}
