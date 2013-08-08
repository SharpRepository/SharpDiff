namespace SharpDiff.Formatter
{
    public interface IDiffFormatter
    {
        bool ShowDeletedFileContents { get; set; }
        bool ShowCreatedFileContents { get; set; }

        void Execute(Diff diff);
    }
}
