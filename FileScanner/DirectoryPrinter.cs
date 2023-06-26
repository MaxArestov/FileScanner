using FileScanner.Models;
using MimeTypes.Core;
using System.Text;

namespace FileScanner
{
    public class DirectoryPrinter
    {
        public Directories Directories { get; set; }

        public DirectoryPrinter(Directories directories)
        {
            this.Directories = directories;
        }

        public StringBuilder GenerateDirectoryTreeHtml(string path, MimeTypeGroup mime)
        {
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
            StringBuilder html = new StringBuilder();
            StringBuilder statistic = new StringBuilder();
            statistic.Append($"<li>Total count: {mime.TotalFilesCount}</li>");
            foreach (var mim in mime.MimeTypes)
            {
                statistic.Append($"<li>{mim.Key}: {mim.Value.Count} / {mime.TotalFilesCount} ({Math.Round((mim.Value.Count / mime.TotalFilesCount * 100), 5)}%). Average value: {Math.Round(mim.Value.Value / mim.Value.Count, 2)} bytes</li>");
            }
            html.Append("<ul>");
            html.Append(GenerateChildItemsHtml(rootDirectoryInfo));
            html.Append("</ul>");
            StringBuilder fullHtml = new StringBuilder($@"
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    .hidden {{ display: none; }}
                    .folder {{ font-weight: bold; cursor: pointer; }}
                    .folder {{
                        list-style-type: none;
                        color: #a88f32;
                    }}
                    .folder:before {{
                        content: ""+"";
                    }}
                    .row{{
                       display: flex;
                       justify-content: space-between;
                       width: 40%;
                    }}
                    .fileItem{{
                        text-overflow: ellipsis;
                        white-space: nowrap;
                        overflow: hidden;
                        width: 60%;
                        margin-right:5px;
                    }}
                    .fileSizeItem{{
                        white-space: nowrap;
                        width: 20%;
                    }}
                </style>
            </head>
            <body>
                {html}
                {statistic}
                <script>
                    var folders = document.getElementsByClassName('folder');
                    for (var i = 0; i < folders.length; i++) {{
                        folders[i].addEventListener('click', function() {{
                            this.nextElementSibling.classList.toggle('hidden');
                        }});
                    }}
                </script>
            </body>
            </html>");
            return fullHtml;
        }

        public static StringBuilder GenerateChildItemsHtml(DirectoryInfo directoryInfo)
        {
            StringBuilder html = new StringBuilder();

            foreach (var file in directoryInfo.GetFiles())
            {
                string mimeType = MimeTypeMap.GetMimeType(file.Extension);
                html.Append(@$"
                <li>
                    <div class='row'>
                        <div class='fileItem'>
                            {file.Name}
                        </div>
                        <div class='fileSizeItem'>
                            {file.Length} bytes
                        </div>
                        <div class='fileSizeItem'>
                            {mimeType}
                        </div>
                    </div>
                </li>");
            }

            foreach (var directory in directoryInfo.GetDirectories())
            {
                html.Append($"<li class='folder'>{directory.Name}</li>");
                html.Append("<ul class='hidden'>");
                html.Append(GenerateChildItemsHtml(directory));
                html.Append("</ul>");
            }

            return html;
        }

    }
}