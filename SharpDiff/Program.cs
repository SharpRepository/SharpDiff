using System;
using System.IO;
using SharpDiff.Formatter;

namespace SharpDiff
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (!File.Exists(options.DiffFile))
                {
                    Console.WriteLine("ERROR: diff file does not exist");
                }

                if (options.Verbose)
                {
                    Console.WriteLine("Processing {0}", options.DiffFile);
                }

                var reader = new DiffReader(options.DiffFile);
                var diff = reader.Read();

                if (!String.IsNullOrEmpty(options.WordOutput))
                {
                    if (options.Verbose) Console.WriteLine("Creating word output {0}", options.WordOutput);

                    var wordOutput = new WordFormatter(options.WordOutput)
                    {
                        Title = options.Title,
                        Summary = options.Summary,
                        ShowDeletedFileContents = options.ShowDeleteFileContents,
                        ShowCreatedFileContents = options.ShowCreatedFileContents
                    };
                    wordOutput.Execute(diff);
                }

                if (!String.IsNullOrEmpty(options.HtmlOutput))
                {
                    if (options.Verbose) Console.WriteLine("Creating html output {0}", options.HtmlOutput);

                    var htmlOutput = new HtmlFormatter(options.HtmlOutput)
                    {
                        Title = options.Title,
                        Summary = options.Summary,
                        ShowDeletedFileContents = options.ShowDeleteFileContents,
                        ShowCreatedFileContents = options.ShowCreatedFileContents
                    };
                    htmlOutput.Execute(diff);
                }

                if (options.Verbose || (String.IsNullOrEmpty(options.WordOutput) && String.IsNullOrEmpty(options.HtmlOutput)))
                {
                    var consoleOutput = new ConsoleFormatter();
                    consoleOutput.Execute(diff);
                }
            }



            if (options.Verbose)
            {
                Console.Write("Press enter to quit: ");
                Console.ReadKey();
            }

        }
    }
}
