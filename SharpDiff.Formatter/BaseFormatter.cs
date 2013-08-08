namespace SharpDiff.Formatter
{
    public abstract class BaseFormatter : IDiffFormatter
    {
        public string Title { get; set; }
        public string Summary { get; set; }

        public bool ShowDeletedFileContents { get; set; }
        public bool ShowCreatedFileContents { get; set; }

        public abstract void Execute(Diff diff);
    }
}
