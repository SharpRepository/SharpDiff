using System;
using System.Collections.Generic;

namespace SharpDiff.Formatter
{
    public class DiffFileSection
    {
        public DiffFileSection(string description)
        {
            Lines = new List<DiffLine>();

            var shortDescription = description.Substring(0, description.IndexOf("@@", 3) + 2);

            Description = description;

            var parts = shortDescription.Replace("@@", "").Trim().Split(' ');

            OldLineNumber = Int32.Parse(parts[0].Split(',')[0].Replace("-", "")) - 1;
            NewLineNumber = Int32.Parse(parts[1].Split(',')[0].Replace("+", "")) - 1;
        }

        public string Description { get; set; }
        public IList<DiffLine> Lines { get; set; }
        public int OldLineNumber { get; set; }
        public int NewLineNumber { get; set; }

        public void AddLine(DiffLine line)
        {
            switch (line.LineChangeType)
            {
                case LineChangeType.Add:
                    NewLineNumber++;
                    line.NewLineNumber = NewLineNumber;

                    break;
                case LineChangeType.Delete:
                    OldLineNumber++;
                    line.OldLineNumber = OldLineNumber;

                    break;
                default:
                    NewLineNumber++;
                    OldLineNumber++;

                    line.NewLineNumber = NewLineNumber;
                    line.OldLineNumber = OldLineNumber;

                    break;
            }

            Lines.Add(line);
        }
    }
}
