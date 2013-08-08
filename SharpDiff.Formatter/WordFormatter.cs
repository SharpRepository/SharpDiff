using System;
using System.Drawing;
using Novacode;

namespace SharpDiff.Formatter
{
    public class WordFormatter : BaseFormatter
    {
        private readonly string _outputPath;

        public WordFormatter(string outputPath)
        {
            _outputPath = outputPath;
        }

        public bool PageBreakBetweenFiles { get; set; }

        public override void Execute(Diff diff)
        {
            var doc = DocX.Create(_outputPath);
            doc.PageLayout.Orientation = Orientation.Landscape;
            // TODO: set margins smaller than defaults

            var needPageBreak = false;
            if (!String.IsNullOrEmpty(Title))
            {
                doc.InsertParagraph(Title).Bold().FontSize(24);
            }

            if (!String.IsNullOrEmpty(Summary))
            {
                doc.InsertParagraph(Summary).FontSize(12);
                needPageBreak = true;
            }

            if (needPageBreak)
            {
                PageBreakOrLineBreak(doc);
            }

            foreach (var file in diff.Files)
            {
                var paragraph = doc.InsertParagraph(file.FileName);
                paragraph.Bold().FontSize(20);

                if (file.FileChangeType == FileChangeType.Deleted)
                {
                    paragraph.Color(Color.DarkRed);

                    if (!ShowDeletedFileContents)
                    {
                        doc.InsertParagraph("[File Deleted]").FontSize(14).Bold().Color(Color.DarkRed);

                        PageBreakOrLineBreak(doc);

                        continue;                  
                    }
                }

                if (file.FileChangeType == FileChangeType.Created)
                {
                    paragraph.Color(Color.DarkGreen);

                    if (!ShowCreatedFileContents)
                    {
                        doc.InsertParagraph("[New File]").FontSize(14).Bold().Color(Color.DarkGreen);

                        PageBreakOrLineBreak(doc);

                        continue;
                    }
                }

                var table = doc.InsertTable(1, 3);
                table.AutoFit = AutoFit.Window;
                table.SetBorder(TableBorderType.Top, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));
                table.SetBorder(TableBorderType.Bottom, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));

                var i = 0;
                foreach (var section in file.FileSections)
                {
                    var row = i == 0 ? table.Rows[0] : table.InsertRow();
                    row.Cells[0].Width = 2.0;
                    row.Cells[1].Width = 2.0;
                    row.Cells[2].Width = 96.0;

                    row.Cells[0].Paragraphs[0].Append("...").FontSize(12).Color(Color.DarkGray);
                    row.Cells[1].Paragraphs[0].Append("...").FontSize(12).Color(Color.DarkGray);
                    row.Cells[2].Paragraphs[0].Append(section.Description).FontSize(12).Color(Color.DarkGray);

                    row.Cells[0].FillColor = Color.LightGray;
                    row.Cells[1].FillColor = Color.LightGray;
                    row.Cells[2].FillColor = Color.LightGray;

                    row.Cells[0].SetBorder(TableCellBorderType.Left, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));
                    row.Cells[0].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));
                    row.Cells[1].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));
                    row.Cells[2].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));

                    foreach (var line in section.Lines)
                    {
                        row = table.InsertRow();
                        row.Cells[0].Width = 2.0;
                        row.Cells[1].Width = 2.0;
                        row.Cells[2].Width = 96.0;

                        var fontColor = Color.FromArgb(1, 20, 20, 20);
                        var backgroundColor = Color.White;
                        var firstChar = " ";
                        switch (line.LineChangeType)
                        {
                            case LineChangeType.Add:
                                fontColor = Color.Black;
                                backgroundColor = Color.LightGreen;
                                firstChar = "+";
                                break;
                            case LineChangeType.Delete:
                                fontColor = Color.DarkRed;
                                backgroundColor = Color.LightPink;
                                firstChar = "-";
                                break;
                        }

                        row.Cells[0].Paragraphs[0].Append(line.OldLineNumber.ToString()).FontSize(12).Color(Color.DarkGray);
                        row.Cells[1].Paragraphs[0].Append(line.NewLineNumber.ToString()).FontSize(12).Color(Color.DarkGray);
                        row.Cells[2].Paragraphs[0].Append(firstChar + line.Text).FontSize(12).Color(fontColor);
                        
                        row.Cells[0].FillColor = backgroundColor;
                        row.Cells[1].FillColor = backgroundColor;
                        row.Cells[2].FillColor = backgroundColor;

                        row.Cells[0].SetBorder(TableCellBorderType.Left, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));
                        row.Cells[0].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));
                        row.Cells[1].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));
                        row.Cells[2].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, Color.LightGray));
                    }

                    i++;
                }

                PageBreakOrLineBreak(doc);
            }

            doc.Save();
        }
        
        private void PageBreakOrLineBreak(DocX doc)
        {
            if (PageBreakBetweenFiles)
            {
                doc.InsertParagraph().InsertPageBreakAfterSelf();
            }
            else
            {
                doc.InsertParagraph();
            }
        }
    }
}
