using FileScanner.Models;
using System.Text;

namespace FileScanner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = $@"{Directory.GetCurrentDirectory()}";
            var fileScanner = new FileScanner(path);
            var mimeTypeStatistic = new MimeTypeGroup();
            var directoryToScan = fileScanner.GetDir(ref mimeTypeStatistic);
            var printer = new DirectoryPrinter(directoryToScan);
            StringBuilder html = printer.GenerateDirectoryTreeHtml(path, mimeTypeStatistic);
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\test.html", false))
            {
                sw.WriteLine(html.ToString());
            }
            
        }
    }
}