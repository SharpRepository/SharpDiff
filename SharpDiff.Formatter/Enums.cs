namespace SharpDiff.Formatter
{
    public enum LineChangeType
    {
        None,
        Add,
        Delete
    }

    public enum FileChangeType
    {
        Unknown,
        Deleted,
        Changed,
        Created
    }
}
