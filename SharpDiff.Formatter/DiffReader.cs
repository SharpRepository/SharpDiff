using System.IO;

namespace SharpDiff.Formatter
{
    public class DiffReader
    {
        public string FilePath { get; set; }

        public DiffReader(string filePath)
        {
            FilePath = filePath;
        }

        public Diff Read()
        {
            using (var stream = File.OpenText(FilePath))
            {
                var diff = new Diff();

                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    // process each line into the word doc
                    diff.ProcessLine(line);
                }

                return diff;
            }
        }
    }
}
