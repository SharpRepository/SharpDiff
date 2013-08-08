using System;
using System.IO;
using System.Text;

namespace SharpDiff.Formatter
{
    public class HtmlFormatter : BaseFormatter
    {
        private readonly string _outputPath;

        public HtmlFormatter(string outputPath)
        {
            _outputPath = outputPath;
        }

        public override void Execute(Diff diff)
        {
            var html = new StringBuilder();

            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("   <style>");
            html.AppendLine("       .created { color: #006400; }");
            html.AppendLine("       .deleted { color: #8B0000; }");
            html.AppendLine("       table { border: 1px solid #D3D3D3; border-collapse:collapse; }");
            html.AppendLine("       td { border-left: 1px solid #D3D3D3; border-right: 1px solid #D3D3D3; }");
            html.AppendLine("       tr { color: #141414; }");
            html.AppendLine("       tr.section { color: #A9A9A9; background-color: #D3D3D3; }");
            html.AppendLine("       tr.addition { color: #000000; background-color: #90EE90; }");
            html.AppendLine("       tr.deletion { color: #8B0000; background-color: #FFB6C1; }");
            html.AppendLine("       td.lineNumber { color: #A9A9A9; }");
            html.AppendLine("   </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            if (!String.IsNullOrEmpty(Title))
            {
                html.AppendLine(String.Format("<h1>{0}</h1>", Title));
            }

            if (!String.IsNullOrEmpty(Summary))
            {
                html.AppendLine(String.Format("<div class\"summary\">{0}</div>", Summary));
            }

            foreach (var file in diff.Files)
            {
                var className = "";
                if (file.FileChangeType == FileChangeType.Deleted)
                {
                    className = "deleted";
                }
                else if (file.FileChangeType == FileChangeType.Created)
                {
                    className = "created";
                }

                html.AppendLine("   <h2 class=\"" +  className + "\">" + file.FileName + "</h2>");

                if (file.FileChangeType == FileChangeType.Deleted)
                {
                    if (!ShowDeletedFileContents)
                    {
                        html.AppendLine("   <div class=\"" + className + "\">[File Deleted]</div>");

                        continue;
                    }
                }

                if (file.FileChangeType == FileChangeType.Created)
                {
                    if (!ShowCreatedFileContents)
                    {
                        html.AppendLine("   <div class=\"" + className + "\">[New File]</div>");

                        continue;
                    }
                }

                html.AppendLine("   <table width=\"100%\">");

                foreach (var section in file.FileSections)
                {
                    html.AppendLine("   <tr class=\"section\">");
                    html.AppendLine("       <td width=\"2%\" class=\"lineNumber\">...</td>");
                    html.AppendLine("       <td width=\"2%\" class=\"lineNumber\">...</td>");
                    html.AppendLine("       <td width=\"96%\">" + section.Description + "</td>");
                    html.AppendLine("   </tr>");

                    foreach (var line in section.Lines)
                    {
                        className = "";
                        var firstChar = " ";
                        switch (line.LineChangeType)
                        {
                            case LineChangeType.Add:
                                className = "addition";
                                firstChar = "+";
                                break;
                            case LineChangeType.Delete:
                                className = "deletion";
                                firstChar = "-";
                                break;
                        }

                        html.AppendLine("   <tr class=\"" + className + "\">");
                        html.AppendLine("       <td class=\"lineNumber\">" + line.OldLineNumber + "</td>");
                        html.AppendLine("       <td class=\"lineNumber\">" + line.NewLineNumber + "</td>");
                        html.AppendLine("       <td >" + firstChar + line.Text + "</td>");
                        html.AppendLine("   </tr>");
                    }
                }

                html.AppendLine("   </table>");
            }


            html.AppendLine("</body>");
            html.AppendLine("</html>");


            if (File.Exists(_outputPath)) File.Delete(_outputPath);
            using (var outputFile = File.CreateText(_outputPath))
                outputFile.Write(html.ToString());


        }
    }
}
