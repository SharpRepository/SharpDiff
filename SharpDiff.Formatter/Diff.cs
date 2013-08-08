using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpDiff.Formatter
{
    public class Diff
    {
        public Diff()
        {
            Files = new List<DiffFile>();
        }

        public IList<DiffFile> Files { get; set; }

        public void AddFile(string filename, FileChangeType fileChangeType)
        {
            if (CurrentFile != null && CurrentFile.FileName == filename) return;

            Files.Add(new DiffFile() { FileName = filename, FileChangeType = fileChangeType });
        }

        public void AddFileSection(string description)
        {
            if (CurrentFile == null) throw new ArgumentException("Need a file first");

            CurrentFile.FileSections.Add(new DiffFileSection(description));
        }

        private DiffFile CurrentFile
        {
            get { return Files.LastOrDefault(); }
        }

        private DiffFileSection CurrentFileSection
        {
            get { return CurrentFile == null ? null : CurrentFile.FileSections.LastOrDefault(); }
        }

        private FileChangeType _currentChangeType = FileChangeType.Unknown;

        public void ProcessLine(string line)
        {
            if (line.StartsWith("diff "))
            {
                _currentChangeType = FileChangeType.Unknown;
                return;
            }

            if (line.StartsWith("index "))
            {
                return;
            }

            if (line.StartsWith("deleted file"))
            {
                _currentChangeType = FileChangeType.Deleted;
                return;
            }

            if (line.StartsWith("new file"))
            {
                _currentChangeType = FileChangeType.Created;
                return;
            }

            if (line.StartsWith("---") || line.StartsWith("+++"))
            {
                // see we have a filename to show
                if (line.Substring(4) != "/dev/null")
                {
                    AddFile(line.Substring(6), _currentChangeType);
                }

                return;
            }

            if (line.StartsWith("@@"))
            {
                AddFileSection(line);
                return;
            }

            if (line.StartsWith("-"))
            {
                CurrentFileSection.AddLine(new DiffLine(line.Substring(1), LineChangeType.Delete));
                return;
            }

            if (line.StartsWith("+"))
            {
                CurrentFileSection.AddLine(new DiffLine(line.Substring(1), LineChangeType.Add));
                return;
            }

            CurrentFileSection.AddLine(new DiffLine(line.Substring(1), LineChangeType.None));
        }
    }
}
