using CommandLine;
using CommandLine.Text;

namespace SharpDiff
{
    class Options
    {
        [Option('f', "file", Required = true, HelpText = "Path to the diff file")]
        public string DiffFile { get; set; }

        [Option("word-output", HelpText = "Path to the Word Output file")]
        public string WordOutput { get; set; }

        [Option("html-output", HelpText = "Path to the HTML Output file")]
        public string HtmlOutput { get; set; }

        [Option("deleted-contents", DefaultValue = false, HelpText = "Show all of the lines from a deleted file")]
        public bool ShowDeleteFileContents { get; set; }

        [Option("created-contents", DefaultValue = false, HelpText = "Show all of the lines from a newly created file")]
        public bool ShowCreatedFileContents { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('t', "title", HelpText = "Header title")]
        public string Title { get; set; }

        [Option('s', "summary", HelpText = "Sumary text")]
        public string Summary { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, text => HelpText.DefaultParsingErrorsHandler(this, text));
        }
    }
}
