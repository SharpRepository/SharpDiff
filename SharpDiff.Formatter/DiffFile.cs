using System.Collections.Generic;

namespace SharpDiff.Formatter
{
    public class DiffFile
    {
        public DiffFile()
        {
            FileSections = new List<DiffFileSection>();
        }

        public string FileName { get; set; }
        public FileChangeType FileChangeType { get; set; }
        public IList<DiffFileSection> FileSections { get; set; }
    }
}
