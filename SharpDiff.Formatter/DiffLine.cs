namespace SharpDiff.Formatter
{
    public class DiffLine
    {
        public DiffLine(string text, LineChangeType lineChangeType)
        {
            Text = text;
            LineChangeType = lineChangeType;
        }

        public int? OldLineNumber { get; set; }
        public int? NewLineNumber { get; set; }
        public string Text { get; set; }
        public LineChangeType LineChangeType { get; set; }
    }
}
